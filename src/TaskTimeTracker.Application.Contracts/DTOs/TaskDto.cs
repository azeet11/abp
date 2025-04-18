using System;
using TaskTimeTracker.Enums;

namespace TaskTimeTracker.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime? DueDate { get; set; }
        public TasksStatus Status { get; set; }
        public TasksPriority Priority { get; set; }
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
    }
}