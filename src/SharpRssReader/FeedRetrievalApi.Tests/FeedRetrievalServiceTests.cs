using FeedRetrievalApi.Exceptions;
using FeedRetrievalApi.Services;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Moq.Protected;
using System.Net;
using System.ServiceModel.Syndication;

namespace FeedRetrievalApi.Tests;

public class FeedRetrievalServiceTests
{
    const string ValidRssFeed =
    @"<?xml version=""1.0"" encoding=""UTF-8""?>
        <rss version=""2.0"">
           <channel>
              <title>Liftoff News</title>
              <link>http://liftoff.msfc.nasa.gov/</link>
              <description>Liftoff to Space Exploration.</description>
              <language>en-us</language>
              <pubDate>Tue, 10 Jun 2003 04:00:00 GMT</pubDate>
              <lastBuildDate>Tue, 10 Jun 2003 09:41:01 GMT</lastBuildDate>
              <docs>http://blogs.law.harvard.edu/tech/rss</docs>
              <generator>Weblog Editor 2.0</generator>
              <managingEditor>editor@example.com</managingEditor>
              <webMaster>webmaster@example.com</webMaster>
              <item>
                 <title>Star City</title>
                 <link>http://liftoff.msfc.nasa.gov/news/2003/news-starcity.asp</link>
                 <description>How do Americans get ready to work with Russians aboard the International Space Station? They take a crash course in culture, language and protocol at Russia's &lt;a href=""http://howe.iki.rssi.ru/GCTC/gctc_e.htm""&gt;Star City&lt;/a&gt;.</description>
                 <pubDate>Tue, 03 Jun 2003 09:39:21 GMT</pubDate>
                 <guid>http://liftoff.msfc.nasa.gov/2003/06/03.html#item573</guid>
              </item>
              <item>
                 <description>Sky watchers in Europe, Asia, and parts of Alaska and Canada will experience a &lt;a href=""http://science.nasa.gov/headlines/y2003/30may_solareclipse.htm""&gt;partial eclipse of the Sun&lt;/a&gt; on Saturday, May 31st.</description>
                 <pubDate>Fri, 30 May 2003 11:06:42 GMT</pubDate>
                 <guid>http://liftoff.msfc.nasa.gov/2003/05/30.html#item572</guid>
              </item>
              <item>
                 <title>The Engine That Does More</title>
                 <link>http://liftoff.msfc.nasa.gov/news/2003/news-VASIMR.asp</link>
                 <description>Before man travels to Mars, NASA hopes to design new engines that will let us fly through the Solar System more quickly.  The proposed VASIMR engine would do that.</description>
                 <pubDate>Tue, 27 May 2003 08:37:32 GMT</pubDate>
                 <guid>http://liftoff.msfc.nasa.gov/2003/05/27.html#item571</guid>
              </item>
              <item>
                 <title>Astronauts' Dirty Laundry</title>
                 <link>http://liftoff.msfc.nasa.gov/news/2003/news-laundry.asp</link>
                 <description>Compared to earlier spacecraft, the International Space Station has many luxuries, but laundry facilities are not one of them.  Instead, astronauts have other options.</description>
                 <pubDate>Tue, 20 May 2003 08:56:02 GMT</pubDate>
                 <guid>http://liftoff.msfc.nasa.gov/2003/05/20.html#item570</guid>
              </item>
           </channel>
        </rss>
        ";

    [Fact]
    public async Task Valid_feed_content_should_return_feed_object()
    {
        // Arrange
        var mockFactory = new Mock<IHttpClientFactory>();

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(ValidRssFeed)
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        var feedRetrievalService = new FeedRetrievalService(
            mockFactory.Object,
            new NullLogger<FeedRetrievalService>());

        // Act
        var result = await feedRetrievalService.ReadFeedAsync("https://example.com/rss.xml");

        // Assert
        Assert.NotNull(result);
        Assert.IsType<SyndicationFeed>(result);
    }

    [Fact]
    public async Task Empty_feed_should_throw_feed_empty_exception()
    {
        // Arrange
        var mockFactory = new Mock<IHttpClientFactory>();

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = null
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        var feedRetrievalService = new FeedRetrievalService(
            mockFactory.Object,
            new NullLogger<FeedRetrievalService>());

        // Act / Assert
        await Assert.ThrowsAsync<FeedEmptyException>(async () =>
        {
            await feedRetrievalService.ReadFeedAsync("https://example.com/rss.xml");
        });
    }

    [Fact]
    public async Task Malformed_feed_should_throw_feed_load_exception()
    {
        // Arrange
        var mockFactory = new Mock<IHttpClientFactory>();

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("<xml")
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        var feedRetrievalService = new FeedRetrievalService(
            mockFactory.Object,
            new NullLogger<FeedRetrievalService>());

        // Act / Assert
        await Assert.ThrowsAsync<FeedLoadException>(async () =>
        {
            await feedRetrievalService.ReadFeedAsync("https://example.com/rss.xml");
        });
    }

    [Fact]
    public async Task Unsuccessful_status_code_should_throw_feed__request_exception()
    {
        // Arrange
        var mockFactory = new Mock<IHttpClientFactory>();

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = null
            });

        var client = new HttpClient(mockHttpMessageHandler.Object);
        mockFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);

        var feedRetrievalService = new FeedRetrievalService(
            mockFactory.Object,
            new NullLogger<FeedRetrievalService>());

        // Act / Assert
        await Assert.ThrowsAsync<FeedRequestException>(async () =>
        {
            await feedRetrievalService.ReadFeedAsync("https://example.com/rss.xml");
        });
    }
}