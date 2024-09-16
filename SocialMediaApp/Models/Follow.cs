namespace SocialMediaApp.Models
{
    public class Follow
    {
        public int Id { get; set; }
        public int FollowerId { get; set; }
        public Author? Follower { get; set; }
        public int FolloweeId { get; set; }

        public Author? Followee { get; set; }
        public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
