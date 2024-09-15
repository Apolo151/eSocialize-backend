using static System.Net.Mime.MediaTypeNames;

namespace SocialMediaApp.Models
{

	public class Like
	{
		public int Id { get; set; }
		public int PostId { get; set; }
		public Post? Post { get; set; }
        public int LikerId { get; set; }
        public Author? Liker { get; set; }
        
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
