namespace SocialMediaApp.Models
{

	public class Reply
	{
		public int Id { get; set; }
		public required string Content { get; set; }
		public required int CommentId { get; set; }
		public Comment? Comment { get; set; }
        public required int ReplierId { get; set; }
        public Author? Replier { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
