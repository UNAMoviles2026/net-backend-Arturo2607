namespace reservations_api.Exceptions;

public sealed class ConflictDomainException : Exception
{
    public ConflictDomainException(string message)
        : base(message)
    {
    }
}
