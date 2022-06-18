using System;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;
using UrlShortener.Utils;

namespace UrlShortener.Services
{
	public class UrlShortenerService : IUrlShortenerService
	{
        private readonly ShortenedUrlContext _context;
 
        public UrlShortenerService(ShortenedUrlContext context)
		{
            _context = context;
        }

        /// <summary>
        /// Check if there are shortened urls
        /// </summary>
        /// <returns></returns>
        public bool AnyShortenedUrls()
        {
            if (_context.ShortenedUrls == null || !_context.ShortenedUrls.Any())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Method to check for a Url that has already been shortened.
        /// </summary>
        /// <param name="url">string</param>
        /// <returns>bool</returns>
        private async Task<bool> IsUrlAlreadyShortened(string url)
        {
            var shortenedUrls = await _context.ShortenedUrls.Where(x => x.Url == url).ToListAsync();
            if (shortenedUrls.Any())
            {
                return true;
            }
            return false;
        }

        public async Task<ShortenedUrl?> GetUrlFromToken(string token)
        {
            var shortenedUrl = await _context.ShortenedUrls.FirstOrDefaultAsync(item => item.Token == token);

            if (shortenedUrl == null) return null;

            shortenedUrl.NumberOfVisits += 1;
            shortenedUrl.LastAccessedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return shortenedUrl;
        }

        /// <summary>
        /// Creates a shortened url if the Url is not already shortened. If the url is already shortened
        /// then retreive the already shortened url.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>ShortenedUrl</returns>
        /// <exception cref="FormatException"></exception>
        public async Task<ShortenedUrl> CreateShortenedUrl(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                throw new FormatException();
            }

            if (await IsUrlAlreadyShortened(url)) return await GetShortenedUrlFromUrl(url);

           
            var shortenedUrl = new ShortenedUrl()
            {
                Url = url,
                Token = GenerateTokenString()
            };

            while(_context.ShortenedUrls.Where(x => x.Token == shortenedUrl.Token).Any())
            {
                shortenedUrl.Token = GenerateTokenString();
            }

            _context.ShortenedUrls.Add(shortenedUrl);
            await _context.SaveChangesAsync();

            return shortenedUrl;
            
        }

        /// <summary>
        /// Finds a ShortenedUrl from a Url. Used if a url has already been shortened.
        /// </summary>
        /// <param name="url"></param>
        /// <returns>ShortenedUrl</returns>
        private async Task<ShortenedUrl?> GetShortenedUrlFromUrl(string url)
        {
            var shortenedUrl = await _context.ShortenedUrls.Where(x => x.Url == url).ToListAsync();

            return shortenedUrl.FirstOrDefault();
        }

        /// <summary>
        /// Generate a new Token for the current class
        /// This needs to be called multiple times in case of clash
        /// </summary>
        private string GenerateTokenString()
        {
            var random = new Random();
            var chars = Enumerable.Range(0, random.Next(3, 7))
            .Select(x => Constants.safeCharacters[random.Next(0, Constants.safeCharacters.Length)]);
            return new string(chars.ToArray());
        }

        
        
    }
}

