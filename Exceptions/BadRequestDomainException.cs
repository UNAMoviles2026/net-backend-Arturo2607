namespace reservations_api.Exceptions;

public sealed class BadRequestDomainException : Exception
{
    public BadRequestDomainException(string message)
        : base(message)
    {
    }
}
