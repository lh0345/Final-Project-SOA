using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

/// <summary>
/// Defines the contract for authentication operations including user registration and login.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided registration details.
    /// </summary>
    /// <param name="request">The registration request containing email, username, and password.</param>
    /// <returns>An <see cref="AuthResponseDto"/> containing the JWT token and user information.</returns>
    Task<AuthResponseDto> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// Authenticates a user with the provided credentials.
    /// </summary>
    /// <param name="request">The login request containing email and password.</param>
    /// <returns>An <see cref="AuthResponseDto"/> containing the JWT token and user information.</returns>
    Task<AuthResponseDto> LoginAsync(LoginRequest request);
}
