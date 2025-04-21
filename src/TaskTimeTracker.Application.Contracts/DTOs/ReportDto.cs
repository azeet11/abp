using System;

namespace TaskTimeTracker.DTOs;

public class ReportDto
{
    public Guid? ProjectId { get; set; }
    public Guid? UserId { get; set; }
    public Guid? TaskId { get; set; }
    public double TotalHours { get; set; }
}
