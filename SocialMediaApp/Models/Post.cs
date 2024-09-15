using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace SocialMediaApp.Models
{
    public enum Status
    {
        Public,
        FriendsOnly
    }
    public class Post
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public Status? Status {get; set;}
        public byte[]? Image { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int AuthorId { get; set; }
        public Author? Author { get; set; }
        public List<Comment>? Comments { get; set; }
        public List<Like>? Likes { get; set; }

    }

}
