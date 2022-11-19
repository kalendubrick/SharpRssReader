using System.Collections.ObjectModel;

namespace SharpRssReader.Models;

public class SharpFeedItem
{
    public Collection<SharpFeedPerson> Authors { get; init; } = new Collection<SharpFeedPerson>();

    public Collection<SharpFeedCategory> Categories { get; init; } = new Collection<SharpFeedCategory>();

    public Collection<SharpFeedPerson> Contributors { get; init; } = new Collection<SharpFeedPerson>();

    public string? Copyright { get; set; }

    public string? Id { get; set; }

    public DateTimeOffset LastUpdatedTime { get; set; }

    public Collection<SharpFeedLink> Links { get; init; } = new Collection<SharpFeedLink>();

    public DateTimeOffset PublishDate { get; set; }

    public string? Summary { get; set; }

    public string? Title { get; set; }
}
