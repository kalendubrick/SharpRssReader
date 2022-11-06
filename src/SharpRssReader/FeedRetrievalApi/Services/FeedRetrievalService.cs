using System.ServiceModel.Syndication;
using System.Xml;

namespace FeedRetrievalApi.Services;

public class FeedRetrievalService : IFeedRetrievalService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public FeedRetrievalService(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<SyndicationFeed> ReadFeedAsync(string feedUrl)
    {
        var client = _httpClientFactory.CreateClient();

        using var feedStream = await client.GetStreamAsync(feedUrl);

        if (feedStream is null)
        {
            return new SyndicationFeed();
        }

        using var reader = XmlReader.Create(feedStream);

        return SyndicationFeed.Load(reader);
    }
}
