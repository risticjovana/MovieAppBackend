using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models.User;
using MovieAPI.Services;

namespace MovieAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        // Register route
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.RegisterAsync(model.FirstName, model.LastName, model.Email, model.Password);

            if (result.Contains("successfully"))
                return Ok(result);
            else
                return BadRequest(result);
        }

        // Login route
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var result = await _authService.LoginAsync(model.Email, model.Password);

            if (result.Contains("successfully"))
                return Ok(result);
            else
                return Unauthorized(result);
        }
    }
}
