using Microsoft.AspNetCore.Mvc;

namespace SocialMediaApp.ViewModels
{
    public class AuthorViewModel
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public List<int>? Followings { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
