namespace MovieAPI.Models.TicketReservation
{
    public class CinemaWithProjectionsDTO
    {
        public int CinemaId { get; set; }
        public string CinemaName { get; set; }
        public string Type { get; set; }
        public List<ProjectionDTO> Projections { get; set; }
    }
}
