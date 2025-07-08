using Microsoft.EntityFrameworkCore;
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

    }
}
