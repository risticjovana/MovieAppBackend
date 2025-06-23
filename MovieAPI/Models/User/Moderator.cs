using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.User
{
    [Table("moderator")]
    public class Moderator
    {
        [Key]
        [Column("id_k")]
        public int Id { get; set; }
    }
}
