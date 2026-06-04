namespace TaskFlow.Application.DTOs;

public class ErrorResponse
{
    public string Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public IEnumerable<string>? Errors { get; set; }
}
