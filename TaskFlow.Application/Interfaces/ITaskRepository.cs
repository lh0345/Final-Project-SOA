using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Interfaces;

/// <summary>
/// Defines data access operations for TaskItem entities.
/// Abstracts persistence logic so the service layer depends on abstractions, not EF Core.
/// </summary>
public interface ITaskRepository
{
    Task<IEnumerable<TaskItem>> GetByUserIdAsync(string userId);
    Task<TaskItem?> GetByIdAndUserIdAsync(Guid taskId, string userId);
    Task<TaskItem> AddAsync(TaskItem task);
    Task UpdateAsync(TaskItem task);
    Task DeleteAsync(TaskItem task);
    Task<IEnumerable<TaskItem>> GetAllWithUserAsync();
    Task<TaskItem?> GetByIdWithUserAsync(Guid taskId);
    Task<TaskItem?> GetByIdAsync(Guid taskId);
}
