using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MovieAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ValuesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult CheckConnection()
        {
            try
            {
                var usersCount = _context.Users.Count();
                return Ok($"Database connected! User count: {usersCount}");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Connection failed: {ex.Message}");
            }
        }
    }
}
