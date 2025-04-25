using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;
using TaskTimeTracker.Enums;

namespace TaskTimeTracker.Entities;

public class Tasks : AuditedAggregateRoot<Guid>
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(256)]
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTime? DueDate { get; set; }

    public TasksStatus Status { get; set; }

    public TasksPriority Priority { get; set; }

    public Guid ProjectId { get; set; }

    [ForeignKey("ProjectId")]
    public virtual Project Project { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual IdentityUser User { get; set; }
}
