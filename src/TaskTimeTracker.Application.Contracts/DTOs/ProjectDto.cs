using System;
using System.ComponentModel.DataAnnotations;

namespace TaskTimeTracker.DTOs;

public class ProjectDto
{
    public Guid Id { get; set; }

    [Required]
    [StringLength(256)]
    public string Name { get; set; }

    public string Description { get; set; }

    [Required]
    public Guid UserId { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Required]
    public string Status { get; set; }
}

