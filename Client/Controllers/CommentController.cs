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
    public class CommentController : Controller
    {
        private readonly string _apiPathCreate = "http://localhost:62029/api/comment/Create";
        private readonly string _apiPathGetModel = "http://localhost:62029/api/comment/GetModel";
        private readonly string _apiPath = "http://localhost:62029/api/comment";
        private readonly string _apiPathDelete = "http://localhost:62029/api/comment/Delete";

        public CommentController()
        {

        }

        public ViewResult Create(int productId, string returnUrl) => View(new CommentViewModel
        {
            ProductId = productId,
            ReturnUrl = returnUrl
        });

        [HttpPost]
        public async Task<IActionResult> Create(CommentViewModel commentModel)
        {
            if (TokenKeeper.Token == String.Empty)
            {
                return RedirectToAction("Login", "Account", new { commentModel.ReturnUrl });
            }

            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(commentModel.ID.ToString()), "ID");
                    data.Add(new StringContent(commentModel.Message), "Message");
                    data.Add(new StringContent(commentModel.ProductId.ToString()), "ProductId");
                    data.Add(new StringContent(commentModel.Rating.ToString()), "Rating");
                    data.Add(new StringContent(commentModel.Time.ToString()), "Time");
                    data.Add(new StringContent(TokenKeeper.UserName), "UserName");
                    data.Add(new StringContent(commentModel.ReturnUrl), "ReturnUrl");

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

            return Redirect(commentModel.ReturnUrl);
        }

        public async Task<ViewResult> Edit(int commentId, string returnUrl)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(commentId.ToString()), "commentId");
                    data.Add(new StringContent(returnUrl), "returnUrl");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathGetModel, data);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<CommentViewModel>(json);

                    return View(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentViewModel commentModel, int id)
        {
            if (TokenKeeper.Token == String.Empty)
            {
                return RedirectToAction("Login", "Account", new { commentModel.ReturnUrl });
            }

            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(id.ToString()), "id");
                    data.Add(new StringContent(commentModel.Message), "Message");
                    data.Add(new StringContent(commentModel.ProductId.ToString()), "ProductId");
                    data.Add(new StringContent(commentModel.Rating.ToString()), "Rating");
                    data.Add(new StringContent(commentModel.Time.ToString()), "Time");
                    data.Add(new StringContent(commentModel.UserName), "UserName");
                    data.Add(new StringContent(commentModel.ReturnUrl), "ReturnUrl");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PutAsync(_apiPath, data);

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

            return Redirect(commentModel.ReturnUrl);
        }

        public async Task<IActionResult> Delete(int commentId, string returnUrl)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(commentId.ToString()), "commentId");
                    data.Add(new StringContent(returnUrl), "returnUrl");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathDelete, data);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return Redirect(returnUrl);
        }
    }
}