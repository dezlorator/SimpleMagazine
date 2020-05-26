using Microsoft.AspNetCore.Mvc;
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
using System.Net.Http;
using Client.Models;
using Newtonsoft.Json;

namespace PetStore.Controllers
{
    public class AdminController : Controller
    {
        private readonly ImagesDbContext _imagesDb;
        private int PageSize = 5;
        private readonly string _apiPathGetData = "http://localhost:62029/api/admin/GetData";
        private readonly string _apiPathSearchList = "http://localhost:62029/api/admin/SearchList";
        private readonly string _apiPathGetCategories = "http://localhost:62029/api/admin/GetCategories";
        private readonly string _apiPathCreate = "http://localhost:62029/api/admin/Create";

        private readonly string _apiPathGetModel = "http://localhost:62029/api/comment/GetModel";
        private readonly string _apiPath = "http://localhost:62029/api/comment";
        private readonly string _apiPathDelete = "http://localhost:62029/api/comment/Delete";

        public AdminController(ImagesDbContext context)
        {
            _imagesDb = context;
        }

        public async Task<ViewResult> Index(FilterParametersProducts filter, int productPage = 1)
        {
            ViewBag.Current = "Products";

            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();

                    data.Add(new StringContent(filter.Name), "Name");
                    data.Add(new StringContent(filter.MinPrice.ToString()), "MinPrice");
                    data.Add(new StringContent(filter.MaxPrice.ToString()), "MaxPrice");
                    data.Add(new StringContent(filter.Category.ToString()), "Category");
                    data.Add(new StringContent(productPage.ToString()), "productPage");

                    string categoryCostyl = String.Empty;

                    if (filter.Categories != null)
                    {
                        foreach (var category in filter.Categories)
                        {
                            categoryCostyl += category.ToString() + ';';
                        }

                        categoryCostyl = categoryCostyl.Substring(0, categoryCostyl.Length - 2);
                    }


                    data.Add(new StringContent(categoryCostyl), "Categories");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathGetData, data);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<AdminProductsListViewModel>(json);

                    if (obj.CurrentFilter.CategoriesList == null)
                    {
                        obj.CurrentFilter.CategoriesList = new List<int>() { 1, 2, 3 };
                    }

                    return View(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewResult> SearchList(FilterParametersProducts filter, int productPage = 1)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();

                    data.Add(new StringContent(filter.Name), "Name");
                    data.Add(new StringContent(filter.MinPrice.ToString()), "MinPrice");
                    data.Add(new StringContent(filter.MaxPrice.ToString()), "MaxPrice");
                    data.Add(new StringContent(filter.Category.ToString()), "Category");
                    data.Add(new StringContent(productPage.ToString()), "productPage");

                    string categoryCostyl = String.Empty;

                    if (filter.Categories != null)
                    {
                        foreach (var category in filter.Categories)
                        {
                            categoryCostyl += category.ToString() + ';';
                        }

                        categoryCostyl = categoryCostyl.Substring(0, categoryCostyl.Length - 2);
                    }


                    data.Add(new StringContent(categoryCostyl), "Categories");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathSearchList, data);

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        TempData["message"] = $"Поиск не дал результатов";

                        return View(new AdminProductsListViewModel());
                    }

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<AdminProductsListViewModel>(json);

                    return View(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewResult> Create()
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.GetAsync(_apiPathGetCategories);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<List<CategoryNode>>(json);

                    return View(new ProductWithCategoryViewModel
                    {
                        Product = new ProductExtended(),
                        Categories = obj.AsQueryable()
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(ProductWithCategoryViewModel productExtended)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();

                    productExtended.Product.Product.ImageId = String.Empty;

                    if (productExtended.Product.Product.Image != null)
                    {
                        var imageName = DateTime.Now.ToString() + productExtended.Product.Product.Name;
                        var image = await _imagesDb.StoreImage(productExtended.Product.Product.Image.OpenReadStream(),
                                                                imageName);

                        productExtended.Product.Product.ImageId = image;
                    }

                    data.Add(new StringContent(productExtended.ID.ToString()), "ID");
                    data.Add(new StringContent(productExtended.Product.ID.ToString()), "Product.ID");
                    data.Add(new StringContent(productExtended.Product.LongDescription), "Product.LongDescription");
                    data.Add(new StringContent(productExtended.Product.Manufacturer), "Product.Manufacturer");
                    data.Add(new StringContent(productExtended.Product.OriginCountry), "Product.OriginCountry");
                    data.Add(new StringContent(productExtended.Product.Product.ID.ToString()), "Product.Product.ID");
                    data.Add(new StringContent(productExtended.Product.Product.Name), "Product.Product.Name");
                    data.Add(new StringContent(productExtended.Product.Product.Description), "Product.Product.Description");
                    data.Add(new StringContent(productExtended.Product.Product.Price.ToString()), "Product.Product.Price");
                    data.Add(new StringContent(productExtended.Product.Product.ImageId), "Product.Product.ImageId");
                    data.Add(new StringContent(productExtended.Product.Product.Category.ID.ToString()), "Product.Product.Category.ID");


                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathCreate, data);

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        return View(productExtended);
                    }

                    TempData["message"] = $"{productExtended.Product.Product.Name} был сохранен";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //[Authorize(Roles = "Admin, Manager")]
        //public IActionResult Edit(int productId)
        //{
        //    var result = _productExtendedRepository.ProductsExtended
        //        .FirstOrDefault(p => p.Product.ID == productId);

        //    if (result == null)
        //    {
        //        TempData["message"] = $"Ошибка";
        //        return RedirectToAction("List");
        //    }

        //    return View(new ProductWithCategoryViewModel { Categories = _categoryRepository.Categories, Product = result });
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin, Manager")]
        //public async Task<IActionResult> Edit(ProductWithCategoryViewModel productExtended, int id)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        if (productExtended.ID <= 0)
        //        {
        //            TempData["message"] = $"Ошибка сохранения";
        //            return RedirectToAction("Index");
        //        }

        //        var product = _productExtendedRepository.ProductsExtended
        //            .FirstOrDefault(p => p.ID == productExtended.ID);

        //        if (productExtended.Product.Product.Image != null)
        //        {
        //            var imageName = DateTime.Now.ToString() + productExtended.Product.Product.Name;
        //            var image = await _imagesDb.StoreImage(productExtended.Product.Product.Image.OpenReadStream(),
        //                                                    imageName);

        //            productExtended.Product.Product.ImageId = image;
        //        }

        //        product.Product.ID = productExtended.Product.ProductIdentifier;
        //        product.Product.Name = productExtended.Product.Product.Name;
        //        product.Product.ImageId = productExtended.Product.Product.ImageId;
        //        product.Product.Price = productExtended.Product.Product.Price;
        //        var category = _categoryRepository.Categories.FirstOrDefault(c => c.ID == productExtended.Product.Product.Category.ID);
        //        product.Product.Category = category;
        //        product.Product.Description = productExtended.Product.Product.Description;
        //        product.LongDescription = productExtended.Product.LongDescription;
        //        product.Manufacturer = productExtended.Product.Manufacturer;
        //        product.OriginCountry = productExtended.Product.OriginCountry;
        //        product.Image = productExtended.Product.Image;
        //        product.Comments = productExtended.Product.Comments;

        //        _productRepository.SaveProduct(product.Product);
        //        _productExtendedRepository.SaveProductExtended(product);
        //        TempData["message"] = $"{productExtended.Product.Product.Name} был сохранен";

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        // there is something wrong with the data values
        //        return View(productExtended);
        //    }
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin")]
        //public IActionResult Delete(int productId)
        //{
        //    List<int> ids = _orderRepository.Orders.Where(o => o.Lines.Any(l => l.Product.ID == productId)).Select(o => o.OrderID).ToList();
        //    foreach (var id in ids)
        //    {
        //        var deletedOrder = _orderRepository.DeleteOrder(id);
        //    }

        //    var stockId = _stockRepository.StockItems.FirstOrDefault(s => s.Product.ID == productId).ID;
        //    var deletedStock = _stockRepository.DeleteStockItem(stockId);

        //    var extendedId = _productExtendedRepository.ProductsExtended.FirstOrDefault(p => p.Product.ID == productId).ID;
        //    var deletedExtended = _productExtendedRepository.DeleteProductExtended(extendedId);

        //    var deletedProduct = _productRepository.DeleteProduct(productId);

        //    if (deletedProduct != null && deletedStock != null && deletedExtended != null)
        //    {
        //        TempData["message"] = $"{deletedProduct.Name} был удален";
        //    }

        //    return RedirectToAction("Index");
        //}

        //[HttpPost]
        //[Authorize(Roles = "Admin, Manager")]
        //public IActionResult AddToStock(int productId, int quantity)
        //{
        //    var stock = _stockRepository.StockItems.FirstOrDefault(s => s.Product.ID == productId);

        //    stock.Quantity += quantity;
        //    _stockRepository.SaveStockItem(stock);

        //    return RedirectToAction("Index");
        //}

        //public async Task<ActionResult> GetImage(string id)
        //{
        //    var image = await _imagesDb.GetImage(id);
        //    if (image == null)
        //    {
        //        return NotFound();
        //    }
        //    return File(image, "image/png");
        //}

        //[HttpPost]
        //public async Task<ActionResult> AttachImage(int id, IFormFile uploadedFile)
        //{
        //    if (uploadedFile != null)
        //    {
        //        var image = await _imagesDb.StoreImage(uploadedFile.OpenReadStream(), uploadedFile.FileName);

        //        Product product = _productRepository.Products.FirstOrDefault(p => p.ID == id);
        //        product.ImageId = image;

        //        _productRepository.SaveProduct(product);
        //        TempData["message"] = $"{product.Name} был сохранен";
        //    }

        //    return RedirectToAction("Index");
        //}
    }
}