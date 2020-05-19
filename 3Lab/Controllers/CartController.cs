using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetStore.Infrastructure;
using PetStore.Models;
using PetStore.Models.ViewModels;

namespace PetStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CartController : ControllerBase
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

        [HttpPost()]
        public async Task<ActionResult> AddToCart([FromForm]int productId)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ID == productId);
            if (product != null)
            {
                _cart.AddItem(product, 1);
            }

            return Ok();
        }

        [HttpPost("IncreaseQuantity")]
        public async Task<ActionResult> IncreaseQuantity([FromForm]int editedLineProductId)
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
                BadRequest("Недостаточное количество единиц на складе");
            }

            return Ok();
        }

        [HttpPost("ReduceQuantity")]
        public async Task<ActionResult> ReduceQuantity([FromForm]int editedLineProductId)
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

            return Ok();
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFromCart([FromForm]int productId,
                [FromForm]string returnUrl)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ID == productId);

            if (product != null)
            {
                _cart.RemoveLine(product);
            }

            return Ok();
        }
    }
}