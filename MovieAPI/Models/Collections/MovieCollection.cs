using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MovieAPI.Models.Collections
{
    [Table("kolekcija")]
    public class MovieCollection
    {
        [Key]
        [Column("id_kol")]
        public int Id { get; set; }

        [Column("naziv_kol")]
        public string Name { get; set; }

        [Column("tip_kol")]
        public string TypeString { get; set; } = null!;

        [NotMapped]
        public CollectionType Type
        {
            get => Enum.Parse<CollectionType>(TypeString, ignoreCase: true);
            set => TypeString = value.ToString();
        }

        [Column("opis_kol")]
        public string Description { get; set; }

        [Column("datum")]
        public DateTime CreatedAt { get; set; }

        [Column("br_cuvanja")]
        public int SaveCount { get; set; }

        [Column("id_k")]
        public int UserId { get; set; }


        public enum CollectionType
        {
            Licna,
            Urednicka
        }
    }
}
