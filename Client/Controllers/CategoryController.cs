using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Client.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PetStore.Models;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    public class CategoryController : Controller
    {
        #region private

        private readonly string _apiPathList = "http://localhost:62029/api/category/GetAll";
        private readonly string _apiPathCreate = "http://localhost:62029/api/category/Create";
        private readonly string _apiPathGetEdit = "http://localhost:62029/api/category/GetEdit";
        private readonly string _apiPathEdit = "http://localhost:62029/api/category/Edit";
        private readonly string _apiPathDelete = "http://localhost:62029/api/category/Delete";

        #endregion

        public CategoryController()
        {

        }

        public async Task<ViewResult> List()
        {
            ViewBag.Current = "Categories";

            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.GetAsync(_apiPathList);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<List<CategoryNode>>(json);

                    return View(obj.AsQueryable());
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ViewResult Create(int parentId)
        {
            ViewBag.Current = "Categories";

            return View(new CategoryViewModel
            {
                ParentID = parentId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryViewModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = null;

                    using (var httpClient = new HttpClient())
                    {
                        MultipartFormDataContent data = new MultipartFormDataContent();
                        data.Add(new StringContent(categoryModel.ID.ToString()), "ID");
                        data.Add(new StringContent(categoryModel.Name.ToString()), "Name");
                        data.Add(new StringContent(categoryModel.IsRoot.ToString()), "IsRoot");
                        data.Add(new StringContent(categoryModel.ParentID.ToString()), "ParentID");

                        httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                        response = await httpClient.PostAsync(_apiPathCreate, data);

                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            TempData["message"] = $"Ошибка";
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                return RedirectToAction("List");
            }
            else
            {
                TempData["message"] = $"Ошибка";
                return RedirectToAction("List");
            }
        }

        public async Task<ViewResult> Edit(int categoryId)
        {
            ViewBag.Current = "Categories";

            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(categoryId.ToString()), "categoryId");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathGetEdit, data);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<CategoryNode>(json);

                    return View(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoryNode category, int id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = null;

                    using (var httpClient = new HttpClient())
                    {
                        MultipartFormDataContent data = new MultipartFormDataContent();
                        data.Add(new StringContent(category.ID.ToString()), "ID");
                        data.Add(new StringContent(category.Name.ToString()), "Name");
                        data.Add(new StringContent(category.IsRoot.ToString()), "IsRoot");

                        httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                        response = await httpClient.PutAsync(_apiPathEdit, data);

                        return RedirectToAction("List");
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                TempData["message"] = $"Ошибка";

                return RedirectToAction("List");
            }
        }

        public async Task<IActionResult> Delete(int categoryId)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(categoryId.ToString()), "categoryId");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathDelete, data);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return RedirectToAction("List");
        }
    }
}