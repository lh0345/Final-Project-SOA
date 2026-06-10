namespace TaskFlow.Application.DTOs;

public class AuthResponseDto
{
    /// <summary>
    /// Gets or sets the JWT token issued upon successful authentication.
    /// </summary>
    public string Token { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the email address of the authenticated user.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the username of the authenticated user.
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the roles assigned to the authenticated user.
    /// </summary>
    public IList<string> Roles { get; set; } = new List<string>();
}
