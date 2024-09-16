using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Models;
using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class FollowsController : ControllerBase
    {
        private readonly SocialMediaContext _context;
        public FollowsController(SocialMediaContext context)
        {
            _context = context;
        }

        // Following
		[Authorize]
		[HttpPost("Followings")]

		public async Task<ActionResult> Follow([FromBody] Follow model)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Invalid Token");
            }
			//
			var follower = await _context.Authors.FirstOrDefaultAsync(a => a.Id == int.Parse(userId));
            var followee = await _context.Authors.FirstOrDefaultAsync(a => a.Id == model.FolloweeId);

            if (follower == null || followee == null)
            {
                return NotFound("follower or followee not found");
            }

            var newFollow = new Follow {
                FollowerId = int.Parse(userId),
                FolloweeId = model.FolloweeId
            };

            _context.Follows.Add(newFollow);
            _context.SaveChanges();
			return Ok();
		}

		[Authorize]
		[HttpGet("Followings")]
		public async Task<ActionResult<List<AuthorViewModel>>> GetFollowings()
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return Unauthorized("Invalid Token");
			}

			// get all Follow where FollowerId == userId
            var result = await _context.Follows
                .Where(f => f.FollowerId == int.Parse(userId))
                .Select(f => new AuthorViewModel
                {
                    Id = f.FolloweeId,
                    UserName = f.Followee.UserName,
                    Email = f.Followee.Email,
                    Bio = f.Followee.Bio,
                    ProfilePicture = f.Followee.ProfilePicture
                })
                .ToListAsync();

			return Ok(result);
		}

		[Authorize]
		[HttpDelete("Followings")]
		public async Task<ActionResult> Unfollow([FromBody] FollowViewModel model)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (userId == null)
			{
				return Unauthorized("Invalid Token");
			}

            var follow = await _context.Follows.FirstOrDefaultAsync(f => f.FollowerId == int.Parse(userId) && f.FolloweeId == model.FolloweeId);

            if (follow == null)
            {
                return NotFound("Follow not found");
            }

            _context.Follows.Remove(follow);
            _context.SaveChanges();

			return Ok();
		}
    }
    
}