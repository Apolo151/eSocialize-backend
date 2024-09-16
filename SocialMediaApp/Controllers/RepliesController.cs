using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApp.Models;
using SocialMediaApp.ViewModels;

namespace SocialMediaApp
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepliesController : ControllerBase
    {
        private readonly SocialMediaContext _context;
        public RepliesController(SocialMediaContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddReply([FromBody] CreateReplyViewModel model)
        {
            var comment = _context.Comments.FirstOrDefault(c => c.Id == model.CommentId);
            if (comment == null)
            {
                return NotFound("Comment not found");
            }

            var newReply = new Reply
            {
                CommentId = model.CommentId,
                Content = model.Content,
                ReplierId = model.ReplierId
            };

            _context.Replies.Add(newReply);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpGet]
        public ActionResult GetReplies(int commentId)
        {
            var replies = _context.Replies.Where(r => r.CommentId == commentId).ToList();
            return Ok(replies);
        }

        [Authorize]
        [HttpDelete]
        public ActionResult DeleteReply(int replyId)
        {
            var reply = _context.Replies.FirstOrDefault(r => r.Id == replyId);
            if (reply == null)
            {
                return NotFound("Reply not found");
            }
            _context.Replies.Remove(reply);
            _context.SaveChanges();
            return Ok();
        }
    }
}
