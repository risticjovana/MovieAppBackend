using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.User
{
    [Table("urednik_sadrzaja")]
    public class Editor
    {
        [Key]
        [Column("id_k")]
        public int Id { get; set; }
    }
}
