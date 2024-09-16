namespace SocialMediaApp.ViewModels
{
    public class CommentViewModel
    {
        public int Id { get; set; }

        public int PostId { get; set; }
        public required int CommenterId { get; set; }
        public required string CommenterName { get; set; }
        public required string Content { get; set; }

        public DateTime CreatedAt { get; set; }

    }
}
