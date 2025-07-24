using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MovieAPI.Models.User
{
    [Table("prati")]
    public class Follow
    {
        [Key, Column("id_k", Order = 0)]
        public int FollowerId { get; set; }

        [Key, Column("id_k1", Order = 1)]
        public int FolloweeId { get; set; }

        [ForeignKey("FollowerId")]
        public User? Follower { get; set; }

        [ForeignKey("FolloweeId")]
        public User? Followee { get; set; }
    }
    public class FollowRequest
    {
        public int FollowerId { get; set; }
        public int FolloweeId { get; set; }
    }

}
