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

    }
}
