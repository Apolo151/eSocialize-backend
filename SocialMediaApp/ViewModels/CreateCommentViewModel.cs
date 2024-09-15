namespace SocialMediaApp.ViewModels
{
    public class CreateCommentViewModel
    {
        public int PostId { get; set; }
        public int CommenterId { get; set; }
        public required string Content { get; set; }

    }
}
