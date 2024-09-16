namespace SocialMediaApp.ViewModels
{
    public class UpdateAuthorViewModel
    {
        public required string UserName { get; set; }
         public required string Password { get; set; }
        public required string Email { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
    
    }
}
