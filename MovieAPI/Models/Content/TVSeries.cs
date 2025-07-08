using MovieAPI.Models.TicketReservation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.Content
{
    [Table("serija")]
    public class TVSeries
    {
        [Key]
        [Column("id_s")]
        public int ContentId { get; set; }

        [Column("br_sezona")]
        public int NumberOfSeasons { get; set; }

        [Column("br_epizoda")]
        public int NumberOfEpisodes { get; set; }

        public virtual VisualContent VisualContent { get; set; }
    }
}