using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.Collections
{
    [Table("je_deo")]
    public class CollectionItem
    {
        [Column("id_s")]
        public int ContentId { get; set; }

        [Column("id_kol")]
        public int CollectionId { get; set; }
    }
}
