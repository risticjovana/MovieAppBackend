using Microsoft.EntityFrameworkCore;
using MovieAPI.Models.Content;
using MovieAPI.Models.TicketReservation;

namespace MovieAPI.Services
{
    public class ContentService
    {
        private readonly AppDbContext _dbContext;

        public ContentService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<VisualContent>> GetMoviesByGenreAsync(string genreName)
        {
            var movies = await (from vs in _dbContext.VisualContents
                                join p in _dbContext.Pripada on vs.ContentId equals p.ContentId
                                join z in _dbContext.Genres on p.GenreId equals z.GenreId
                                where z.Name.ToLower() == genreName.ToLower() && vs.ContentTypeString == "Movie"
                                select vs)
                        .Distinct()
                        .ToListAsync();

            return movies;
        }

        public async Task<List<VisualContent>> GetTVSeriesByGenreAsync(string genreName)
        {
            var series = await (from vs in _dbContext.VisualContents
                                join p in _dbContext.Pripada on vs.ContentId equals p.ContentId
                                join z in _dbContext.Genres on p.GenreId equals z.GenreId
                                where z.Name.ToLower() == genreName.ToLower() && vs.ContentTypeString == "TVSeries"
                                select vs)
                        .Distinct()
                        .ToListAsync();

            return series;
        }

        public async Task<List<Review>> GetReviewsByContentIdAsync(int contentId)
        {
            return await _dbContext.Reviews
                .Where(r => r.ContentId == contentId)
                .ToListAsync();
        }

        public async Task<Review> AddReviewAsync(Review review)
        { 
            var maxId = await _dbContext.Reviews.MaxAsync(r => (int?)r.ReviewId) ?? 0; 
            review.ReviewId = maxId + 1;

            _dbContext.Reviews.Add(review);
            await _dbContext.SaveChangesAsync();

            return review;
        }

        public async Task<Review?> GetReviewByIdAsync(int reviewId)
        {
            return await _dbContext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
        }

        public async Task DeleteReviewAsync(int reviewId)
        {
            var review = await _dbContext.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
            if (review != null)
            {
                _dbContext.Reviews.Remove(review);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<Critique> AddCritiqueAsync(Critique critique)
        { 
            var maxId = await _dbContext.Critiques.MaxAsync(c => (int?)c.CritiqueId) ?? 0;
            critique.CritiqueId = maxId + 1;
             
            var contentExists = await _dbContext.VisualContents.AnyAsync(vc => vc.ContentId == critique.ContentId);
            var criticExists = await _dbContext.Critics.AnyAsync(fc => fc.Id == critique.CriticId);

            if (!contentExists)
                throw new ArgumentException($"VisualContent with ID {critique.ContentId} does not exist.");
             
            _dbContext.Critiques.Add(critique);
            await _dbContext.SaveChangesAsync();

            return critique;
        }

        public async Task<List<Critique>> GetCritiquesByContentIdAsync(int contentId)
        {
            return await _dbContext.Critiques
                .Where(c => c.ContentId == contentId)
                .ToListAsync();
        }
    }
}
