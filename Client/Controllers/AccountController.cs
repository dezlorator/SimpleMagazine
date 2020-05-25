using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PetStore.Models.ViewModels;
using PetStore.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System;
using Client.Models;

namespace PetStore.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly string _apiPathRegister = "http://localhost:62029/api/account/register";
        private readonly string _apiPathLogin = "http://localhost:62029/api/account/login";

        public AccountController()
        {

        }

        [AllowAnonymous]
        public ViewResult Register(string returnUrl)
        {
            return View(new RegisterModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = null;

                    using (var httpClient = new HttpClient())
                    {
                        MultipartFormDataContent data = new MultipartFormDataContent();

                        data.Add(new StringContent(model.UserName), "UserName");
                        data.Add(new StringContent(model.Email), "Email");
                        data.Add(new StringContent(model.Birthday.ToString()), "Birthday");
                        data.Add(new StringContent(model.Password), "Password");
                        data.Add(new StringContent(model.PasswordConfirm), "PasswordConfirm");

                        response = await httpClient.PostAsync(_apiPathRegister, data);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            return RedirectToAction("Login", "Account");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            ModelState.AddModelError("", "Ошибка при регистрации");

            return View(model);
        }

        [AllowAnonymous]
        public ViewResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    HttpResponseMessage response = null;

                    using (var httpClient = new HttpClient())
                    {
                        MultipartFormDataContent data = new MultipartFormDataContent();

                        data.Add(new StringContent(model.Name), "Name");
                        data.Add(new StringContent(model.Password), "Password");

                        response = await httpClient.PostAsync(_apiPathLogin, data);

                        if (response.StatusCode == System.Net.HttpStatusCode.OK)
                        {
                            var json = await response.Content.ReadAsStringAsync();
                            var obj = JsonConvert.DeserializeObject<LoginResponse>(json);
                            TokenKeeper.Token = obj.access_token;

                            return RedirectToAction("List", "Product");
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }

            ModelState.AddModelError("", "Некорректное имя пользователя или пароль");

            return View(model);
        }

        [AllowAnonymous]
        public async Task<RedirectToActionResult> Logout()
        {
            TokenKeeper.Token = String.Empty;

            return RedirectToAction("Login", "Account");
        }
    }
}