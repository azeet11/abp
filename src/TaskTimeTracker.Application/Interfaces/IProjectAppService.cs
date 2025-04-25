using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using TaskTimeTracker.DTOs;

namespace TaskTimeTracker.Interfaces;

public interface IProjectAppService
{
    Task<ProjectDto> CreateAsync(ProjectDto project);
    Task<List<ProjectDto>> GetListAsync(PagedAndFilteredResultRequestDto input); // Updated method signature
    Task<ProjectDto> GetAsync(Guid id);
    Task<ProjectDto> UpdateAsync(Guid id, ProjectDto project);
    Task DeleteAsync(Guid id);
}

