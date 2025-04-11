using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.Students;

public class Student : AuditedAggregateRoot<Guid>
{
    public Student(Guid id, string name, string email) : base(id)
    {
        Name = name;
        Email = email;
    }

    [Required]
    [StringLength(128, MinimumLength = 1)]
    public string Name { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; }
}

