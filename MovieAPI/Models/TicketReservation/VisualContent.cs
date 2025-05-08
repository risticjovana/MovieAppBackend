using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.TicketReservation
{
    [Table("vizuelni_sadrzaj")]
    public class VisualContent
    {
        [Key]
        [Column("id_s")]
        public int ContentId { get; set; }

        [Column("naziv_s")]
        public string Name { get; set; }

        [Column("opis_s")]
        public string Description { get; set; }

        [Column("pr_ocena")]
        public decimal Rating { get; set; }

        [NotMapped]
        public ContentType ContentType
        {
            get => Enum.Parse<ContentType>(ContentTypeString);
            set => ContentTypeString = value.ToString();
        }

        [Column("tip_s")]
        public string ContentTypeString { get; set; }

        [Column("godina")]
        public int Year { get; set; }

        [Column("favorizuj")]
        public bool IsFavorite { get; set; }

        [Column("odgledano")]
        public int Watched { get; set; }

        [Column("id_r")]
        public int DirectorId { get; set; }

        // Optionally, include the Director entity if you need to access it directly
        // [ForeignKey("DirectorId")]
        // public virtual Director Director { get; set; }
    }
}
