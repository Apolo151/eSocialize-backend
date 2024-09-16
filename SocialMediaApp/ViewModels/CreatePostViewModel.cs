using SocialMediaApp.Models;

namespace SocialMediaApp.ViewModels
{
    public class CreatePostViewModel
    {
        public required string Title { get; set; }
        public required string Content { get; set; }

        public Status? Status { get; set; }

        public byte[]? Image { get; set; }
        public int AuthorId { get; set; }
    }
}
