using Inlämningsuppgift_1.Services.Implementations;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inlämningsuppgift_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService = new OrderService();
        private readonly CartService _cartService = new CartService();
        private readonly ProductService _productService = new ProductService();
        private readonly UserService _userService = new UserService();

        [HttpPost("create")]
        public IActionResult CreateOrder([FromHeader(Name = "X-Auth-Token")] string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            var cart = _cartService.GetCartForUser(user.Id).ToList();
            if (!cart.Any()) return BadRequest("Cart is empty");

            var orderItems = new List<object>();
            decimal total = 0m;

            foreach (var ci in cart)
            {
                var product = _productService.GetById(ci.ProductId);
                if (product == null) return BadRequest($"Product {ci.ProductId} missing.");
                if (product.Stock < ci.Quantity) return BadRequest($"Not enough stock for {product.Name}.");

                product.Stock -= ci.Quantity;
                _productService.UpdateProduct(product);

                orderItems.Add(new { product.Id, product.Name, ci.Quantity, product.Price });
                total += product.Price * ci.Quantity;
            }

            var order = _orderService.CreateOrder(user.Id, orderItems, total);

            _cartService.ClearCart(user.Id);

            return Ok(new { OrderId = order.Id, Total = total });
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrder(int orderId, [FromHeader(Name = "X-Auth-Token")] string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            var order = _orderService.Get(orderId);
            if (order == null) return NotFound();
            if (order.UserId != user.Id) return Forbid();

            return Ok(order);
        }
    }
}
