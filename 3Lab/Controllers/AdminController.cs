﻿using Microsoft.AspNetCore.Mvc;
using PetStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using PetStore.Models.MongoDb;
using Microsoft.AspNetCore.Http;
using System;
using PetStore.Filters.FilterParameters;
using PetStore.Filters;
using PetStore.Models.ViewModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using _3Lab.Models.ViewModels;
using _3Lab.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using LearningEngine.Api.Extensions;

namespace PetStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ImagesDbContext _imagesDb;
        private IStockRepository _stockRepository;
        private IProductRepository _productRepository;
        private IProductExtendedRepository _productExtendedRepository;
        private IOrderRepository _orderRepository;
        private ICategoryRepository _categoryRepository;
        private ApplicationDbContext _context;
        private IEFUserRepository _userRepository;
        private IFilterConditionsProducts _filterConditions;
        private int PageSize = 5;

        public AdminController(IProductRepository repo,
                               IEFUserRepository userRepository,
                               IStockRepository stockRepo,
                               IProductExtendedRepository productExtendedRepository,
                               IOrderRepository orderRepository,
                               ICategoryRepository categoryRepository,
                               ImagesDbContext context,
                               IFilterConditionsProducts filterConditions,
                               ApplicationDbContext dbcontext)
        {
            _context = dbcontext;
            _productRepository = repo;
            _stockRepository = stockRepo;
            _productExtendedRepository = productExtendedRepository;
            _orderRepository = orderRepository;
            _categoryRepository = categoryRepository;
            _imagesDb = context;
            _filterConditions = filterConditions;
            _userRepository = userRepository;
        }

        [HttpPost("GetData")]
        public async Task<ActionResult> Index([FromForm] FilterParametersProducts filter, [FromForm] int productPage = 1)
        {
            var categories = new List<int>();

            if (filter.Categories != null)
            {
                var categoriesStrings = filter.Categories.Split(';');

                foreach (var category in categoriesStrings)
                {
                    categories.Add(Convert.ToInt32(category));
                }

                filter.CategoriesList = categories;
            }

            var stock = _stockRepository.StockItems;
            stock = _filterConditions.GetStockProducts(stock, filter);

            var paging = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = filter.Categories == null ?
                        stock.Count() :
                        stock.Where(e =>
                             categories.Contains(e.Product.Category.ID)).Count()
            };

            return Ok(new AdminProductsListViewModel
            {
                Stock = stock
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = paging,
                CurrentFilter = filter
            });
        }

        [HttpPost("SearchList")]
        public async Task<ActionResult> SearchList([FromForm] FilterParametersProducts filter, [FromForm] int productPage = 1)
        {
            var categories = new List<int>();

            if (filter.Categories != null)
            {
                var categoriesStrings = filter.Categories.Split(';');

                foreach (var category in categoriesStrings)
                {
                    categories.Add(Convert.ToInt32(category));
                }

                filter.CategoriesList = categories;
            }

            var stock = _stockRepository.StockItems;
            stock = _filterConditions.GetStockProducts(stock, filter);

            var paging = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = filter.Categories == null ?
                        stock.Count() :
                        stock.Where(e =>
                             categories.Contains(e.Product.Category.ID)).Count()
            };

            if (stock.Count() == 0)
            {
                return BadRequest("No results");
            }

            return Ok(new AdminProductsListViewModel
            {
                Stock = stock
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = paging,
                CurrentFilter = filter,
                Categories = _stockRepository.StockItems
                    .Select(x => x.Product.Category)
                    .Distinct()
                    .OrderBy(x => x).ToList()
            });
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            return Ok(_categoryRepository.Categories);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] ProductWithCategoryViewModel productExtended)
        {
            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == this.GetUserRole());

            if (role.CanAddProducts == false)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                if (productExtended.Product.Product.Image != null)
                {
                    var imageName = DateTime.Now.ToString() + productExtended.Product.Product.Name;
                    var image = await _imagesDb.StoreImage(productExtended.Product.Product.Image.OpenReadStream(),
                                                            imageName);

                    productExtended.Product.Product.ImageId = image;
                }

                productExtended.Product.Product.Category = _categoryRepository.Categories
                    .FirstOrDefault(c => c.ID == productExtended.Product.Product.Category.ID);

                _productExtendedRepository.SaveProductExtended(productExtended.Product);

                _stockRepository.SaveStockItem(new Stock { Product = productExtended.Product.Product, Quantity = 0 });

                return Ok();
            }
            else
            {
                return BadRequest(productExtended);
            }
        }

        [HttpPost("EditModel")]
        public async Task<IActionResult> Edit([FromForm] int productId)
        {
            var result = _productExtendedRepository.ProductsExtended
                .FirstOrDefault(p => p.Product.ID == productId);

            if (result == null)
            {
                return BadRequest();
            }

            return Ok(new ProductWithCategoryViewModel { Categories = _categoryRepository.Categories, Product = result });
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromForm] ProductWithCategoryViewModel productExtended, [FromForm] int id)
        {
            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == this.GetUserRole());

            if (role.CanEditProducts == false)
            {
                return Forbid();
            }


            if (ModelState.IsValid)
            {
                if (productExtended.ID <= 0)
                {
                    return BadRequest();
                }

                var product = _productExtendedRepository.ProductsExtended
                    .FirstOrDefault(p => p.ID == productExtended.ID);

                if (productExtended.Product.Product.Image != null)
                {
                    var imageName = DateTime.Now.ToString() + productExtended.Product.Product.Name;
                    var image = await _imagesDb.StoreImage(productExtended.Product.Product.Image.OpenReadStream(),
                                                            imageName);

                    productExtended.Product.Product.ImageId = image;
                }

                product.Product.ID = productExtended.Product.ProductIdentifier;
                product.Product.Name = productExtended.Product.Product.Name;
                product.Product.ImageId = productExtended.Product.Product.ImageId;
                product.Product.Price = productExtended.Product.Product.Price;
                var category = _categoryRepository.Categories.FirstOrDefault(c => c.ID == productExtended.Product.Product.Category.ID);
                product.Product.Category = category;
                product.Product.Description = productExtended.Product.Product.Description;
                product.LongDescription = productExtended.Product.LongDescription;
                product.Manufacturer = productExtended.Product.Manufacturer;
                product.OriginCountry = productExtended.Product.OriginCountry;
                product.Image = productExtended.Product.Image;
                product.Comments = productExtended.Product.Comments;

                try
                {
                    _productRepository.SaveProduct(product.Product);
                    _productExtendedRepository.SaveProductExtended(product);
                }
                catch(DbUpdateConcurrencyException ex)
                {
                    return BadRequest("The product is already editing");
                }

                return Ok();
            }
            else
            {
                // there is something wrong with the data values
                return BadRequest(productExtended);
            }
        }

        [HttpPost("Delete")]
        public async Task<IActionResult> Delete([FromForm] int productId)
        {
            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == this.GetUserRole());

            if (role.CanDeleteProducts == false)
            {
                return Forbid();
            }

            List<int> ids = _orderRepository.Orders.Where(o => o.Lines.Any(l => l.Product.ID == productId)).Select(o => o.OrderID).ToList();
            foreach (var id in ids)
            {
                var deletedOrder = _orderRepository.DeleteOrder(id);
            }

            var stockId = _stockRepository.StockItems.FirstOrDefault(s => s.Product.ID == productId).ID;
            var deletedStock = _stockRepository.DeleteStockItem(stockId);

            var extendedId = _productExtendedRepository.ProductsExtended.FirstOrDefault(p => p.Product.ID == productId).ID;
            var deletedExtended = _productExtendedRepository.DeleteProductExtended(extendedId);

            var deletedProduct = _productRepository.DeleteProduct(productId);

            return Ok();
        }

        [HttpPost("AddToStock")]
        public async Task<IActionResult> AddToStock([FromForm] int productId, [FromForm] int quantity)
        {
            var role = await _context.UserRole.FirstOrDefaultAsync(role => role.Id == this.GetUserRole());

            if (role.CanPurchaseToStock == false)
            {
                return Forbid();
            }

            var stock = _stockRepository.StockItems.FirstOrDefault(s => s.Product.ID == productId);

            stock.Quantity += quantity;
            _stockRepository.SaveStockItem(stock);

            return Ok();
        }

        [HttpGet("GetImage")]
        public async Task<ActionResult> GetImage([FromForm] string id)
        {
            var image = await _imagesDb.GetImage(id);
            if (image == null)
            {
                return NotFound();
            }
            return File(image, "image/png");
        }

        [HttpPost("AttachImage")]
        public async Task<ActionResult> AttachImage([FromForm] int id, [FromForm] string image)
        {
            Product product = _productRepository.Products.FirstOrDefault(p => p.ID == id);
            product.ImageId = image;

            _productRepository.SaveProduct(product);

            return Ok();
        }
    }
}