using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace TaskTimeTracker.Entities;

public class Project : AuditedAggregateRoot<Guid>
{
    public Project(Guid id, string name, string description, Guid userId, DateTime? startDate, DateTime? endDate, string status) : base(id)
    {
        Name = name;
        Description = description;
        UserId = userId;
        StartDate = startDate;
        EndDate = endDate;
        Status = status;
    }

    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(256)]
    public string Name { get; set; }

    public string Description { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public IdentityUser User { get; set; } // Use IdentityUser from Volo.Abp.Identity

    public DateTime? StartDate { get; set; } // Added StartDate property
    public DateTime? EndDate { get; set; } // Added EndDate property
    public string Status { get; set; } // Added Status property
}
