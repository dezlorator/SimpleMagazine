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

namespace PetStore.Controllers
{
    public class StatisticsController : Controller
    {
        private IStockRepository _stockRepository;
        private IProductRepository _productRepository;
        private IOrderRepository _orderRepository;

        public StatisticsController(IProductRepository productRepository, 
                                    IStockRepository stockRepository,
                                    IOrderRepository orderRepository)
        {
            _productRepository = productRepository;
            _stockRepository = stockRepository;
            _orderRepository = orderRepository;
        }

        [Authorize(Roles = "Admin, Manager")]
        public IActionResult Index()
        {
            ViewBag.Current = "Statistics";

            var listModel = new List<CategoriesChartViewModel>();
            var products = _productRepository.Products;
            var orders = _orderRepository.Orders;
            var categroies = products.Select(p => p.Category).Distinct();

            foreach (var c in categroies)
            {
                var model = new CategoriesChartViewModel();

                var categoryItems = products.Where(p => p.Category.ID == c.ID);
                foreach (var p in categoryItems)
                {
                    model.Charts.Add(new SimpleChartViewModel
                    {
                        DimensionOne = p.Name,
                        Quantity = orders
                            .Where(o => o.Lines
                                .FirstOrDefault(i => i.Product == p) != null).Count()
                    });
                }

                model.Category = c.Name;

                listModel.Add(model);
            }

            return View(listModel.OrderBy(i => i.Category).ToList());
        }
    }
}