using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers;

/// <summary>
/// Provides administrative task management endpoints restricted to users with the Admin role.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly ITaskService _taskService;

    public AdminController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    /// <summary>
    /// Retrieves all tasks across all users. Admin only.
    /// </summary>
    /// <returns>A list of all tasks in the system.</returns>
    /// <response code="200">Returns all tasks.</response>
    /// <response code="403">If the user is not an admin.</response>
    [HttpGet("tasks")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    /// <summary>
    /// Retrieves any task by its ID regardless of ownership. Admin only.
    /// </summary>
    /// <param name="id">The unique identifier of the task.</param>
    /// <returns>The requested task.</returns>
    /// <response code="200">Returns the requested task.</response>
    /// <response code="403">If the user is not an admin.</response>
    /// <response code="404">If the task is not found.</response>
    [HttpGet("tasks/{id}")]
    public async Task<ActionResult<TaskDto>> GetTaskById(Guid id)
    {
        var task = await _taskService.GetAnyTaskByIdAsync(id);
        if (task is null)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return Ok(task);
    }

    /// <summary>
    /// Updates any task by its ID regardless of ownership. Admin only.
    /// </summary>
    /// <param name="id">The unique identifier of the task to update.</param>
    /// <param name="dto">The updated task data.</param>
    /// <returns>The updated task.</returns>
    /// <response code="200">Returns the updated task.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="403">If the user is not an admin.</response>
    /// <response code="404">If the task is not found.</response>
    [HttpPut("tasks/{id}")]
    public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, [FromBody] TaskUpdateDto dto)
    {
        var task = await _taskService.UpdateAnyTaskAsync(id, dto);
        if (task is null)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return Ok(task);
    }

    /// <summary>
    /// Deletes any task by its ID regardless of ownership. Admin only.
    /// </summary>
    /// <param name="id">The unique identifier of the task to delete.</param>
    /// <response code="204">If the task was successfully deleted.</response>
    /// <response code="403">If the user is not an admin.</response>
    /// <response code="404">If the task is not found.</response>
    [HttpDelete("tasks/{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var deleted = await _taskService.DeleteAnyTaskAsync(id);
        if (!deleted)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return NoContent();
    }
}
