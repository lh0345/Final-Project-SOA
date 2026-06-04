using TaskFlow.Application.DTOs;

namespace TaskFlow.Application.Interfaces;

public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(string userId, TaskCreateDto dto);
    Task<IEnumerable<TaskDto>> GetUserTasksAsync(string userId);
    Task<TaskDto?> GetTaskByIdAsync(string userId, Guid taskId);
    Task<TaskDto?> UpdateTaskAsync(string userId, Guid taskId, TaskUpdateDto dto);
    Task<bool> DeleteTaskAsync(string userId, Guid taskId);
    Task<IEnumerable<TaskDto>> GetAllTasksAsync();
    Task<TaskDto?> GetAnyTaskByIdAsync(Guid taskId);
    Task<TaskDto?> UpdateAnyTaskAsync(Guid taskId, TaskUpdateDto dto);
    Task<bool> DeleteAnyTaskAsync(Guid taskId);
}
