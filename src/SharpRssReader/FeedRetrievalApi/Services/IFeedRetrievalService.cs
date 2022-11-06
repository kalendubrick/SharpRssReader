using System.ServiceModel.Syndication;

namespace FeedRetrievalApi.Services
{
    public interface IFeedRetrievalService
    {
        Task<SyndicationFeed> ReadFeedAsync(string feedUrl);
    }
}