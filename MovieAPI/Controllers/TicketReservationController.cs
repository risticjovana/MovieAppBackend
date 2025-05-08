using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models.TicketReservation;
using MovieAPI.Services;

namespace MovieAPI.Controllers
{
    [Route("api/ticketReservation")]
    [ApiController]
    public class TicketReservationController : ControllerBase
    {
        private readonly MovieService _movieService;

        public TicketReservationController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet("availableMovies")]
        public async Task<ActionResult<List<MovieDTO>>> GetAllAvailableMovies()
        {
            var movies = await _movieService.GetAllAvailableMoviesAsync();

            if (movies == null || movies.Count == 0)
            {
                return NotFound("No available movies found.");
            }

            return Ok(movies);
        }
    }
}
