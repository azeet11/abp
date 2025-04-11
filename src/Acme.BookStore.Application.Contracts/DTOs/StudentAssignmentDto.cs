using System;

namespace Acme.BookStore.DTOs;

public class StudentAssignmentDto
{
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Guid AssignmentId { get; set; }
    public DateTime SubmissionDate { get; set; }
    public string Grade { get; set; }
}