using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;

/// <summary>
/// Provides authentication services including user registration, login, and JWT token generation.
/// </summary>
public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _configuration = configuration;
    }

    /// <summary>
    /// Registers a new user, assigns the "User" role, and returns an authentication response with a JWT token.
    /// </summary>
    /// <param name="request">The registration details.</param>
    /// <returns>An <see cref="AuthResponseDto"/> with the generated token and user data.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the email already exists or identity creation fails.</exception>
    public async Task<AuthResponseDto> RegisterAsync(RegisterRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
            throw new InvalidOperationException("A user with this email already exists.");

        var user = new ApplicationUser
        {
            UserName = request.UserName,
            Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            throw new InvalidOperationException(string.Join("; ", errors));
        }

        await _userManager.AddToRoleAsync(user, "User");

        return await GenerateAuthResponse(user);
    }

    /// <summary>
    /// Authenticates a user by email and password and returns an authentication response with a JWT token.
    /// </summary>
    /// <param name="request">The login credentials.</param>
    /// <returns>An <see cref="AuthResponseDto"/> with the generated token and user data.</returns>
    /// <exception cref="UnauthorizedAccessException">Thrown when the email or password is invalid.</exception>
    public async Task<AuthResponseDto> LoginAsync(LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            throw new UnauthorizedAccessException("Invalid email or password.");

        var validPassword = await _userManager.CheckPasswordAsync(user, request.Password);
        if (!validPassword)
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await GenerateAuthResponse(user);
    }

    /// <summary>
    /// Generates a JWT token and constructs an <see cref="AuthResponseDto"/> for the specified user.
    /// </summary>
    /// <param name="user">The authenticated user.</param>
    /// <returns>An <see cref="AuthResponseDto"/> containing the token, email, username, and roles.</returns>
    private async Task<AuthResponseDto> GenerateAuthResponse(ApplicationUser user)
    {
        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? string.Empty),
            new(ClaimTypes.Name, user.UserName ?? string.Empty),
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(24),
            signingCredentials: credentials
        );

        return new AuthResponseDto
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = user.Email ?? string.Empty,
            UserName = user.UserName ?? string.Empty,
            Roles = roles
        };
    }
}
