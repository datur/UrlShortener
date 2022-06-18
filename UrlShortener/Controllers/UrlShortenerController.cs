using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Models;
using UrlShortener.Services;

namespace UrlShortener.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrlShortenerController : ControllerBase
    {
        private readonly IUrlShortenerService _urlShortener;

        public UrlShortenerController(IUrlShortenerService urlShortener)
        {
            _urlShortener = urlShortener;
        }

        // GET: api/UrlShortener/5
        [HttpGet("{token}")]
        public async Task<ActionResult<ShortenedUrl>> GetShortenedUrl(string token)
        {
            if (!_urlShortener.AnyShortenedUrls())
            {
                  return NotFound();
            }

            var shortenedUrl = await _urlShortener.GetUrlFromToken(token);

            if (shortenedUrl == null)
            {
                return NotFound();
            }

            return shortenedUrl;
        }

        //CreateShortUrl
        [HttpPost]
        public async Task<ActionResult<ShortenedUrl>> PostShortenedUrl([FromBody] string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest();
            }
            try
            {
                var result = await _urlShortener.CreateShortenedUrl(url);

                return CreatedAtAction(nameof(GetShortenedUrl), new { Token = result.Token }, result);
            }
            catch (FormatException)
            {
                return BadRequest();
            }
        }
                
    }
}
