using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Models;

namespace SocialMediaApp
{
	public class SocialMediaContext : DbContext
	{
		public DbSet<Author> Authors { get; set; }

		public DbSet<Follow> Follows { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Like> Likes { get; set; }
		public DbSet<Reply> Replies { get; set; }

		public SocialMediaContext(DbContextOptions<SocialMediaContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Add any custom configurations here
		}
	}
}
