using MovieAPI.Models.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.Collections
{
    [Table("komentar")]
    public class Comment
    {
        [Column("id_k")]
        public int UserId { get; set; }

        [Column("id_kol")]
        public int CollectionId { get; set; }

        [Key]
        [Column("id_kom")]
        public int Id { get; set; }

        [Column("opis_kom")]
        public string Text { get; set; }

        [Column("datum")]
        public DateTime Date { get; set; }

        [Column("id_kor")]
        public int ModeratorId { get; set; }
    }
}
