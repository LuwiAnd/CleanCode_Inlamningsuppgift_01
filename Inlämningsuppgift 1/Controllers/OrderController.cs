using Inlämningsuppgift_1.Entities;
using Inlämningsuppgift_1.Services.Implementations;
using Inlämningsuppgift_1.Services.Interfaces;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inlämningsuppgift_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IProductService _productService;
        private readonly IUserService _userService;

        public OrderController(
            IOrderService orderService,
            ICartService cartService,
            IProductService productService,
            IUserService userService)
        {
            _orderService = orderService;
            _cartService = cartService;
            _productService = productService;
            _userService = userService;
        }


        [HttpPost("create")]
        public IActionResult CreateOrder([FromHeader(Name = "X-Auth-Token")] string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            var cart = _cartService.GetCartForUser(user.Id);
            if (cart == null || cart?.CartItems?.Count == 0)
                return BadRequest("Cart is empty.");

            var orderItems = cart.CartItems.Select(ci =>
            {
                var p = _productService.GetProductById(ci.ProductId);
                if (p == null)
                    throw new Exception($"Product not found: {ci.ProductId}");

                return new OrderItem
                {
                    ProductId = p.Id,
                    ProductName = p.Name,
                    Quantity = ci.Quantity,
                    UnitPrice = p.Price
                };

            }).ToList();

            var order = _orderService.CreateOrder(user.Id, orderItems);

            _cartService.ClearCart(user.Id);

            return Ok(new { OrderId = order.Id, Total = order.Total });
        }

        [HttpGet("{orderId}")]
        public IActionResult GetOrder(int orderId, [FromHeader(Name = "X-Auth-Token")] string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            var order = _orderService.GetOrderById(orderId);
            if (order == null) return NotFound();
            if (order.UserId != user.Id) return Forbid();

            return Ok(order);
        }
    }
}
