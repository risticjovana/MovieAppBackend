using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.User
{
    [Table("obican_korisnik")]
    public class RegularUser
    {
        [Key]
        [Column("id_k")]
        public int Id { get; set; }

    }
}
