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
    public class CommentsController : ControllerBase
    {
        private readonly SocialMediaContext _context;
        public CommentsController(SocialMediaContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddComment([FromBody] CreateCommentViewModel model)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == model.PostId);
            if (post == null)
            {
                return NotFound("Post not found");
            }

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Invalid Token");
            }

            var newComment = new Comment
            {
              
                PostId = model.PostId,
                Content = model.Content,
                CommenterId = int.Parse(userId)
            };

           _context.Comments.Add(newComment);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpGet("{postId}")]
        public ActionResult GetComments(int postId)
        {
            var comments = _context.Comments.Include(c => c.Commenter).Where(c => c.PostId == postId).ToList();
            return Ok(comments);
        }
        [HttpDelete("{commentId}")]
        public ActionResult DeleteComment(int commentId)
        {

            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Invalid Token");
            }

            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            if(comment.CommenterId != int.Parse(userId))
            {
                return Unauthorized("You are not authorized to delete this comment");
            }

            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpPut("{Id}")]
        public ActionResult EditComment([FromRoute] int Id, [FromBody] EditCommentViewModel model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Invalid Token");
            }

            var comment = _context.Comments.FirstOrDefault(c => c.Id == Id);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            if(comment.CommenterId != int.Parse(userId))
            {
                return Unauthorized("You are not authorized to edit this comment");
            }

            comment.Content = model.Content;
            _context.SaveChanges();
            return Ok();
        }
    }

}