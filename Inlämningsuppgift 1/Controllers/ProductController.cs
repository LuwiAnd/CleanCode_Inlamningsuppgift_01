using Inlämningsuppgift_1.Services.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Inlämningsuppgift_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service = new ProductService();

        public class CreateProductRequest { 
            public string Name { get; set; } = ""; 
            public decimal Price { get; set; } 
            public int Stock { get; set; } 
        }

        [HttpGet]
        public IActionResult GetAll() => Ok(_service.GetAll());

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var p = _service.GetById(id);
            if (p == null) return NotFound();
            return Ok(p);
        }

        [HttpGet("search")]
        public IActionResult Search([FromQuery] string q, [FromQuery] decimal? maxPrice)
        {            
            var results = _service.Search(q);
            if (maxPrice.HasValue) results = results.Where(x => x.Price <= maxPrice.Value).ToList();
            return Ok(results);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateProductRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Name)) return BadRequest("Name required.");
            var created = _service.Create(req.Name, req.Price, req.Stock);
            return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
        }

        [HttpPost("{id}/stock/increase")]
        public IActionResult IncreaseStock(int id, [FromQuery] int amount)
        {           
            if (amount <= 0) return BadRequest("Amount must be > 0");
            var ok = _service.ChangeStock(id, amount);
            if (!ok) return NotFound();
            return Ok();
        }
    }
}
