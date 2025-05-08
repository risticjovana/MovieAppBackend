using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.TicketReservation
{
    [Table("bioskop")]
    public class Cinema
    {
        [Key]
        [Column("id_b")]
        public int CinemaId { get; set; }

        [Column("naziv_b")]
        public string Name { get; set; }

        [Column("lokacija")]
        public string Location { get; set; }

        [Column("tip_b")]
        public string Type { get; set; } // e.g., "zatvoreni" or "otvoreni"
    }
}
