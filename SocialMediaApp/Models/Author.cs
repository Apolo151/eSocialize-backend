using SocialMediaApp.Controllers;
using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Models
{
	public class Author
	{
		public int Id { get; set; }
		public required string UserName { get; set; }
		public required string Password { get; set; }
		public string? Email { get; set; }
		public string? Bio { get; set; }
		public string? ProfilePicture { get; set; }
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public List<Post> Posts { get; set; } = new List<Post>();

		public List<FollowViewModel>? Followings { get; set; } = new List<FollowViewModel>(); // people author follows
    }

}
