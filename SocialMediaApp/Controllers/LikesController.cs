using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Models;
using SocialMediaApp.ViewModels;

namespace SocialMediaApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LikesController : ControllerBase
    {
        private readonly SocialMediaContext _context;
        public LikesController(SocialMediaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult AddLike([FromBody] CreateLikeViewModel model)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == model.PostId);
            if (post == null)
            {
                return NotFound("Post not found");
            }

            var newLike = new Like
            {
                PostId = model.PostId,
                AuthorId = model.AuthorId
            };

            _context.Likes.Add(newLike);
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public ActionResult GetLikes(int postId)
        {
            var likes = _context.Likes.Include(l => l.Author).Where(l => l.PostId == postId).ToList();
            return Ok(likes);
        }

        [HttpDelete]
        public ActionResult DeleteLike(int likeId)
        {
            var like = _context.Likes.FirstOrDefault(l => l.Id == likeId);
            if (like == null)
            {
                return NotFound("Like not found");
            }
            _context.Likes.Remove(like);
            _context.SaveChanges();
            return Ok();
        }
    }

}