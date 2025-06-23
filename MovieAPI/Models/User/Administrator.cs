using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.User
{
    [Table("administrator")]
    public class Administrator
    {
        [Key]
        [Column("id_k")]
        public int Id { get; set; }
    }
}
