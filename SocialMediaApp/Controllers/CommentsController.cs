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

        [HttpPost]
        public ActionResult AddComment([FromBody] CreateCommentViewModel model)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == model.PostId);
            if (post == null)
            {
                return NotFound("Post not found");
            }

            var newComment = new Comment
            {
              
                PostId = model.PostId,
                Content = model.Content,
                AuthorId = model.AuthorId
            };

           _context.Comments.Add(newComment);
            _context.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public ActionResult GetComments(int postId)
        {
            var comments = _context.Comments.Include(c => c.Author).Where(c => c.PostId == postId).ToList();
            return Ok(comments);
        }
        [HttpDelete]
        public ActionResult DeleteComment(int commentId)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == commentId);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            _context.Comments.Remove(comment);
            _context.SaveChanges();
            return Ok();
        }
        [HttpPut]
        public ActionResult EditComment([FromBody] EditCommentViewModel model)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == model.Id);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }
            comment.Content = model.Content;
            _context.SaveChanges();
            return Ok();
        }
    }

}