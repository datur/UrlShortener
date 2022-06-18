using System;
using Microsoft.EntityFrameworkCore;

namespace UrlShortener.Models
{
	public class ShortenedUrlContext : DbContext
	{
		public ShortenedUrlContext(DbContextOptions<ShortenedUrlContext> options) : base(options)
		{
		}

		public DbSet<ShortenedUrl> ShortenedUrls { get; set; } = null!;


	}
}

