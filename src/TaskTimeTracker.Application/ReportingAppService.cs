using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using TaskTimeTracker.DTOs;
using TaskTimeTracker.Entities;
using TaskTimeTracker.Permissions;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace TaskTimeTracker;

[Authorize(TaskTimeTrackerPermissions.Reporting.Default)]
public class ReportingAppService : ApplicationService
{
    private readonly IRepository<Time, Guid> _timeRepository;

    public ReportingAppService(IRepository<Time, Guid> timeRepository)
    {
        _timeRepository = timeRepository;
    }

    [Authorize(TaskTimeTrackerPermissions.Reporting.View)]
    [HttpGet("api/reports")]
    public async Task<List<ReportDto>> GetTotalHoursAsync(ReportFilterDto filter)
    {
        var queryable = await _timeRepository.GetQueryableAsync(); // Get IQueryable<Time>

        var query = queryable
            .WhereIf(filter.ProjectId.HasValue, t => t.Tasks.ProjectId == filter.ProjectId)
            .WhereIf(filter.UserId.HasValue, t => t.UserId == filter.UserId)
            .WhereIf(filter.TaskId.HasValue, t => t.TaskId == filter.TaskId)
            .WhereIf(filter.StartDate.HasValue, t => t.Date >= filter.StartDate)
            .WhereIf(filter.EndDate.HasValue, t => t.Date <= filter.EndDate);

        var result = await query
            .GroupBy(t => new { t.Tasks.ProjectId, t.UserId, t.TaskId })
            .Select(g => new ReportDto
            {
                ProjectId = g.Key.ProjectId,
                UserId = g.Key.UserId,
                TaskId = g.Key.TaskId,
                TotalHours = g.Sum(t => t.Hours)
            })
            .ToListAsync();

        return result;
    }
}
