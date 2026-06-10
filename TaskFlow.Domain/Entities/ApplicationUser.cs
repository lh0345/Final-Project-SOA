using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Domain.Entities;

/// <summary>
/// Represents an application user extending ASP.NET Core Identity.
/// Navigation property links users to their owned tasks.
/// </summary>
public class ApplicationUser : IdentityUser
{
    /// <summary>Collection of tasks owned by this user.</summary>
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
