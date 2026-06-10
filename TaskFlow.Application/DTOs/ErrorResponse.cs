namespace TaskFlow.Application.DTOs;

public class ErrorResponse
{
    /// <summary>
    /// Gets or sets the error message describing what went wrong.
    /// </summary>
    public string Message { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the HTTP status code associated with the error.
    /// </summary>
    public int StatusCode { get; set; }
    /// <summary>
    /// Gets or sets the collection of detailed error messages.
    /// </summary>
    public IEnumerable<string>? Errors { get; set; }
}
