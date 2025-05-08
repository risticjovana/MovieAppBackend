using MovieAPI.Models.TicketReservation;
using Microsoft.EntityFrameworkCore;

namespace MovieAPI.Services
{
    public class MovieService
    {
        private readonly AppDbContext _dbContext;

        public MovieService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // This method retrieves all movies based on projections.
        public async Task<List<MovieDTO>> GetAllAvailableMoviesAsync()
        {
            // Join Projections to VisualContent and Movies by ContentId
            var availableMovies = await (from p in _dbContext.Projections
                                         join vc in _dbContext.VisualContents on p.ContentId equals vc.ContentId
                                         join m in _dbContext.Movies on p.ContentId equals m.ContentId
                                         select new MovieDTO
                                         {
                                             ContentId = vc.ContentId,
                                             Name = vc.Name,
                                             Description = vc.Description,
                                             Rating = vc.Rating,
                                             ContentTypeString = vc.ContentTypeString, 
                                             Year = vc.Year,
                                             IsFavorite = vc.IsFavorite,
                                             Watched = vc.Watched,
                                             Duration = m.Duration,
                                             DirectorId = vc.DirectorId,
                                         }).ToListAsync();

            return availableMovies;
        }
    }
}
