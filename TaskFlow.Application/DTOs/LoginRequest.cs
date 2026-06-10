namespace TaskFlow.Application.DTOs;

public class LoginRequest
{
    /// <summary>
    /// Gets or sets the email address of the user logging in.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the password of the user logging in.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
