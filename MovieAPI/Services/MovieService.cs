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
                                             Image = m.Image,
                                         })
                            .Distinct()
                            .ToListAsync();

            return availableMovies;
        }

        public async Task<List<CinemaWithProjectionsDTO>> GetGroupedProjectionsByContentIdAsync(int contentId)
        {
            var groupedProjections = await (from p in _dbContext.Projections
                                            where p.ContentId == contentId
                                            join c in _dbContext.Cinemas on p.CinemaId equals c.CinemaId
                                            group new { p, c } by new { c.CinemaId, c.Name } into g
                                            select new CinemaWithProjectionsDTO
                                            {
                                                CinemaId = g.Key.CinemaId,
                                                CinemaName = g.Key.Name,
                                                Projections = g.Select(x => new ProjectionDTO
                                                {
                                                    Id = x.p.Id,
                                                    Date = x.p.Date,
                                                    Time = x.p.Time,
                                                    AvailableTickets = x.p.AvailableTickets,
                                                    RoomNumber = x.p.RoomNumber,
                                                    SeatNumber = x.p.SeatNumber
                                                }).ToList()
                                            }).ToListAsync();

            return groupedProjections;
        }

        public async Task<bool> ReserveTicketAsync(Ticket ticket)
        {
            try
            {
                ticket.TicketId = GetNextTicketId();

                await _dbContext.Tickets.AddAsync(ticket);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<Ticket>> GetTicketsByUserIdAsync(int userId)
        {
            return await _dbContext.Tickets
                .Where(t => t.UserId == userId)
                .ToListAsync();
        }

        private int GetNextTicketId()
        {
            var lastTicket = _dbContext.Tickets.OrderByDescending(t => t.TicketId).FirstOrDefault();
            return lastTicket == null ? 1 : lastTicket.TicketId + 1;
        }
    }
}
