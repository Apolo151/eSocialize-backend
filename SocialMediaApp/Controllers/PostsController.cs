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
            var result = await _context.Posts.Include(p => p.Author).Include(p => p.Comments).Select(p => new PostViewModel()
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                Author = new AuthorViewModel()
                {
                    Id = p.Author.Id,
                    UserName = p.Author.UserName
                },
                Comments = p.Comments.Select(p => p.Content).ToList()
            }).ToListAsync();


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
                    Comments = p.Comments.Select(p => p.Content).ToList()
                }).ToListAsync();


            return Ok(result);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreatePostViewModel model)
        {
            var newPost = new Post
            {
                Title = model.Title,
                Content = model.Content,
                AuthorId = model.AuthorId
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
                Comments = p.Comments.Select(p => p.Content).ToList()
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

