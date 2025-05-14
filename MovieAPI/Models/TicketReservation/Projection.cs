using MovieAPI.Models.TicketReservation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.TicketReservation
{
    [Table("projekcija")]
    public class Projection
    {
        [Key]
        [Column("id_pr")]
        public int Id { get; set; }

        [Column("datum")]
        public DateTime Date { get; set; }

        [Column("br_sl_krt")]
        public int AvailableTickets { get; set; }

        [Column("vreme")]
        public TimeSpan Time { get; set; }

        [Column("br_sale")]
        public int RoomNumber { get; set; }

        [Column("id_s")]
        public int ContentId { get; set; }

        [Column("id_b")]
        public int CinemaId { get; set; }
    }
}
