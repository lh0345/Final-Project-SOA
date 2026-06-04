using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.API.Controllers;

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

    [HttpPost]
    public async Task<ActionResult<TaskDto>> Create([FromBody] TaskCreateDto dto)
    {
        var task = await _taskService.CreateTaskAsync(UserId, dto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskDto>>> GetAll()
    {
        var tasks = await _taskService.GetUserTasksAsync(UserId);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskDto>> GetById(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(UserId, id);
        if (task is null)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return Ok(task);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskDto>> Update(Guid id, [FromBody] TaskUpdateDto dto)
    {
        var task = await _taskService.UpdateTaskAsync(UserId, id, dto);
        if (task is null)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return Ok(task);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _taskService.DeleteTaskAsync(UserId, id);
        if (!deleted)
            return NotFound(new ErrorResponse { Message = "Task not found.", StatusCode = 404 });
        return NoContent();
    }
}
