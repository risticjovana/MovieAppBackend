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

        [HttpGet("user/{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound("User not found.");

            // You may want to limit which data is returned
            return Ok(new
            {
                user.Id,
                user.FirstName,
                user.LastName,
                user.Email,
                user.Status,
                user.Role,
                user.Rank
            });
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordModel model)
        {
            var result = await _authService.ChangePasswordAsync(model.UserId, model.CurrentPassword, model.NewPassword);

            if (result.Contains("incorrect") || result.Contains("not found"))
                return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("request-role")]
        public async Task<IActionResult> RequestRoleChange([FromBody] RoleChangeRequestModel model)
        {
            // Convert English role to Serbian before sending to service
            var requestedRoleSerbian = RoleTranslationHelper.ToSerbian(model.RequestedRole);

            var result = await _authService.RequestRoleChangeAsync(model.UserId, model.RequestedRole);

            if (result.Contains("not found") || result.Contains("already"))
                return BadRequest(result);

            return Ok(new
            {
                message = result,
                requestedRoleSerbian = requestedRoleSerbian,
                requestedRoleEnglish = model.RequestedRole
            });
        }

        [HttpGet("role-requests")]
        public async Task<IActionResult> GetAllRequests()
        {
            var requests = await _authService.GetAllRoleRequestsAsync();

            var result = requests.Select(r => new {
                r.Id,
                r.Status,
                r.RequestedRole,
                r.CreatedAt,
                User = new
                {
                    r.User.Id,
                    r.User.FirstName,
                    r.User.LastName,
                    r.User.Email,
                    r.User.Role
                },
                RequestedRoleEnglish = RoleTranslationHelper.TranslateToEnglish(r.RequestedRole)
            });

            return Ok(result);
        }

        [HttpPost("verify-role/{id}")]
        public async Task<IActionResult> VerifyRequest(int id)
        {
            var result = await _authService.VerifyRoleRequestAsync(id);

            if (result.Contains("not found"))
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost("decline-role/{id}")]
        public async Task<IActionResult> DeclineRequest(int id)
        {
            var result = await _authService.DeclineRoleRequestAsync(id);

            if (result.Contains("not found"))
                return NotFound(result);

            return Ok(result);
        }

    }
}
