using Inlämningsuppgift_1.Services.Implementations;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inlämningsuppgift_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly CartService _service = new CartService();
        private readonly UserService _userService = new UserService(); 

        public class AddItemRequest { public int ProductId { get; set; } public int Quantity { get; set; } }

        [HttpPost("add")]
        public IActionResult AddItem([FromHeader(Name = "X-Auth-Token")] string token, [FromBody] AddItemRequest req)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            if (req.Quantity <= 0) return BadRequest("Quantity must be > 0");
                        
            _service.AddToCart(user.Id, req.ProductId, req.Quantity);
            return Ok();
        }

        [HttpGet("me")]
        public IActionResult GetCart([FromHeader(Name = "X-Auth-Token")] string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            var cart = _service.GetCartForUser(user.Id);
            
            var productService = new ProductService();
            var detailed = cart.Select(ci =>
            {
                var p = productService.GetById(ci.ProductId);
                return new { ci.ProductId, ProductName = p?.Name, ci.Quantity, UnitPrice = p?.Price };
            });
            return Ok(detailed);
        }

        [HttpPost("remove")]
        public IActionResult RemoveItem([FromHeader(Name = "X-Auth-Token")] string token, [FromQuery] int productId)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            _service.RemoveFromCart(user.Id, productId);
            return Ok();
        }

        [HttpPost("clear")]
        public IActionResult ClearCart([FromHeader(Name = "X-Auth-Token")] string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            _service.ClearCart(user.Id);
            return Ok();
        }
    }
}
