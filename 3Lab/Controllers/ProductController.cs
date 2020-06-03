using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetStore.Filters;
using PetStore.Filters.FilterParameters;
using PetStore.Models;
using PetStore.Models.MongoDb;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        #region fields

        private readonly ImagesDbContext _imagesDb;

        private IProductRepository _repository;

        private IStockRepository _stockRepository;

        private IProductExtendedRepository _productExtendedRepository;

        private ICategoryRepository _categoryRepository;

        private IFilterConditionsProducts _filterConditions;

        public int PageSize = 4;

        #endregion

        public ProductController(IProductRepository repository,
                                IStockRepository stockRepository,
                                IProductExtendedRepository productExtendedRepository,
                                ICategoryRepository categoryRepository,
                                ImagesDbContext context,
                                IFilterConditionsProducts filterConditions)
        {
            _repository = repository;
            _stockRepository = stockRepository;
            _productExtendedRepository = productExtendedRepository;
            _categoryRepository = categoryRepository;
            _imagesDb = context;
            _filterConditions = filterConditions;
        }

        [HttpPost("GetAll")]
        public async Task<ActionResult> List([FromForm]FilterParametersProducts filter, [FromForm]int productPage = 1)
        {
            var categories = new List<int>();

            if (!String.IsNullOrEmpty(filter.Categories))
            {
                var categoriesStrings = filter.Categories.Split(';');

                foreach (var category in categoriesStrings)
                {
                    categories.Add(Convert.ToInt32(category));
                }

                filter.CategoriesList = categories;

                int addedCount = 0;

                do
                {
                    addedCount = 0;
                    List<int> toAdd = new List<int>();

                    foreach (int categoryID in categories)
                    {
                        foreach (var id in _categoryRepository.Categories
                            .FirstOrDefault(c => c.ID == categoryID).Children
                            .Where(c => !categories.Contains(c.ID))
                            .Select(c => c.ID))
                        {
                            toAdd.Add(id);
                            addedCount++;
                        }
                    }

                    categories.AddRange(toAdd);
                }
                while (addedCount > 0);

                filter.CategoriesList = categories;
            }

            var products = _repository.Products;
            products = _filterConditions.GetProducts(products, filter);

            foreach (var p in products)
            {
                if (_stockRepository.StockItems.FirstOrDefault(pr => pr.Product == p && pr.Quantity > 0) != null)
                {
                    p.IsInStock = true;
                }
            }

            var paging = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = filter.Categories == null ?
                        products.Count() :
                        products.Where(e =>
                             categories.Contains(e.Category.ID)).Count()
            };

            return Ok(new ProductsListViewModel
            {
                Products = products
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = paging,
                CurrentFilter = filter
            });
        }

        [HttpGet("SearchList")]
        public async Task<ActionResult> SearchList([FromForm]FilterParametersProducts filter, [FromForm]int productPage = 1)
        {
            var products = _repository.Products;
            products = _filterConditions.GetProducts(products, filter);

            var categories = new List<int>();
            if (!String.IsNullOrEmpty(filter.Categories))
            {
                var categoriesStrings = filter.Categories.Split(';');

                foreach (var category in categoriesStrings)
                {
                    categories.Add(Convert.ToInt32(category));
                }
            }

            filter.CategoriesList = categories;

            foreach (var p in products)
            {
                if (_stockRepository.StockItems.FirstOrDefault(pr => pr.Product == p && pr.Quantity > 0) != null)
                {
                    p.IsInStock = true;
                }
            }

            var paging = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItems = filter.Categories == null ?
                        products.Count() :
                        products.Where(e =>
                             categories.Contains(e.Category.ID)).Count()
            };

            if (products.Count() == 0)
            {
                return BadRequest("No result");
            }

            return Ok(new ProductsListViewModel
            {
                Products = products
                    .Skip((productPage - 1) * PageSize)
                    .Take(PageSize),
                PagingInfo = paging,
                CurrentFilter = filter,
                Categories = _repository.Products
                    .Select(x => x.Category)
                    .Distinct()
                    .OrderBy(x => x).ToList()
            });
        }

        [HttpPost("GetInfo")]
        public async Task<ActionResult> Info([FromForm]int productId)
        {
            var result = _productExtendedRepository.ProductsExtended
                    .FirstOrDefault(p => p.Product.ID == productId);

            if (result == null)
            {
                return BadRequest("No result");
            }

            if (_stockRepository.StockItems.FirstOrDefault(p => p.Product.ID == result.Product.ID && p.Quantity > 0) != null)
            {
                result.Product.IsInStock = true;
            }

            return Ok(result);
        }

        [HttpGet("Image")]
        public async Task<ActionResult> GetImage([FromForm]string id)
        {
            var image = await _imagesDb.GetImage(id);

            if (image == null)
            {
                return NotFound();
            }

            return File(image, "image/png");
        }
    }
}
