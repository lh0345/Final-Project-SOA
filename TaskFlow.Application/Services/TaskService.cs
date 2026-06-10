using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Services;

/// <summary>
/// Implements task CRUD operations with user-scoped access control.
/// Delegates all persistence to ITaskRepository, keeping business logic and data access decoupled.
/// </summary>
public class TaskService : ITaskService
{
    private readonly ITaskRepository _repository;

    public TaskService(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskDto> CreateTaskAsync(string userId, TaskCreateDto dto)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            UserId = userId
        };

        var created = await _repository.AddAsync(task);
        return MapToDto(created);
    }

    public async Task<IEnumerable<TaskDto>> GetUserTasksAsync(string userId)
    {
        var tasks = await _repository.GetByUserIdAsync(userId);
        return tasks.Select(MapToDto);
    }

    public async Task<TaskDto?> GetTaskByIdAsync(string userId, Guid taskId)
    {
        var task = await _repository.GetByIdAndUserIdAsync(taskId, userId);
        return task is null ? null : MapToDto(task);
    }

    public async Task<TaskDto?> UpdateTaskAsync(string userId, Guid taskId, TaskUpdateDto dto)
    {
        var task = await _repository.GetByIdAndUserIdAsync(taskId, userId);
        if (task is null)
            return null;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task);
        return MapToDto(task);
    }

    public async Task<bool> DeleteTaskAsync(string userId, Guid taskId)
    {
        var task = await _repository.GetByIdAndUserIdAsync(taskId, userId);
        if (task is null)
            return false;

        await _repository.DeleteAsync(task);
        return true;
    }

    public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
    {
        var tasks = await _repository.GetAllWithUserAsync();
        return tasks.Select(MapToDto);
    }

    public async Task<TaskDto?> GetAnyTaskByIdAsync(Guid taskId)
    {
        var task = await _repository.GetByIdWithUserAsync(taskId);
        return task is null ? null : MapToDto(task);
    }

    public async Task<TaskDto?> UpdateAnyTaskAsync(Guid taskId, TaskUpdateDto dto)
    {
        var task = await _repository.GetByIdAsync(taskId);
        if (task is null)
            return null;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(task);
        return MapToDto(task);
    }

    public async Task<bool> DeleteAnyTaskAsync(Guid taskId)
    {
        var task = await _repository.GetByIdAsync(taskId);
        if (task is null)
            return false;

        await _repository.DeleteAsync(task);
        return true;
    }

    private static TaskDto MapToDto(TaskItem task)
    {
        return new TaskDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt,
            UserId = task.UserId,
            UserName = task.User?.UserName
        };
    }
}
