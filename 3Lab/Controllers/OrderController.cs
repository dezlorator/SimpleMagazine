using Microsoft.AspNetCore.Mvc;
using PetStore.Models;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using LearningEngine.Api.Extensions;

namespace PetStore.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        #region fields

        private IOrderRepository _repository;
        private IStockRepository _stockRepository;
        private Cart _cart;

        #endregion

        public OrderController(IOrderRepository repoService, IStockRepository stockRepository, Cart cartService)
        {
            _repository = repoService;
            _stockRepository = stockRepository;
            _cart = cartService;
        }

        public async Task<ActionResult> MyList()
        {
            return Ok(_repository.Orders.Where(o => o.UserName == this.GetUserName()).OrderByDescending(o => o.Date));
        }

        public async Task<ActionResult> List()
        {
            return Ok(_repository.Orders.Where(o => !o.Shipped && !o.Canceled).OrderByDescending(o => o.Date));
        }

        public async Task<ActionResult> ListShipped()
        {
            return Ok(_repository.Orders.Where(o => o.Shipped).OrderByDescending(o => o.Date));
        }

        [HttpPost]
        public async Task<ActionResult> MarkShipped([FromForm]int orderID)
        {
            Order order = _repository.Orders
                .FirstOrDefault(o => o.OrderID == orderID);

            if (order != null)
            {
                bool flag = true;

                foreach (var l in order.Lines)
                {
                    if (l.Quantity > _stockRepository.StockItems.FirstOrDefault(s => s.Product.ID == l.Product.ID).Quantity)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    order.Shipped = true;
                    _repository.SaveOrder(order);

                    foreach (var l in order.Lines)
                    {
                        _stockRepository.ReduceQuantity(l.Product.ID, l.Quantity);
                    }
                }
            }

            return BadRequest(nameof(List));
        }

        [HttpPost("cancel")]
        public async Task<ActionResult> Cancel([FromForm]int orderID)
        {
            Order order = _repository.Orders
                .FirstOrDefault(o => o.OrderID == orderID);

            if(order!=null)
            {
                order.Canceled = true;
                _repository.SaveOrder(order);

                return Ok();
            }

            return BadRequest(nameof(MyList));
        }

        public async Task<ActionResult> Checkout() => Ok(new Order());

        [HttpPost("checkout")]
        public async Task<ActionResult> Checkout([FromForm]Order order)
        {
            if (_cart.Lines.Count() == 0)
            {
                return BadRequest("Empty");
            }

            if (ModelState.IsValid)
            {
                order.Lines = _cart.Lines.ToArray();
                order.UserName = this.GetUserName();

                _repository.SaveOrder(order);

                return Ok(nameof(Completed));
            }
            else
            {
                return BadRequest(order);
            }
        }

        public async Task<ActionResult> Completed()
        {
            _cart.Clear();

            return Ok();
        }
    }
}