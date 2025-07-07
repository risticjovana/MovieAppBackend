using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.TicketReservation
{
    [Table("pripada")]
    public class Pripada
    {
        [Column("id_s")]
        public int ContentId { get; set; }

        [Column("id_z")]
        public int GenreId { get; set; }

        [ForeignKey("ContentId")]
        public VisualContent VisualContent { get; set; }

        [ForeignKey("GenreId")]
        public Genre Genre { get; set; }
    }
}
