namespace MovieAPI.Models.TicketReservation
{
    public class ProjectionDTO
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public int AvailableTickets { get; set; }
        public int RoomNumber { get; set; }
    }
}
