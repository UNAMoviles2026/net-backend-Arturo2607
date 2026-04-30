namespace reservations_api.Exceptions;

public sealed class NotFoundDomainException : Exception
{
    public NotFoundDomainException(string message)
        : base(message)
    {
    }
}
