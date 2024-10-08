using System.Net;
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
    public partial class PostsController : ControllerBase
    {
        private readonly SocialMediaContext _context;
        public static List<Post> _posts = new List<Post>();
        private static List<Author> _authors = new List<Author>();

        public PostsController(SocialMediaContext context)
        {
            _context = context;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<PostViewModel>>> GetAll()
        {
            // Assuming you have a method to get the current user's followed authors
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var followedAuthorIds = await _context.Follows
                .Where(f => f.FollowerId == int.Parse(currentUserId))
                .Select(f => f.FolloweeId)
                .ToListAsync();

            var result = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Author = new AuthorViewModel()
                    {
                        Id = p.Author.Id,
                        UserName = p.Author.UserName
                    },
                   Comments = p.Comments.Select(c => new CommentViewModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CommenterId = c.CommenterId,
                        CommenterName = c.Commenter.UserName,
                    }).ToList(),
                    IsFollowedAuthor = followedAuthorIds.Contains(p.Author.Id)
                })
                .OrderByDescending(p => p.IsFollowedAuthor) // Order by followed authors first
                .ToListAsync();

            return Ok(result);
        }

        [Authorize]
        [HttpGet("Author/{authorId}")]
        public async Task<ActionResult<List<PostViewModel>>> GetAll(int authorId)
        {
            var result = await _context.Posts
                .Include(p => p.Author)
                .Include(p => p.Comments)
                .Where(p => p.AuthorId == authorId)
                .Select(p => new PostViewModel()
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content,
                    Author = new AuthorViewModel()
                    {
                        Id = p.Author.Id,
                        UserName = p.Author.UserName
                    },
                    Comments = p.Comments.Select(c => new CommentViewModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CommenterId = c.CommenterId,
                        CommenterName = c.Commenter.UserName,
                    }).ToList()
                }).ToListAsync();


            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePostViewModel model)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Invalid Token");
            }

            var newPost = new Post
            {
                Title = model.Title,
                Content = model.Content,
                AuthorId = int.Parse(userId)
            };

            _context.Posts.Add(newPost);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [Authorize]
        [HttpGet("{Id}")]
        public async Task<ActionResult<PostViewModel>> GetPostById([FromRoute] int Id)
        {
            var result = await _context.Posts.Include(p => p.Author).Include(p => p.Comments).Where(a => a.Id == Id).Select(p => new PostViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                Author = new AuthorViewModel()
                {
                    Id = p.Author.Id,
                    UserName = p.Author.UserName
                },
                Comments = p.Comments.Select(c => new CommentViewModel
                    {
                        Id = c.Id,
                        Content = c.Content,
                        CommenterId = c.CommenterId,
                        CommenterName = c.Commenter.UserName,
                    }).ToList(),
            }).FirstOrDefaultAsync();

            return Ok(result);
        }

        [Authorize]
        [HttpDelete("{Id}")]
        public async Task<ActionResult> Delete([FromRoute] int Id)
        {
            var post = await _context.Posts.Where(p => p.Id == Id).ExecuteDeleteAsync();

            return Ok();
        }


        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] PostViewModel updatedPost)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound("Post not found");
            }

            post.Content = updatedPost.Content;
            post.Title = updatedPost.Title;

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

