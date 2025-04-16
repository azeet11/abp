using System;
using System.ComponentModel.DataAnnotations;

namespace TaskTimeTracker.Entities
{
    public class OperationWorkflowConfiguration
    {
        [Key]
        public Guid Id { get; set; }
        // Add other properties as needed
    }
}