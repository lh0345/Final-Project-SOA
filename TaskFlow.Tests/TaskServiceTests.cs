using Microsoft.EntityFrameworkCore;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Services;
using TaskFlow.Domain;
using TaskFlow.Domain.Entities;
using FluentAssertions;

namespace TaskFlow.Tests;

public class TaskServiceTests
{
    private readonly TaskFlowDbContext _context;
    private readonly TaskService _taskService;

    public TaskServiceTests()
    {
        var options = new DbContextOptionsBuilder<TaskFlowDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new TaskFlowDbContext(options);
        _taskService = new TaskService(_context);
    }

    [Fact]
    public async Task CreateTaskAsync_ShouldCreateTask()
    {
        var dto = new TaskCreateDto
        {
            Title = "Test Task",
            Description = "Test Description"
        };

        var result = await _taskService.CreateTaskAsync("user1", dto);

        result.Should().NotBeNull();
        result.Title.Should().Be("Test Task");
        result.Description.Should().Be("Test Description");
        result.UserId.Should().Be("user1");
        result.IsCompleted.Should().BeFalse();
    }

    [Fact]
    public async Task GetUserTasksAsync_ShouldReturnOnlyUserTasks()
    {
        await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "User 1 Task" });
        await _taskService.CreateTaskAsync("user2", new TaskCreateDto { Title = "User 2 Task" });

        var result = await _taskService.GetUserTasksAsync("user1");

        result.Should().HaveCount(1);
        result.First().Title.Should().Be("User 1 Task");
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnTask_WhenOwnerRequests()
    {
        var created = await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "My Task" });

        var result = await _taskService.GetTaskByIdAsync("user1", created.Id);

        result.Should().NotBeNull();
        result!.Title.Should().Be("My Task");
    }

    [Fact]
    public async Task GetTaskByIdAsync_ShouldReturnNull_WhenNotOwnerRequests()
    {
        var created = await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "My Task" });

        var result = await _taskService.GetTaskByIdAsync("user2", created.Id);

        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldUpdateTask_WhenOwnerRequests()
    {
        var created = await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "Original" });

        var result = await _taskService.UpdateTaskAsync("user1", created.Id, new TaskUpdateDto
        {
            Title = "Updated",
            IsCompleted = true
        });

        result.Should().NotBeNull();
        result!.Title.Should().Be("Updated");
        result.IsCompleted.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateTaskAsync_ShouldReturnNull_WhenNotOwnerRequests()
    {
        var created = await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "Original" });

        var result = await _taskService.UpdateTaskAsync("user2", created.Id, new TaskUpdateDto { Title = "Updated" });

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldDeleteTask_WhenOwnerRequests()
    {
        var created = await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "To Delete" });

        var result = await _taskService.DeleteTaskAsync("user1", created.Id);

        result.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteTaskAsync_ShouldReturnFalse_WhenNotOwnerRequests()
    {
        var created = await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "Keep" });

        var result = await _taskService.DeleteTaskAsync("user2", created.Id);

        result.Should().BeFalse();
    }

    [Fact]
    public async Task GetAllTasksAsync_ShouldReturnAllTasks()
    {
        SeedUser("user1", "user1@test.com");
        SeedUser("user2", "user2@test.com");

        await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "Task A" });
        await _taskService.CreateTaskAsync("user2", new TaskCreateDto { Title = "Task B" });

        var result = await _taskService.GetAllTasksAsync();

        result.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetAnyTaskByIdAsync_ShouldReturnAnyTask()
    {
        SeedUser("user1", "user1@test.com");

        var created = await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "Secret Task" });

        var result = await _taskService.GetAnyTaskByIdAsync(created.Id);

        result.Should().NotBeNull();
        result!.Title.Should().Be("Secret Task");
    }

    private void SeedUser(string userId, string email)
    {
        if (!_context.Set<ApplicationUser>().Any(u => u.Id == userId))
        {
            var user = new ApplicationUser
            {
                Id = userId,
                UserName = email,
                Email = email,
                NormalizedEmail = email.ToUpper(),
                NormalizedUserName = email.ToUpper()
            };
            _context.Set<ApplicationUser>().Add(user);
            _context.SaveChanges();
        }
    }

    [Fact]
    public async Task UpdateAnyTaskAsync_ShouldUpdateAnyTask()
    {
        var created = await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "Original" });

        var result = await _taskService.UpdateAnyTaskAsync(created.Id, new TaskUpdateDto { Title = "Admin Updated" });

        result.Should().NotBeNull();
        result!.Title.Should().Be("Admin Updated");
    }

    [Fact]
    public async Task DeleteAnyTaskAsync_ShouldDeleteAnyTask()
    {
        var created = await _taskService.CreateTaskAsync("user1", new TaskCreateDto { Title = "Delete Me" });

        var result = await _taskService.DeleteAnyTaskAsync(created.Id);

        result.Should().BeTrue();
    }
}
