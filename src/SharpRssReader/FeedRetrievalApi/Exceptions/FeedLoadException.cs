namespace FeedRetrievalApi.Exceptions;

public class FeedLoadException : Exception
{
    public FeedLoadException()
    {
    }

    public FeedLoadException(string message)
        : base(message)
    {
    }

    public FeedLoadException(string message, Exception inner)
        : base(message, inner)
    {
    }
}