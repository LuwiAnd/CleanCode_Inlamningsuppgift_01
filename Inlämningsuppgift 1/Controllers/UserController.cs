using Inlämningsuppgift_1.Services.Implementations;
using Microsoft.AspNetCore.Mvc;
using static Inlämningsuppgift_1.Services.Implementations.UserService;

namespace Inlämningsuppgift_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly UserService _service = new UserService();
        
        public class LoginRequest { public string Username { get; set; } = ""; public string Password { get; set; } = ""; }
        public class RegisterRequest { public string Username { get; set; } = ""; public string Password { get; set; } = ""; public string Email { get; set; } = ""; }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Username and password required.");

            var created = _service.Register(req.Username, req.Password, req.Email);
            if (!created) return Conflict("User already exists.");

            return Ok(new { Message = "Registered" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) || string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Missing credentials.");

            var token = _service.Login(req.Username, req.Password);
            if (token == null) return Unauthorized("Invalid credentials.");
                       
            return Ok(new { Token = token });
        }

        [HttpGet("profile")]
        public IActionResult Profile([FromHeader(Name = "X-Auth-Token")] string token)
        {
            
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized();

            var profile = _service.GetUserByToken(token);
            if (profile == null) return Unauthorized();

            return Ok(new { profile.Username, profile.Email, profile.Id });
        }
    }
}
