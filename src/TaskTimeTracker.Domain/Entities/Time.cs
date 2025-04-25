using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.Identity;

namespace TaskTimeTracker.Entities;

public class Time : AuditedAggregateRoot<Guid>
{
    [Key]
    public Guid Id { get; set; }

    public DateTime Date { get; set; }

    public double Hours { get; set; }

    public string Notes { get; set; }

    public Guid TasksId { get; set; }

    [ForeignKey("TasksId")]
    public virtual Tasks Tasks { get; set; }

    public Guid UserId { get; set; }

    [ForeignKey("UserId")]
    public virtual IdentityUser User { get; set; }
}
