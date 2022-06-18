using System;
using UrlShortener.Models;

namespace UrlShortener.Services
{
	public interface IUrlShortenerService
	{
        bool AnyShortenedUrls();
        Task<ShortenedUrl?> GetUrlFromToken(string token);
        Task<ShortenedUrl> CreateShortenedUrl(string url);
    }
}

