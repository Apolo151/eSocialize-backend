using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Models;
using SocialMediaApp.ViewModels;
using SocialMediaApp.Controllers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SocialMediaApp.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public partial class AuthorsController : ControllerBase
	{
		private readonly SocialMediaContext _context;

		public AuthorsController(SocialMediaContext context)
		{
			_context = context;
		}

		[HttpGet]
		public async Task<ActionResult<List<AuthorViewModel>>> GetAll()
		{
			var result = await _context.Authors.Select(a => new AuthorViewModel
			{
				Id = a.Id,
				UserName = a.UserName,
				Email = a.Email,
				Bio = a.Bio,
				ProfilePicture = a.ProfilePicture

			}).ToListAsync();

			return Ok(result);
		}

		[HttpGet("{Id}")]
		public async Task<ActionResult<AuthorViewModel>> GetAuthorById([FromRoute] int Id)
		{
			var result = await _context.Authors.Where(a => a.Id == Id).FirstOrDefaultAsync();

			if(result == null)
			{
				return NotFound("Author not found");
			}

			return Ok(result);
		}

		[HttpPost]
		public async Task<ActionResult> Create([FromBody] CreateAuthorViewModel model)
		{
			var newAuthor = new Author
			{
				UserName = model.UserName,
				Email = model.Email,
				// TODO: handle null possibility
				Password = AuthController.ComputeSha256Hash(model.Password),
				Bio = model.Bio,
				ProfilePicture = model.ProfilePicture
			};

			_context.Authors.Add(newAuthor);
			await _context.SaveChangesAsync();

			return Ok();
		}

		[Authorize]
		[HttpPut("{Id}")]
		public async Task<ActionResult> Update([FromRoute] int Id, [FromBody] UpdateAuthorViewModel model)
		{
			var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized("Invalid Token");
            }

			var author = await _context.Authors.Where(a => a.Id == Id).FirstOrDefaultAsync();

			if(author == null)
			{
				return NotFound("Author not found");
			}

			if(author.Id != int.Parse(userId))
			{
				return Unauthorized("You are not authorized to update another author");
			}

			author.UserName = model.UserName;
			author.Password = AuthController.ComputeSha256Hash(model.Password);
			author.Email = model.Email;
			author.Bio = model.Bio;
			author.ProfilePicture = model.ProfilePicture;

			await _context.SaveChangesAsync();

			return Ok();
		}
	}
}
