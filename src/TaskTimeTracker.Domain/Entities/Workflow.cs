using System;
using System.ComponentModel.DataAnnotations;

namespace TaskTimeTracker.Entities
{
    public class Workflow
    {
        [Key]
        public Guid Id { get; set; }
        // Add other properties as needed
    }
}