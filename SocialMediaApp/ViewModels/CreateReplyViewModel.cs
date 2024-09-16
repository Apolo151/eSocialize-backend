namespace SocialMediaApp.ViewModels
{
    public class CreateReplyViewModel
    {
        public int CommentId { get; set; }
        public int ReplierId { get; set; }
        public required string Content { get; set; }

    }
}
