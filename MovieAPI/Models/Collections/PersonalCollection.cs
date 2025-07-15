using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.Collections
{
    [Table("licne_kolekcije")]
    public class PersonalCollection
    {
        [Key]
        [Column("id_kol")]
        public int CollectionId { get; set; }

        [Column("id_k")]
        public int UserId { get; set; }
    }
}
