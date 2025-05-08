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
        public async Task<IActionResult> Register([FromBody] RegisterModel dto)
        {
            var result = await _authService.RegisterAsync(dto.FirstName, dto.LastName, dto.Email, dto.Password);
            if (result.Contains("already"))
                return BadRequest(result);

            return Ok(result); // Angular expects plain text
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel dto)
        {
            var token = await _authService.LoginAsync(dto.Email, dto.Password);

            if (token == null || token.StartsWith("Invalid") || token.StartsWith("User not"))
                return Unauthorized("Invalid email or password");

            return Ok(new { token });
        }
    }
}
