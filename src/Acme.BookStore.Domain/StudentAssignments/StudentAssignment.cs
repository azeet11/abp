using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities.Auditing;

namespace Acme.BookStore.StudentAssignments;

public class StudentAssignment : AuditedAggregateRoot<Guid>
{
    public StudentAssignment(Guid id, Guid studentId, Guid assignmentId, DateTime submissionDate, string grade) : base(id)
    {
        StudentId = studentId;
        AssignmentId = assignmentId;
        SubmissionDate = submissionDate;
        Grade = grade;
    }

    [Required]
    public Guid StudentId { get; set; }

    [Required]
    public Guid AssignmentId { get; set; }

    [Required]
    public DateTime SubmissionDate { get; set; }

    [StringLength(64)]
    public string Grade { get; set; }
}

