namespace TaskFlow.Application.DTOs;

public class TaskCreateDto
{
    /// <summary>
    /// Gets or sets the title of the task to create.
    /// </summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the optional description of the task to create.
    /// </summary>
    public string? Description { get; set; }
}
