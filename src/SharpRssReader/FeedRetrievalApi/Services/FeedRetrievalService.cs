using System.ServiceModel.Syndication;
using System.Xml;

namespace FeedRetrievalApi.Services;

public class FeedRetrievalService : IFeedRetrievalService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<FeedRetrievalService> _logger;

    public FeedRetrievalService(
        IHttpClientFactory httpClientFactory,
        ILogger<FeedRetrievalService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<SyndicationFeed> ReadFeedAsync(string feedUrl)
    {
        using (_logger.BeginScope(nameof(ReadFeedAsync)))
        {
            _logger.LogTrace(
                "Creating HttpClient");

            var client = _httpClientFactory.CreateClient();

            _logger.LogTrace(
                "Created HttpClient: {Client}", client);

            using var feedResponse = await client.GetAsync(feedUrl);

            _logger.LogTrace(
                    "Feed request returned {StatusCode}: {Reason}",
                    feedResponse.StatusCode,
                    feedResponse.ReasonPhrase);

            if (!feedResponse.IsSuccessStatusCode)
            {
                _logger.LogDebug("Feed request failed, returning empty feed");

                return new SyndicationFeed();
            }

            _logger.LogTrace(
                "Creating stream from response");

            using var feedStream = await feedResponse.Content.ReadAsStreamAsync();

            _logger.LogTrace(
                "Created stream from response: {Stream}", feedStream);

            if (feedStream is null)
            {
                _logger.LogDebug("Feed is empty");

                return new SyndicationFeed();
            }

            _logger.LogTrace(
                "Creating XmlReader from stream");

            using var reader = XmlReader.Create(feedStream);

            _logger.LogTrace(
                "Created XmlReader from feed stream: {Reader}", reader);

            try
            {
                _logger.LogTrace(
                    "Loading feed from reader");

                var feed = SyndicationFeed.Load(reader);

                _logger.LogTrace(
                    "Loaded feed from reader: {Feed}", feed);

                return feed;
            }
            catch (XmlException e)
            {
                _logger.LogError(
                    "Error loading feed: {Exception}",
                    e);

                throw;
            }
        }   
    }
}
