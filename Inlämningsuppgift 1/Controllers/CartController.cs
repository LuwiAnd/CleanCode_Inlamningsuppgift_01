using Inlämningsuppgift_1.Services.Implementations;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Inlämningsuppgift_1.Dto.Requests;
using Inlämningsuppgift_1.Services.Interfaces;
using System.Runtime.InteropServices;
using Inlämningsuppgift_1.Dto.Responses;

namespace Inlämningsuppgift_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        //private readonly CartService _service = new CartService();
        //private readonly UserService _userService = new UserService();

        private readonly ICartService _cartService;
        private readonly IUserService _userService;
        private readonly IProductService _productService;

        public CartController(
            ICartService cartService, 
            IUserService userService,
            IProductService productService
        )
        {
            this._cartService = cartService;
            this._userService = userService;
            this._productService = productService;
        }

        [HttpPost("add")]
        public IActionResult AddItem(
            [FromHeader(Name = "X-Auth-Token")] string token, 
            [FromBody] AddItemRequest req
        )
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            if (req.Quantity <= 0) return BadRequest("Quantity must be > 0");
                        
            _cartService.AddToCart(user.Id, req.ProductId, req.Quantity);
            return Ok();
        }

        [HttpGet("me")]
        public IActionResult GetCart([FromHeader(Name = "X-Auth-Token")] string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            var cart = _cartService.GetCartForUser(user.Id);
            
            // Luwi: denna service sparas i konstruktorn för närvarande. Jag
            // kommer troligtvis uppdatera så att Program.cs instansierar alla
            // service:ar senare.
            //var productService = new ProductService(); 


            var detailedCart = cart.Select(ci =>
            {
                var p = _productService.GetById(ci.ProductId);

                //return new { ci.ProductId, ProductName = p?.Name, ci.Quantity, UnitPrice = p?.Price };
                return new CartItemResponse{ 
                    ProductId = ci.ProductId, 
                    ProductName = p?.Name, 
                    Quantity = ci.Quantity, 
                    UnitPrice = p?.Price 
                };
            });
            return Ok(detailedCart);
        }

        [HttpPost("remove")]
        public IActionResult RemoveItem([FromHeader(Name = "X-Auth-Token")] string token, [FromQuery] int productId)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            _cartService.RemoveFromCart(user.Id, productId);
            return Ok();
        }

        [HttpPost("clear")]
        public IActionResult ClearCart([FromHeader(Name = "X-Auth-Token")] string token)
        {
            var user = _userService.GetUserByToken(token);
            if (user == null) return Unauthorized();

            _cartService.ClearCart(user.Id);
            return Ok();
        }
    }
}
