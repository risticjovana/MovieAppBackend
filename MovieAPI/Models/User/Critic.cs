using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.User
{
    [Table("filmski_kriticar")]
    public class Critic
    {
        [Key]
        [Column("id_k")]
        public int Id { get; set; }
    }
}
