using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.Models;
using Microsoft.Extensions.Logging;

namespace SocialMediaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public partial class AuthController : ControllerBase
    {
        private readonly SocialMediaContext _context;
        private readonly JwtOptions _jwtOptions;
        private readonly ILogger<AuthController> _logger;

        public AuthController(SocialMediaContext context, JwtOptions jwtOptions, ILogger<AuthController> logger)
        {
            _jwtOptions = jwtOptions;
            _context = context;
            _logger = logger;
        }

        public static string ComputeSha256Hash(string rawData)
        {
            using (var sha256Hash = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                var builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
            var user = _context.Authors.FirstOrDefault(u => u.UserName == request.Username);
            if (user == null)
            {
                _logger.LogWarning("User not found: {Username}", request.Username);
                return NotFound("User not found");
            }

            // Use a proper hashing algorithm for password comparison
            var hashedPassword = ComputeSha256Hash(request.Password);
            if (user.Password != hashedPassword)
            {
                _logger.LogWarning("Invalid password for user: {Username}", request.Username);
                return Unauthorized("Invalid password");
            }

            _logger.LogInformation("User authenticated: {Username}", request.Username);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Key)), SecurityAlgorithms.HmacSha256Signature),
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);
            return Ok(accessToken);
        }
    }
}