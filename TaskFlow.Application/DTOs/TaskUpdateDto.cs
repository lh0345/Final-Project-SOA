namespace TaskFlow.Application.DTOs;

public class TaskUpdateDto
{
    /// <summary>
    /// Gets or sets the updated title of the task.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the updated description of the task.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// Gets or sets whether the task has been completed.
    /// </summary>
    public bool IsCompleted { get; set; }
}
