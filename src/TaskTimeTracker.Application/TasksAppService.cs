using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTimeTracker.DTOs;
using TaskTimeTracker.Entities;
using TaskTimeTracker.Permissions;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace TaskTimeTracker;

[Authorize(TaskTimeTrackerPermissions.Tasks.Default)]
public class TasksAppService : ApplicationService, ITransientDependency
{
    private readonly IRepository<Tasks, Guid> _taskRepository;
    private readonly IGuidGenerator _guidGenerator;

    public TasksAppService(IRepository<Tasks, Guid> taskRepository, IGuidGenerator guidGenerator)
    {
        _taskRepository = taskRepository;
        _guidGenerator = guidGenerator;
    }

    [Authorize(TaskTimeTrackerPermissions.Tasks.Create)]
    [HttpPost("api/tasks")]
    public async Task<TaskDto> CreateAsync(TaskDto task)
    {
        try
        {
            var newTask = new Tasks
            {
                Id = _guidGenerator.Create(),
                Title = task.Title,
                Description = task.Description,
                ProjectId = task.ProjectId,
                UserId = task.UserId,
                DueDate = task.DueDate,
                Priority = task.Priority,
                Status = task.Status
            };

            await _taskRepository.InsertAsync(newTask);

            return new TaskDto
            {
                Id = newTask.Id,
                Title = newTask.Title,
                Description = newTask.Description,
                ProjectId = newTask.ProjectId,
                UserId = newTask.UserId,
                DueDate = newTask.DueDate,
                Priority = newTask.Priority,
                Status = newTask.Status
            };
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while creating the task.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.Tasks.Default)]
    [HttpGet("api/tasks")]
    public async Task<List<TaskDto>> GetListAsync([FromQuery] PagedAndFilteredResultRequestDto input)
    {
        try
        {
            var taskQueryable = await _taskRepository.GetQueryableAsync();

            // Apply general filter (Title or Description)
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                taskQueryable = taskQueryable.Where(t =>
                    t.Title.Contains(input.Filter) ||
                    t.Description.Contains(input.Filter));
            }

            // Filter by ProjectId
            if (input.ProjectId.HasValue)
            {
                taskQueryable = taskQueryable.Where(t => t.ProjectId == input.ProjectId.Value);
            }

            // Filter by UserId
            if (input.UserId.HasValue)
            {
                taskQueryable = taskQueryable.Where(t => t.UserId == input.UserId.Value);
            }

            // Filter by Status
            if (input.TaskStatus.HasValue)
            {
                taskQueryable = taskQueryable.Where(t => t.Status == input.TaskStatus.Value);
            }

            // Apply pagination
            var tasks = await taskQueryable
                .OrderBy(t => t.Title) // Optional: Order by Title
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    ProjectId = t.ProjectId,
                    UserId = t.UserId,
                    DueDate = t.DueDate,
                    Priority = t.Priority,
                    Status = t.Status
                })
                .ToListAsync();

            return tasks;
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the tasks.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.Tasks.Default)]
    [HttpGet("api/tasks/{id}")]
    public async Task<TaskDto> GetAsync(Guid id)
    {
        try
        {
            var task = await (await _taskRepository.GetQueryableAsync()).Where(t => t.Id == id).Select(x => new TaskDto
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                ProjectId = x.ProjectId,
                UserId = x.UserId,
                DueDate = x.DueDate,
                Priority = x.Priority,
                Status = x.Status
            }).FirstOrDefaultAsync();

            if (task == null)
            {
                throw new UserFriendlyException("Task not found.");
            }
            return task;
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the task.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.Tasks.Update)]
    [HttpPut("api/tasks/{id}")]
    public async Task<TaskDto> UpdateAsync(Guid id, TaskDto task)
    {
        try
        {
            var existingTask = await _taskRepository.GetAsync(id);
            existingTask.Title = task.Title;
            existingTask.Description = task.Description;
            existingTask.ProjectId = task.ProjectId;
            existingTask.UserId = task.UserId;
            existingTask.DueDate = task.DueDate;
            existingTask.Priority = task.Priority; 
            existingTask.Status = task.Status; 

            await _taskRepository.UpdateAsync(existingTask);

            return new TaskDto
            {
                Id = existingTask.Id,
                Title = existingTask.Title,
                Description = existingTask.Description,
                ProjectId = existingTask.ProjectId,
                UserId = existingTask.UserId,
                DueDate = existingTask.DueDate,
                Priority = existingTask.Priority, 
                Status = existingTask.Status 
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while updating the task.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.Tasks.Delete)]
    [HttpDelete("api/tasks/{id}")]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var task = await _taskRepository.GetAsync(id);
            if (task == null)
            {
                throw new UserFriendlyException("Task not found.");
            }
            await _taskRepository.DeleteAsync(id);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while deleting the task.");
        }
    }
}
