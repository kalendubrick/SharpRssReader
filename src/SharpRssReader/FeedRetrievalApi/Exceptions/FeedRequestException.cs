namespace FeedRetrievalApi.Exceptions;

public class FeedRequestException : Exception
{
    public FeedRequestException()
    {
    }

    public FeedRequestException(string message)
        : base(message)
    {
    }

    public FeedRequestException(string message, Exception inner)
        : base(message, inner)
    {
    }
}