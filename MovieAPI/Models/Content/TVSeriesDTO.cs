using MovieAPI.Models.TicketReservation;

namespace MovieAPI.Models.Content
{
    public class TVSeriesDTO: VisualContent
    {
        public int NumberOfSeasons { get; set; }
        public int NumberOfEpisodes { get; set; }
    }
}
