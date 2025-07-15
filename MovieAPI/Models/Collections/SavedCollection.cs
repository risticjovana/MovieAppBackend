using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 
using MovieAPI.Models.User; 

namespace MovieAPI.Models.Collections
{
    [Table("cuva")]
    public class SavedCollection
    {
        [Key, Column("id_k", Order = 0)]
        public int UserId { get; set; }

        [Key, Column("id_kol", Order = 1)]
        public int CollectionId { get; set; }

        [ForeignKey("UserId")]
        public RegularUser User { get; set; }

        [ForeignKey("CollectionId")]
        public MovieCollection Collection { get; set; }
    }
}
