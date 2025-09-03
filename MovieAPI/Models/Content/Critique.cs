using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.Content
{
    [Table("kritika")]
    public class Critique
    {
        [Key]
        [Column("id_kr")]
        public int CritiqueId { get; set; }

        [Column("datum")]
        public DateTime Date { get; set; }

        [Column("opis_kr")]
        public string Description { get; set; }

        [Column("ocena")]
        public int Rating { get; set; }

        [Column("id_s")]
        [ForeignKey("VisualContent")]
        public int ContentId { get; set; }

        [Column("id_k")]
        [ForeignKey("FilmCritic")]
        public int CriticId { get; set; }
    }
}
