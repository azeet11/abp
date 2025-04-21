using System;

namespace TaskTimeTracker.DTOs;

public class ReportFilterDto
{
    public Guid? ProjectId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? TaskId { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}