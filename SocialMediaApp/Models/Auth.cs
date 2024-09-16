using static System.Net.Mime.MediaTypeNames;

namespace SocialMediaApp.Models
{
	public class AuthenticationRequest
	{
		public required string Username { get; set; }
        public required string Password { get; set; }    
    }

	public class JwtOptions
{
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Key { get; set; }
}

}
