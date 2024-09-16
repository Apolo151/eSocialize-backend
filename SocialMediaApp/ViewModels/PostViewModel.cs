using SocialMediaApp.Models;

namespace SocialMediaApp.ViewModels
{
    public class PostViewModel
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Content { get; set; }
        public byte[]? Image { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public AuthorViewModel? Author { get; set; }
        public List<CommentViewModel>? Comments { get; set; }
        public List<String>? Likes { get; set; }

        public bool? IsFollowedAuthor { get; set; }
    }
}
