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

[Authorize(TaskTimeTrackerPermissions.TimeTracking.Default)]
public class TimeAppService : ApplicationService, ITransientDependency
{
    private readonly IRepository<Time, Guid> _timeRepository;
    private readonly IGuidGenerator _guidGenerator;

    public TimeAppService(IRepository<Time, Guid> timeRepository, IGuidGenerator guidGenerator)
    {
        _timeRepository = timeRepository;
        _guidGenerator = guidGenerator;
    }

    [Authorize(TaskTimeTrackerPermissions.TimeTracking.Create)]
    [HttpPost("api/times")]
    public async Task<TimeDto> CreateAsync(TimeDto time)
    {
        try
        {
            var newTime = new Time
            {
                Id = _guidGenerator.Create(),
                Date = time.Date,
                Hours = time.Hours,
                Notes = time.Notes,
                TasksId = time.TaskId,
                UserId = time.UserId
            };

            await _timeRepository.InsertAsync(newTime);

            return new TimeDto
            {
                Id = newTime.Id,
                Date = newTime.Date,
                Hours = newTime.Hours,
                Notes = newTime.Notes,
                TaskId = newTime.TasksId,
                UserId = newTime.UserId
            };
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while creating the time entry.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.TimeTracking.Default)]
    [HttpGet("api/times")]
    public async Task<List<TimeDto>> GetListAsync([FromQuery] PagedAndFilteredResultRequestDto input)
    {

        try
        {
            var timeQueryable = await _timeRepository.GetQueryableAsync();

            // Apply general filter (Notes)
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                timeQueryable = timeQueryable.Where(t =>
                    t.Notes.Contains(input.Filter));
            }

            // Filter by ProjectId (via Tasks)
            if (input.ProjectId.HasValue)
            {
                timeQueryable = timeQueryable.Where(t => t.Tasks.ProjectId == input.ProjectId.Value);
            }

            // Filter by UserId
            if (input.UserId.HasValue)
            {
                timeQueryable = timeQueryable.Where(t => t.UserId == input.UserId.Value);
            }

            // Filter by Status (if applicable)
            if (!string.IsNullOrWhiteSpace(input.Status))
            {
                timeQueryable = timeQueryable.Where(t => t.Tasks.Status.ToString() == input.Status);
            }

            // Apply pagination
            var times = await timeQueryable
                .OrderBy(t => t.Date) // Optional: Order by Date
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .Select(t => new TimeDto
                {
                    Id = t.Id,
                    Date = t.Date,
                    Hours = t.Hours,
                    Notes = t.Notes,
                    TaskId = t.TasksId,
                    UserId = t.UserId
                })
                .ToListAsync();

            return times;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the time entries.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.TimeTracking.Default)]
    [HttpGet("api/times/{id}")]
    public async Task<TimeDto> GetAsync(Guid id)
    {
        try
        {
            var time = (await _timeRepository.GetQueryableAsync()).Where(t => t.Id == id).Select(x => new TimeDto
            {
                Id = x.Id,
                Date = x.Date,
                Hours = x.Hours,
                Notes = x.Notes,
                TaskId = x.TasksId,
                UserId = x.UserId
            }).FirstOrDefault();

            if (time == null)
            {
                throw new UserFriendlyException("Time entry not found.");
            }

            return time;
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the time entry.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.TimeTracking.Update)]
    [HttpPut("api/times/{id}")]
    public async Task<TimeDto> UpdateAsync(Guid id, TimeDto time)
    {
        try
        {
            var existingTime = await _timeRepository.GetAsync(id);
            existingTime.Date = time.Date;
            existingTime.Hours = time.Hours;
            existingTime.Notes = time.Notes;
            existingTime.TasksId = time.TaskId;
            existingTime.UserId = time.UserId;

            await _timeRepository.UpdateAsync(existingTime);

            return new TimeDto
            {
                Id = existingTime.Id,
                Date = existingTime.Date,
                Hours = existingTime.Hours,
                Notes = existingTime.Notes,
                TaskId = existingTime.TasksId,
                UserId = existingTime.UserId
            };
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while updating the time entry.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.TimeTracking.Delete)]
    [HttpDelete("api/times/{id}")]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var existingTime = await _timeRepository.GetAsync(id);
            if (existingTime == null)
            {
                throw new UserFriendlyException("Time entry not found.");
            }
            await _timeRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while deleting the time entry.");
        }
    }
}
