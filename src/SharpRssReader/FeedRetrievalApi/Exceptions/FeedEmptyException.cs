namespace FeedRetrievalApi.Exceptions;

public class FeedEmptyException : Exception
{
    public FeedEmptyException()
    {
    }

    public FeedEmptyException(string message)
        : base(message)
    {
    }

    public FeedEmptyException(string message, Exception inner)
        : base(message, inner)
    {
    }
}