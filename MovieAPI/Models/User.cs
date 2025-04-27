using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models
{
    [Table("korisnik")]
    public class User
    {
        [Key]
        [Column("id_k")]
        public int Id { get; set; }

        [Column("ime_k")]
        public string FirstName { get; set; }

        [Column("prz_k")]
        public string LastName { get; set; }

        [Column("email_k")]
        public string Email { get; set; }

        [Column("loz_k")]
        public string PasswordHash { get; set; }

        [Column("uloga_k")]
        public string Role { get; set; }

        [Column("rang_k")]
        public int Rank { get; set; }

        [Column("slika_k")]
        public string? ImageUrl { get; set; }

        [Column("status_k")]
        public string Status { get; set; }
    }
}
