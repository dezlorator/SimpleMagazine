using Microsoft.AspNetCore.Mvc;
using PetStore.Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Client.Models;
using Newtonsoft.Json;
using Client.Models.ViewModels;

namespace PetStore.Controllers
{
    public class UserController : Controller
    {
        private readonly string _apiPathGetAll = "http://localhost:62029/api/user/GetAll";
        private readonly string _apiPathGetModel = "http://localhost:62029/api/user/GetModel";
        private readonly string _apiPathChangePermission = "http://localhost:62029/api/user/ChangePermission";
        private readonly string _apiPathDelete = "http://localhost:62029/api/user/Delete";

        public UserController()
        {

        }

        public async Task<ViewResult> Index()//(FilterParametersProducts filter, int productPage = 1)
        {
            ViewBag.Current = "Users";

            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);

                    response = await httpClient.GetAsync(_apiPathGetAll);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ApplicationUser>(json);

                    return View(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IActionResult> Edit(int userId)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();

                    data.Add(new StringContent(userId.ToString()), "userId");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathGetModel, data);

                    var json = await response.Content.ReadAsStringAsync();
                    var obj = JsonConvert.DeserializeObject<ChangeUserPermissionViewModel>(json);

                    return View(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ChangeUserPermissionViewModel permissions, int userId)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();

                    data.Add(new StringContent(userId.ToString()), "UserId");
                    data.Add(new StringContent(permissions.CanAddComments.ToString()), "CanAddComments");
                    data.Add(new StringContent(permissions.CanModerateComments.ToString()), "CanModerateComments");
                    data.Add(new StringContent(permissions.CanEditProducts.ToString()), "CanEditProducts");
                    data.Add(new StringContent(permissions.CanPurchaseToStock.ToString()), "CanPurchaseToStock");
                    data.Add(new StringContent(permissions.CanDeleteProducts.ToString()), "CanDeleteProducts");
                    data.Add(new StringContent(permissions.CanAddProducts.ToString()), "CanAddProducts");
                    data.Add(new StringContent(permissions.CanViewStatistics.ToString()), "CanViewStatistics");
                    data.Add(new StringContent(permissions.CanViewUsersList.ToString()), "CanViewUsersList");
                    data.Add(new StringContent(permissions.CanSetRoles.ToString()), "CanSetRoles");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PutAsync(_apiPathChangePermission, data);

                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        TempData["message"] = $"Ошибка обновления";

                        return View(permissions);
                    }

                    TempData["message"] = $"Разрешения пользователя обновлены";

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int userId)
        {
            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    MultipartFormDataContent data = new MultipartFormDataContent();
                    data.Add(new StringContent(userId.ToString()), "userId");

                    httpClient.DefaultRequestHeaders.Authorization =
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.PostAsync(_apiPathDelete, data);

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        TempData["message"] = $"Пользователь был удален";
                    }

                    return RedirectToAction("Index");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}