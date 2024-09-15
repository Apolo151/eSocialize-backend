namespace SocialMediaApp.Models
{

	public class Comment
	{
		public int Id { get; set; }
		public required string Content { get; set; }
		public required int PostId { get; set; }
		public Post? Post { get; set; }
        public required int CommenterId { get; set; }
        public Author? Commenter { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
