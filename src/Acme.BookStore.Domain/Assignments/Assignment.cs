using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Assignments;

public class Assignment : AuditedAggregateRoot<Guid>
{
    public Assignment(Guid id, string title, string description, DateTime dueDate) : base(id)
    {
        Title = title;
        Description = description;
        DueDate = dueDate;
    }

    [Required]
    [StringLength(256, MinimumLength = 1)]
    public string Title { get; set; }

    [StringLength(1024)]
    public string Description { get; set; }

    [Required]
    public DateTime DueDate { get; set; }
}

