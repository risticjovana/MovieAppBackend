using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.TicketReservation
{
    [Table("film")]
    public class Movie
    {
        [Key]
        [Column("id_s")]
        public int ContentId { get; set; }

        [Column("trajanje_f")]
        public int Duration { get; set; }

        [Column("imageurl")]
        public string Image { get; set; }

        public virtual VisualContent VisualContent { get; set; }
    }
}
