using System.Net;
using System.Text.Json;
using TaskFlow.Application.DTOs;

namespace TaskFlow.API.Middleware;

/// <summary>
/// Middleware that catches unhandled exceptions during request processing
/// and returns a structured JSON error response with the appropriate HTTP status code.
/// </summary>
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware delegate in the pipeline.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    /// <summary>
    /// Invokes the middleware, wrapping the downstream pipeline in a try-catch block.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    /// <summary>
    /// Writes a JSON error response based on the exception type.
    /// Maps <see cref="UnauthorizedAccessException"/> to 401,
    /// <see cref="InvalidOperationException"/> to 400, and all others to 500.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <param name="exception">The caught exception.</param>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            UnauthorizedAccessException => (HttpStatusCode.Unauthorized, exception.Message),
            InvalidOperationException => (HttpStatusCode.BadRequest, exception.Message),
            _ => (HttpStatusCode.InternalServerError, "An internal server error occurred.")
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new ErrorResponse
        {
            Message = message,
            StatusCode = (int)statusCode
        };

        var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}
