
namespace UrlShortener.Tests;

public class UrlShortenerServiceTests : IClassFixture<UrlShortenerServiceFixture>
{
    [Fact]
    public void Test_AnyShortenedUrls_NonePresent_ReturnsFalse()
    {
        var fixture = new UrlShortenerServiceFixture();
        
        var anyShortenedUrls = fixture._service.AnyShortenedUrls();
        
        Assert.False(anyShortenedUrls);
    }

    [Fact]
    public void Test_AnyShortenedUrls_Present_ReturnsTrue()
    {
        var fixture = new UrlShortenerServiceFixture();
        fixture._context.ShortenedUrls.Add(new ShortenedUrl { Url = "http://test.com", Token = "1234" });
        fixture._context.SaveChanges();

        var anyShortenedUrls = fixture._service.AnyShortenedUrls();
        Assert.True(anyShortenedUrls);

        fixture.Dispose();
    }

    [Fact]
    public async Task Test_CreateShortenedUrl_UrlCorrectFormat_ReturnsShortenedUrl()
    {
        var fixture = new UrlShortenerServiceFixture();

        var createdShortUrl = await fixture._service.CreateShortenedUrl("http://google.com");

        Assert.NotNull(createdShortUrl);
        Assert.Equal("http://google.com", createdShortUrl.Url);
    }

    [Fact]
    public async Task Test_CreateShortenedUrl_UrlIncorrectFormat_ThrowsFormatException()
    {
        var fixture = new UrlShortenerServiceFixture();

        await Assert.ThrowsAsync<FormatException>(() => fixture._service.CreateShortenedUrl("google.com"));
    }

    [Fact]
    public async Task Test_CreateShortenedUrl_UrlAlreadyShortened_ReturnsCurrentShortenedUrl()
    {
        var fixture = new UrlShortenerServiceFixture();

        fixture._context.ShortenedUrls.Add(new ShortenedUrl { Url = "http://google.com", Token = "1234" });
        fixture._context.SaveChanges();

        var createdShortUrl = await fixture._service.CreateShortenedUrl("http://google.com");

        Assert.Equal("1234", createdShortUrl.Token);
        Assert.Equal(1, fixture._context.ShortenedUrls.Count());
    }

    [Fact]
    public async Task Test_GetUrlFromToken_UrlDoesNotExist_ReturnsNull()
    {
        var fixture = new UrlShortenerServiceFixture();

        var shortenedUrl = await fixture._service.GetUrlFromToken("1234");

        Assert.Null(shortenedUrl);
    }

    [Fact]
    public async Task Test_GetUrlFromToken_UrlExists_ReturnsShortenedUrl()
    {
        var fixture = new UrlShortenerServiceFixture();

        fixture._context.ShortenedUrls.Add(new ShortenedUrl { Url = "http://google.com", Token = "1234" });
        fixture._context.SaveChanges();

        var shortenedUrl = await fixture._service.GetUrlFromToken("1234");

        Assert.NotNull(shortenedUrl);
        Assert.Equal("http://google.com", shortenedUrl.Url);
    }

    
}
