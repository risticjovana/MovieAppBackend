using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.TicketReservation
{
    [Table("zanr")]
    public class Genre
    {
        [Key]
        [Column("id_z")]
        public int GenreId { get; set; }

        [Column("naziv_z")]
        public string Name { get; set; }
    }
}
