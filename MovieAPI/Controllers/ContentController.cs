using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieAPI.Models.Content;
using MovieAPI.Services;

namespace MovieAPI.Controllers
{
    [Route("api/content")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly ContentService _contentService;

        public ContentController(ContentService contentService)
        {
            _contentService = contentService;
        }

        [HttpGet("by-genre/{genreName}")]
        public async Task<IActionResult> GetContentByGenre(string genreName)
        {
            var contents = await _contentService.GetMoviesByGenreAsync(genreName);

            if (contents == null || contents.Count == 0)
            {
                return NotFound($"No content found for genre: {genreName}");
            }

            return Ok(contents);
        }

        [HttpGet("series-by-genre/{genreName}")]
        public async Task<IActionResult> GetSeriesContentByGenre(string genreName)
        {
            var contents = await _contentService.GetTVSeriesByGenreAsync(genreName);

            if (contents == null || contents.Count == 0)
            {
                return NotFound($"No content found for genre: {genreName}");
            }

            return Ok(contents);
        }

        [HttpGet("{contentId}/content-reviews")]
        public async Task<IActionResult> GetContentReviews(int contentId)
        {
            var reviews = await _contentService.GetReviewsByContentIdAsync(contentId);

            if (reviews == null || reviews.Count == 0)
            {
                return NotFound($"No reviews found for content ID: {contentId}");
            }

            return Ok(reviews);
        }
         
        [HttpPost("{contentId}/add-review")]
        public async Task<IActionResult> AddContentReview(int contentId, [FromBody] Review review)
        {
            if (review == null)
            {
                return BadRequest("Review cannot be null.");
            }

            review.ContentId = contentId;
            review.Date = DateTime.UtcNow;

            var createdReview = await _contentService.AddReviewAsync(review);

            return CreatedAtAction(nameof(GetContentReviews),
                new { contentId = createdReview.ContentId }, createdReview);
        }

        [HttpDelete("{reviewId}/delete-review")]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            var review = await _contentService.GetReviewByIdAsync(reviewId);
            if (review == null)
                return NotFound($"No review found with ID: {reviewId}");

            await _contentService.DeleteReviewAsync(reviewId);
            return NoContent();
        }

        [HttpPost("add-critique")]
        public async Task<IActionResult> AddContentCritique([FromBody] Critique critique)
        {
            if (critique == null)
                return BadRequest(new { message = "Critique cannot be null." });

            if (critique.ContentId == 0 || critique.CriticId == 0)
                return BadRequest(new { message = "ContentId and CriticId must be provided." });

            try
            {
                var created = await _contentService.AddCritiqueAsync(critique);
                return Ok(created);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An unexpected error occurred." });
            }
        }


        [HttpGet("{contentId}/content-critiques")]
        public async Task<IActionResult> GetContentCritiques(int contentId)
        {
            var critiques = await _contentService.GetCritiquesByContentIdAsync(contentId);

            if (critiques == null || critiques.Count == 0)
            {
                return NotFound($"No critiques found for content ID: {contentId}");
            }

            return Ok(critiques);
        }
    }
}
