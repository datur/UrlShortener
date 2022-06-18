namespace UrlShortener.Tests;

public class UrlShortenerServiceFixture : IDisposable
{
    public ShortenedUrlContext _context;

    public UrlShortenerService _service;

    public UrlShortenerServiceFixture()
    {
        _context = new ShortenedUrlContext(new DbContextOptionsBuilder<ShortenedUrlContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

        _service = new UrlShortenerService(_context);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}


