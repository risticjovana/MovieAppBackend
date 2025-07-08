using Microsoft.AspNetCore.Mvc;
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
    }
}
