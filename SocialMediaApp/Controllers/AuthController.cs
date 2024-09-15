using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMediaApp.Models;

namespace SocialMediaApp.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
    public partial class AuthController : ControllerBase
    {
        private readonly SocialMediaContext _context;
        private readonly JwtOptions _jwtOptions;

        public AuthController(SocialMediaContext context, JwtOptions jwtOptions) : this(context)
        {
            this._jwtOptions = jwtOptions;
        }

        public AuthController(SocialMediaContext context)
        {
            this._context = context;
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
            var user = _context.Authors.FirstOrDefault(u => u.UserName == request.Username);
            if (user == null)
            {
                return NotFound("User not found");
            }
            if (user.Password != request.Password)
            {
                return Unauthorized("Invalid password");
            }

            Console.WriteLine("User authenticated");

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
