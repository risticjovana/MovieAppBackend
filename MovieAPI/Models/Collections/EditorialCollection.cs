using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.Collections
{
    [Table("urednicke_kolekcije")]
    public class EditorialCollection
    {
        [Key]
        [Column("id_kol")]
        public int CollectionId { get; set; }

        [Column("id_k")]
        public int? ModeratorId { get; set; }

        [Column("id_kor")]
        public int? ContentEditorId { get; set; }
    }
}
