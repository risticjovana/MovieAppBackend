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

        [HttpGet("projectionsByContent/{contentId}")]
        public async Task<ActionResult<List<CinemaWithProjectionsDTO>>> GetProjectionsByContentId(int contentId)
        {
            var groupedProjections = await _movieService.GetGroupedProjectionsByContentIdAsync(contentId);

            if (groupedProjections == null || groupedProjections.Count == 0)
            {
                return NotFound($"No projections found for content ID {contentId}.");
            }

            return Ok(groupedProjections);
        }

        [HttpPost("reserve")]
        public async Task<ActionResult> ReserveTicket([FromBody] Ticket ticket)
        {
            if (ticket == null)
            {
                return BadRequest("Ticket data is required.");
            }

            if (ticket.PurchaseTime == default)
            {
                ticket.PurchaseTime = DateTime.UtcNow;
            }

            var success = await _movieService.ReserveTicketAsync(ticket);

            if (success)
            {
                return Ok("Ticket reserved successfully.");
            }
            else
            {
                return StatusCode(500, "An error occurred while reserving the ticket.");
            }
        }
    }
}
