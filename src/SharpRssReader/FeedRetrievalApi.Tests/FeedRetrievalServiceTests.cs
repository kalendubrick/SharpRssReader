using Moq;
using Moq.Protected;
using System.Net;

namespace FeedRetrievalApi.Tests;

public class FeedRetrievalServiceTests
{
    [Fact]
    public void Test1()
    {
        // Arrange
        var mockFactory = new Mock<IHttpClientFactory>();

        var mockHttpMessageHandler = new Mock<HttpMessageHandler>();
        mockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>("GetAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(""),
            });

        // Act

        // Assert
    }
}