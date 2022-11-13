using FeedRetrievalApi.Tests.Models;

namespace FeedRetrievalApi.Tests;

public class FeedRetrievalApiTests
{
    [Fact]
    public async Task Api_should_return_200_on_valid_feed()
    {
        // Arrange

        await using var application = new FeedRetrievalApiApplication();
        var client = application.CreateClient();

        // Act
        var response = await client.GetAsync("feed?feedUrl=https%3A%2F%2Fcommunity.ricksteves.com%2Ftravel-forum%2Fgermany.atom");

        // Assert
        Assert.Equal(200, (int)response.StatusCode);
    }
}
