using Inlämningsuppgift_1.Dto.Requests;
using Inlämningsuppgift_1.Services.Implementations;
using Inlämningsuppgift_1.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inlämningsuppgift_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;

        public ProductController(IProductService service)
        {
            _service = service;
        }



        [HttpGet]
        public IActionResult GetAll()
        {
            var products = _service.GetAllProducts();
            return Ok(products.Select(p => _service.ToProductResponse(p)));
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var p = _service.GetProductById(id);
            if (p == null) return NotFound();

            return Ok(_service.ToProductResponse(p));
        }

        [HttpGet("search")]
        public IActionResult Search(
            [FromQuery] string q, 
            [FromQuery] decimal? maxPrice
        )
        {
            var results = _service.Search(q);
            
            if (maxPrice.HasValue)
                results = results
                    .Where(x => x.Price <= maxPrice.Value)
                    .ToList();

            return Ok(results.Select(p => _service.ToProductResponse(p)));
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name)) 
                return BadRequest("Name is required.");

            var created = _service.CreateProduct(req.Name, req.Price, req.StockBalance);

            var response = _service.ToProductResponse(created);
            return CreatedAtAction(
                nameof(Get),
                new { id = created.Id },
                response
            );
        }

        [HttpPost("{id}/stock/increase")]
        public IActionResult IncreaseStock(
            int id, 
            [FromQuery] int amount
        )
        {           
            if (amount <= 0) return BadRequest("Amount must be > 0");
            
            var ok = _service.ChangeProductStock(id, amount);
            if (!ok) return NotFound();

            return Ok();
        }
    }
}
