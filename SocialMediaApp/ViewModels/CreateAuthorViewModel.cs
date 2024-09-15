namespace SocialMediaApp.ViewModels
{
    public class CreateAuthorViewModel
    {
        public int Id { get; set; }
        public required string UserName { get; set; }
         public required string Password { get; set; }
        public required string Email { get; set; }
        public string? Bio { get; set; }
        public byte[]? ProfilePicture { get; set; }
    
    }
}
