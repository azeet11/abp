using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskTimeTracker.DTOs;
using TaskTimeTracker.Entities;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace TaskTimeTracker
{
    public class TasksAppService : ApplicationService, ITransientDependency
    {
        private readonly IRepository<Tasks, Guid> _taskRepository;
        private readonly IGuidGenerator _guidGenerator;

        public TasksAppService(IRepository<Tasks, Guid> taskRepository, IGuidGenerator guidGenerator)
        {
            _taskRepository = taskRepository;
            _guidGenerator = guidGenerator;
        }

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
                    DueDate = (DateTime)task.DueDate,
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

        [HttpGet("api/tasks")]
        public async Task<List<TaskDto>> GetListAsync()
        {
            try
            {
                var taskQueryable = await _taskRepository.GetQueryableAsync();
                var tasks = await taskQueryable.Select(t => new TaskDto
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    ProjectId = t.ProjectId,
                    UserId = t.UserId,
                    DueDate = t.DueDate,
                    Priority = t.Priority,
                    Status = t.Status
                }).ToListAsync();

                if (tasks == null || !tasks.Any())
                {
                    throw new UserFriendlyException("No tasks found.");
                }

                return tasks;
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("An error occurred while retrieving the tasks.");
            }
        }

        [HttpGet("api/tasks/{id}")]
        public async Task<TaskDto> GetAsync(Guid id)
        {
            try
            {
                var task = await _taskRepository.GetAsync(id);
                return new TaskDto
                {
                    Id = task.Id,
                    Title = task.Title,
                    Description = task.Description,
                    ProjectId = task.ProjectId,
                    UserId = task.UserId,
                    DueDate = task.DueDate,
                    Priority = task.Priority, 
                    Status = task.Status 
                };
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("An error occurred while retrieving the task.");
            }
        }

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
                existingTask.DueDate = (DateTime)task.DueDate;
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
            catch (Exception ex)
            {
                throw new UserFriendlyException("An error occurred while updating the task.");
            }
        }

        [HttpDelete("api/tasks/{id}")]
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await _taskRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("An error occurred while deleting the task.");
            }
        }
    }
}
