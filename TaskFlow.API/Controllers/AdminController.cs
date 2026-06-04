using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers;

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

    [HttpGet("tasks")]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAllTasks()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("tasks/{id}")]
    public async Task<ActionResult<TaskDto>> GetTaskById(Guid id)
    {
        var task = await _taskService.GetAnyTaskByIdAsync(id);
        if (task is null)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return Ok(task);
    }

    [HttpPut("tasks/{id}")]
    public async Task<ActionResult<TaskDto>> UpdateTask(Guid id, [FromBody] TaskUpdateDto dto)
    {
        var task = await _taskService.UpdateAnyTaskAsync(id, dto);
        if (task is null)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return Ok(task);
    }

    [HttpDelete("tasks/{id}")]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var deleted = await _taskService.DeleteAnyTaskAsync(id);
        if (!deleted)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return NoContent();
    }
}
