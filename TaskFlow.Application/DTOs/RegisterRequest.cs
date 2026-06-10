namespace TaskFlow.Application.DTOs;

public class RegisterRequest
{
    /// <summary>
    /// Gets or sets the email address of the user registering.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the username of the user registering.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the password for the new account.
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
