using Microsoft.AspNetCore.Identity;

namespace TaskFlow.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
}
