using System;
using TaskTimeTracker.Enums;

namespace TaskTimeTracker.DTOs;

public class PagedAndFilteredResultRequestDto
{
    public string Filter { get; set; } // Optional general filter (e.g., Title or Description)
    public Guid? ProjectId { get; set; } // Filter by ProjectId
    public Guid? UserId { get; set; } // Filter by UserId
    public TasksStatus? TaskStatus { get; set; } // Filter by Task Status
    public string Status { get; set; } // Filter by Status (string for Project/Time)
    public int SkipCount { get; set; } = 0; // Number of items to skip
    public int MaxResultCount { get; set; } = 10; // Maximum number of items to return
}