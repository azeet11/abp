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
                TaskId = time.TaskId,
                UserId = time.UserId
            };

            await _timeRepository.InsertAsync(newTime);

            return new TimeDto
            {
                Id = newTime.Id,
                Date = newTime.Date,
                Hours = newTime.Hours,
                Notes = newTime.Notes,
                TaskId = newTime.TaskId,
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
    public async Task<List<TimeDto>> GetListAsync()
    {
        try
        {
            var timeQueryable = await _timeRepository.GetQueryableAsync();
            var times = await timeQueryable.Select(t => new TimeDto
            {
                Id = t.Id,
                Date = t.Date,
                Hours = t.Hours,
                Notes = t.Notes,
                TaskId = t.TaskId,
                UserId = t.UserId
            }).ToListAsync();

            if (times == null || !times.Any())
            {
                throw new UserFriendlyException("No time entries found.");
            }

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
            var time = await _timeRepository.GetAsync(id);
            return new TimeDto
            {
                Id = time.Id,
                Date = time.Date,
                Hours = time.Hours,
                Notes = time.Notes,
                TaskId = time.TaskId,
                UserId = time.UserId
            };
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
            existingTime.TaskId = time.TaskId;
            existingTime.UserId = time.UserId;

            await _timeRepository.UpdateAsync(existingTime);

            return new TimeDto
            {
                Id = existingTime.Id,
                Date = existingTime.Date,
                Hours = existingTime.Hours,
                Notes = existingTime.Notes,
                TaskId = existingTime.TaskId,
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
            await _timeRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while deleting the time entry.");
        }
    }
}
