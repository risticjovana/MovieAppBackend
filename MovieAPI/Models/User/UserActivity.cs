using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.User
{
    [Table("aktivnost_korisnika")]
    public class UserActivity
    {
        [Key]
        [Column("id_ak")]
        public int UserActivityId { get; set; }

        [Column("br_kolekcija")]
        public int CollectionCount { get; set; }

        [Column("br_recenzija")]
        public int ReviewCount { get; set; }

        [Column("br_komentara")]
        public int CommentCount { get; set; }

        [Column("nivo_ak")]
        public int ActivityLevel { get; set; }

        [Column("sati_gledanja")]
        public int WatchHours { get; set; }

        [Column("br_kor_cuvanja")]
        public int UserSavesCount { get; set; }

        [Column("br_sac_kol")]
        public int SavedCollectionsCount { get; set; }

        [Column("id_k")]
        public int UserId { get; set; }
    }
}
