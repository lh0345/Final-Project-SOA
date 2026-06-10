using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers;

/// <summary>
/// Manages task CRUD operations for the authenticated user.
/// All endpoints require a valid JWT token.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private string UserId => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

    /// <summary>
    /// Creates a new task for the authenticated user.
    /// </summary>
    /// <param name="dto">The task creation data.</param>
    /// <returns>The newly created task.</returns>
    /// <response code="201">Returns the created task.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create([FromBody] TaskCreateDto dto)
    {
        var task = await _taskService.CreateTaskAsync(UserId, dto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    /// <summary>
    /// Retrieves all tasks belonging to the authenticated user.
    /// </summary>
    /// <returns>A list of tasks for the current user.</returns>
    /// <response code="200">Returns the list of tasks.</response>
    /// <response code="401">If the user is not authenticated.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
    {
        var tasks = await _taskService.GetUserTasksAsync(UserId);
        return Ok(tasks);
    }

    /// <summary>
    /// Retrieves a specific task by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The unique identifier of the task.</param>
    /// <returns>The requested task.</returns>
    /// <response code="200">Returns the requested task.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="404">If the task is not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetById(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(UserId, id);
        if (task is null)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return Ok(task);
    }

    /// <summary>
    /// Updates an existing task for the authenticated user.
    /// </summary>
    /// <param name="id">The unique identifier of the task to update.</param>
    /// <param name="dto">The updated task data.</param>
    /// <returns>The updated task.</returns>
    /// <response code="200">Returns the updated task.</response>
    /// <response code="400">If the request data is invalid.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="404">If the task is not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDto>> Update(Guid id, [FromBody] TaskUpdateDto dto)
    {
        var task = await _taskService.UpdateTaskAsync(UserId, id, dto);
        if (task is null)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return Ok(task);
    }

    /// <summary>
    /// Deletes a task by its ID for the authenticated user.
    /// </summary>
    /// <param name="id">The unique identifier of the task to delete.</param>
    /// <response code="204">If the task was successfully deleted.</response>
    /// <response code="401">If the user is not authenticated.</response>
    /// <response code="404">If the task is not found.</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _taskService.DeleteTaskAsync(UserId, id);
        if (!deleted)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return NoContent();
    }
}
