namespace SocialMediaApp.ViewModels
{
    public class AuthorViewModel
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public List<int>? Followings { get; set; }
    }
}
