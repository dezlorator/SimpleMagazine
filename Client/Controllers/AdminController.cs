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

        private readonly string _apiPath = "http://localhost:62029/api/admin";
        private readonly string _apiPathGetData = "http://localhost:62029/api/admin/GetData";
        private readonly string _apiPathSearchList = "http://localhost:62029/api/admin/SearchList";
        private readonly string _apiPathGetCategories = "http://localhost:62029/api/admin/GetCategories";
        private readonly string _apiPathCreate = "http://localhost:62029/api/admin/Create";
        private readonly string _apiPathEditModel = "http://localhost:62029/api/admin/EditModel";
        private readonly string _apiPathDelete = "http://localhost:62029/api/admin/Delete";
        private readonly string _apiPathAddToStock = "http://localhost:62029/api/admin/AddToStock";
        private readonly string _apiPathAttachImage = "http://localhost:62029/api/admin/AttachImage";


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
                        obj.CurrentFilter.CategoriesList = new List<int>();
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
                        Categories = obj
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

        public async Task<IActionResult> Edit(int productId)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();

                    data.Add(new StringContent(productId.ToString()), "productId");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathEditModel, data);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ProductWithCategoryViewModel>(json);

                    return View(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ProductWithCategoryViewModel productExtended, int id)
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

                    data.Add(new StringContent(id.ToString()), "ID");
                    data.Add(new StringContent(productExtended.Product.ID.ToString()), "Product.ID");
                    data.Add(new StringContent(productExtended.Product.ProductIdentifier.ToString()), "Product.ProductIdentifier");
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
                    response = await httpClient.PutAsync(_apiPath, data);

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

        [HttpPost]
        public async Task<IActionResult> Delete(int productId)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(productId.ToString()), "productId");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathDelete, data);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["message"] = $"Товар был удален";
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddToStock(int productId, int quantity)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(productId.ToString()), "productId");
                    data.Add(new StringContent(quantity.ToString()), "quantity");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathAddToStock, data);

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ActionResult> GetImage(string id)
        {
            var image = await _imagesDb.GetImage(id);
            if (image == null)
            {
                return NotFound();
            }
            return File(image, "image/png");
        }

        [HttpPost]
        public async Task<ActionResult> AttachImage(int id, IFormFile uploadedFile)
        {
            if (uploadedFile != null)
            {
                var image = await _imagesDb.StoreImage(uploadedFile.OpenReadStream(), uploadedFile.FileName);

                try
                {
                    HttpResponseMessage response = null;

                    using (var httpClient = new HttpClient())
                    {
                        MultipartFormDataContent data = new MultipartFormDataContent();
                        data.Add(new StringContent(id.ToString()), "id");
                        data.Add(new StringContent(image), "image");

                        httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                        response = await httpClient.PostAsync(_apiPathAttachImage, data);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            TempData["message"] = $"Товар был сохранен";
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return RedirectToAction("Index");
        }
    }
}