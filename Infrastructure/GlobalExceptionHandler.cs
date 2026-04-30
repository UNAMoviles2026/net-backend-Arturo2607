using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using reservations_api.DTOs.Responses;
using reservations_api.Exceptions;

namespace reservations_api.Infrastructure;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, message, logLevel) = Map(exception);

        if (logLevel == LogLevel.Error)
        {
            _logger.LogError(exception, "Unhandled exception: {TraceId}", Activity.Current?.Id ?? httpContext.TraceIdentifier);
        }
        else
        {
            _logger.LogWarning(exception, "Domain rule violation: {Message}", message);
        }

        httpContext.Response.StatusCode = statusCode;
        await httpContext.Response.WriteAsJsonAsync(
            new ErrorResponse { Message = message },
            cancellationToken);

        return true;
    }

    private static (int StatusCode, string Message, LogLevel LogLevel) Map(Exception exception)
    {
        return exception switch
        {
            BadRequestDomainException e => (StatusCodes.Status400BadRequest, e.Message, LogLevel.Warning),
            NotFoundDomainException e => (StatusCodes.Status404NotFound, e.Message, LogLevel.Warning),
            ConflictDomainException e => (StatusCodes.Status409Conflict, e.Message, LogLevel.Warning),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.", LogLevel.Error)
        };
    }
}
