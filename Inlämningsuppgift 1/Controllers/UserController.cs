using Inlämningsuppgift_1.Dto.Requests;
using Inlämningsuppgift_1.Services.Implementations;
using Inlämningsuppgift_1.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using static Inlämningsuppgift_1.Services.Implementations.UserService;

namespace Inlämningsuppgift_1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserRegisterRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Username) ||
                string.IsNullOrWhiteSpace(req.Password))
                return BadRequest("Username and password required.");

            var ok = _service.Register(req);
            if (!ok) return Conflict("User already exists.");

            return Ok(new { Message = "Registered" });
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginRequest req)
        {
            var response = _service.Login(req);
            if (response == null) return Unauthorized("Invalid credentials.");

            return Ok(response);
        }

        [HttpGet("profile")]
        public IActionResult Profile([FromHeader(Name = "X-Auth-Token")] string token)
        {
            if (string.IsNullOrWhiteSpace(token)) return Unauthorized();

            var user = _service.GetUserByToken(token);
            if (user == null) return Unauthorized();

            return Ok(_service.ToUserResponse(user));
        }
    }
}
