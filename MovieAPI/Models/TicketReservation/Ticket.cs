using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.TicketReservation
{
    [Table("karta")]
    public class Ticket
    {
        [Key]
        [Column("id_krt")]
        public int TicketId { get; set; }

        [Column("id_pr")]
        public int ProjectionId { get; set; }

        [Column("id_s")]
        public int ContentId { get; set; }

        [Column("id_k")]
        public int UserId { get; set; }

        [Column("br_sed")]
        public int SeatNumber { get; set; }

        [Column("br_sale")]
        public int RoomNumber { get; set; }

        [Column("datum_rez")]
        public DateTime PurchaseTime { get; set; }
    }
}
