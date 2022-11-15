using System.Collections.ObjectModel;

namespace SharpRssReader.Models;
public class SharpFeed
{
    public Collection<SharpFeedPerson> Authors { get; init; } = new Collection<SharpFeedPerson>();

    public Uri BaseUri { get; set; }

    public Collection<SharpFeedCategory> Categories { get; init; } = new Collection<SharpFeedCategory>();

    public Collection<SharpFeedPerson> Contributors { get; init; } = new Collection<SharpFeedPerson>();

    public string? Copyright { get; set; }

    public string? Description { get; set; }

    public string? Id { get; set; }

    public Uri? ImageUrl { get; set; }

    public Collection<SharpFeedItem> Items { get; init; } = new Collection<SharpFeedItem>();

    public DateTimeOffset LastUpdatedTime { get; set; }

    public Collection<SharpFeedLink> Links { get; init; } = new Collection<SharpFeedLink>();

    public string? Title { get; set; }
}
