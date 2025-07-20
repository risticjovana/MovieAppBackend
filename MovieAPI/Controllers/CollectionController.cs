using Microsoft.AspNetCore.Mvc;
using MovieAPI.Models.Collections;
using MovieAPI.Services;

namespace MovieAPI.Controllers
{
    [Route("api/collections")]
    [ApiController]
    public class CollectionController : ControllerBase
    {
        private readonly CollectionService _collectionService;

        public CollectionController(CollectionService collectionService)
        {
            _collectionService = collectionService;
        }

        [HttpPost("create-personal")]
        public async Task<IActionResult> CreatePersonalCollection([FromBody] CollectionDTOs.CreatePersonalCollectionRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            var collection = await _collectionService.CreatePersonalCollectionAsync(
                request.Name, request.Description, request.UserId);

            return CreatedAtAction(nameof(GetPersonalCollectionsByUserId),
                new { userId = request.UserId }, collection);
        }

        [HttpPost("create-editorial")]
        public async Task<IActionResult> CreateEditorialCollection([FromBody] CollectionDTOs.CreateEditorialCollectionRequest request)
        {
            if (request == null)
                return BadRequest("Invalid request.");

            var collection = await _collectionService.CreateEditorialCollectionAsync(
                request.Name, request.Description, request.ModeratorId, request.EditorId);

            return Ok(collection);
        }

        [HttpPost("{collectionId}/add-content/{contentId}")]
        public async Task<IActionResult> AddContentToCollection(int collectionId, int contentId, int userId)
        {
            var success = await _collectionService.AddContentToCollectionAsync(collectionId, contentId, userId);

            if (!success)
                return BadRequest("Failed to add content to collection. Content or collection may not exist, or it may already be added.");

            return Ok("Content added to collection successfully.");
        }

        [HttpGet("personal/user/{userId}")]
        public async Task<IActionResult> GetPersonalCollectionsByUserId(int userId)
        {
            var collections = await _collectionService.GetPersonalCollectionsByUserIdAsync(userId);

            if (collections == null || collections.Count == 0)
                return NotFound($"No personal collections found for user ID: {userId}");

            return Ok(collections);
        }
         
        [HttpGet("{collectionId}/contents")]
        public async Task<IActionResult> GetAllContentByCollectionId(int collectionId)
        {
            var contents = await _collectionService.GetAllContentByCollectionIdAsync(collectionId);

            if (contents == null || contents.Count == 0)
                return NotFound($"No content found for collection ID: {collectionId}");

            return Ok(contents);
        }
         
        [HttpGet("{collectionId}")]
        public async Task<IActionResult> GetCollectionInfoById(int collectionId)
        {
            var collection = await _collectionService.GetCollectionInfoByIdAsync(collectionId);

            if (collection == null)
                return NotFound($"Collection with ID {collectionId} not found.");

            return Ok(collection);
        }
    }
}
