using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTimeTracker.DTOs;

namespace TaskTimeTracker.Interfaces;

public interface ITasksAppService
{
    Task<TaskDto> CreateAsync(TaskDto task);
    Task<List<TaskDto>> GetListAsync(PagedAndFilteredResultRequestDto input);
    Task<TaskDto> GetAsync(Guid id);
    Task<TaskDto> UpdateAsync(Guid id, TaskDto task);
    Task DeleteAsync(Guid id);
}