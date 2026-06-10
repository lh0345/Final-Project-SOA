using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers;

/// <summary>
/// Handles user authentication operations such as registration and login.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// Registers a new user account.
    /// </summary>
    /// <param name="request">The registration details including username, email, and password.</param>
    /// <returns>An authentication response containing the JWT token and user info.</returns>
    /// <response code="200">Returns the authentication result with token.</response>
    /// <response code="400">If the request data is invalid or the user already exists.</response>
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequest request)
    {
        var response = await _authService.RegisterAsync(request);
        return Ok(response);
    }

    /// <summary>
    /// Authenticates an existing user and returns a JWT token.
    /// </summary>
    /// <param name="request">The login credentials (email and password).</param>
    /// <returns>An authentication response containing the JWT token and user info.</returns>
    /// <response code="200">Returns the authentication result with token.</response>
    /// <response code="401">If the credentials are invalid.</response>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);
        return Ok(response);
    }
}
