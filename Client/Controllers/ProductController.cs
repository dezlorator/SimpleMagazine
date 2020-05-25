using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.Ocsp;
using PetStore.Filters;
using PetStore.Filters.FilterParameters;
using PetStore.Models;
using PetStore.Models.MongoDb;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    public class ProductController : Controller
    {
        #region fields

        public int PageSize = 4;
        private readonly string _apiPathList = "http://localhost:62029/api/product/GetAll";
        private readonly string _apiPathSearchList = "http://localhost:62029/api/product/SearchList";
        private readonly string _apiPathInfo = "http://localhost:62029/api/product/GetInfo";
        private readonly ImagesDbContext _imagesDb;

        #endregion

        public ProductController(ImagesDbContext context)
        {
            _imagesDb = context;
        }

        public async Task<ViewResult> List(FilterParametersProducts filter, int productPage = 1)
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

               //     httpClient.DefaultRequestHeaders.Add("Authorization", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathList, data);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ProductsListViewModel>(json);

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

               //     httpClient.DefaultRequestHeaders.Add("Authorization", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathSearchList, data);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ProductsListViewModel>(json);

                    return View(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ViewResult> Info(int productId)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(productId.ToString()), "productId");

                //    httpClient.DefaultRequestHeaders.Add("Authorization", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathInfo, data);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ProductExtended>(json);

                    return View(obj);
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
    }
}
