namespace SharpRssReader.Models;
public class SharpFeedLink
{
    public Uri BaseUri { get; set; }

    public long Length { get; set; }

    public string MediaType { get; set; }

    public string RelationshipType { get; set; }

    public string Title { get; set; }

    public Uri Uri { get; set; }
}
