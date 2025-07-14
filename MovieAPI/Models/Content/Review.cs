using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.Content
{
        [Table("recenzija")]
        public class Review
        {
            [Key]
            [Column("id_r")]
            public int ReviewId { get; set; }            

            [Column("datum")]
            public DateTime Date { get; set; }            

            [Column("tekst_r")]
            public string Description { get; set; }            

            [Column("ocena")]
            public int Rating { get; set; }             

            [Column("id_s")]
            public int ContentId { get; set; }            

            [Column("id_k")]
            public int UserId { get; set; }            
        }
}
