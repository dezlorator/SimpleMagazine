using Microsoft.AspNetCore.Mvc;
using PetStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using PetStore.Models.MongoDb;
using Microsoft.AspNetCore.Http;
using System;
using PetStore.Models.ViewModels;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Client.Models;

namespace PetStore.Controllers
{
    public class StatisticsController : Controller
    {
        private readonly string _apiPath = "http://localhost:62029/api/statistics";

        public StatisticsController()
        {

        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Current = "Statistics";

            try
            {
                HttpResponseMessage response = null;

                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization =
                            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", TokenKeeper.Token);
                    response = await httpClient.GetAsync(_apiPath);

                    var json = await response.Content.ReadAsStringAsync();

                    var obj = JsonConvert.DeserializeObject<List<CategoriesChartViewModel>>(json);

                    return View(obj);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}