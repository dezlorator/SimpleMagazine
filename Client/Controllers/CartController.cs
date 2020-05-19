using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetStore.Infrastructure;
using PetStore.Models;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        #region fields

        private IProductRepository _repository;
        private IStockRepository _stockRepository;
        private Cart _cart;

        #endregion

        public CartController(IProductRepository repo, IStockRepository stockRepository, Cart cartService)
        {
            _repository = repo;
            _stockRepository = stockRepository;
            _cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = _cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ID == productId);
            if (product != null)
            {
                _cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult IncreaseQuantity(int editedLineProductId, string returnUrl)
        {
            var line = _cart.Lines.FirstOrDefault(l => l.Product.ID == editedLineProductId);

            Product product = _repository.Products
                .FirstOrDefault(p => p.ID == line.Product.ID);

            if (product != null && _stockRepository.StockItems
                        .FirstOrDefault(s => s.Product.ID == product.ID).Quantity > line.Quantity)
            {
                _cart.AddItem(product, 1);
            }
            else
            {
                TempData["message"] = $"Недостаточное количество единиц на складе";
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult ReduceQuantity(int editedLineProductId, string returnUrl)
        {
            var line = _cart.Lines.FirstOrDefault(l => l.Product.ID == editedLineProductId);

            Product product = _repository.Products
                .FirstOrDefault(p => p.ID == line.Product.ID);

            if (product != null)
            {
                if (line.Quantity == 1)
                {
                    _cart.RemoveLine(product);
                }
                else
                {
                    _cart.ReduceQuantity(product);
                }
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId,
                string returnUrl)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ID == productId);

            if (product != null)
            {
                _cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}