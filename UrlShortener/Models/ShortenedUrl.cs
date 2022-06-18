using System.ComponentModel.DataAnnotations;
using UrlShortener.Utils;

namespace UrlShortener.Models
{
	public class ShortenedUrl
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string Url { get; set; }
		public string ShortUrl { get { return Constants.baseUrl + Token; } }
        public string Token { get; set; }
        public int NumberOfVisits { get; set; } = 0;
		public DateTime CreatedAt { get; set; } = DateTime.Now;
		public DateTime LastAccessedAt { get; set; } = DateTime.Now;
    }
}

