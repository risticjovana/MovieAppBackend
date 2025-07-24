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
    }

} 
public class SaveCollectionRequest
{
    public int UserId { get; set; }
    public int CollectionId { get; set; }
}