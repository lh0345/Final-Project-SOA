namespace TaskFlow.Application.DTOs;

public class TaskDto
{
    /// <summary>
    /// Gets or sets the unique identifier of the task.
    /// </summary>
    public Guid Id { get; set; }
    /// <summary>
    /// Gets or sets the title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the description of the task.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Gets or sets whether the task is completed.
    /// </summary>
    public bool IsCompleted { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the task was created.
    /// </summary>
    public DateTime CreatedAt { get; set; }
    /// <summary>
    /// Gets or sets the date and time when the task was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
    /// <summary>
    /// Gets or sets the ID of the user who owns the task.
    /// </summary>
    public string UserId { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the username of the user who owns the task.
    /// </summary>
    public string? UserName { get; set; }
}
