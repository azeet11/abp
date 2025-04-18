using System;

namespace TaskTimeTracker.DTOs
{
    public class TimeDto
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public double Hours { get; set; }
        public string Notes { get; set; }
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
    }
}
