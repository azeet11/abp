using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.DTOs;
using Acme.BookStore.Permissions;
using Acme.BookStore.StudentAssignments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;

namespace Acme.BookStore;

[Authorize]
public class StudentAssignmentAppService : ApplicationService
{
    private readonly IRepository<StudentAssignment, Guid> _studentAssignmentRepository;
    private readonly IGuidGenerator _guidGenerator;

    public StudentAssignmentAppService(IRepository<StudentAssignment, Guid> studentAssignmentRepository, IGuidGenerator guidGenerator)
    {
        _studentAssignmentRepository = studentAssignmentRepository;
        _guidGenerator = guidGenerator;
    }

    [Authorize(BookStorePermissions.StudentAssignments.Create)]
    [HttpPost("api/studentassignments")]
    public async Task<StudentAssignmentDto> CreateAsync(StudentAssignmentDto studentAssignmentDto)
    {
        try
        {
            var studentAssignment = new StudentAssignment(
                _guidGenerator.Create(),
                studentAssignmentDto.StudentId,
                studentAssignmentDto.AssignmentId,
                studentAssignmentDto.SubmissionDate,
                studentAssignmentDto.Grade
            );

            await _studentAssignmentRepository.InsertAsync(studentAssignment);
            return new StudentAssignmentDto
            {
                Id = studentAssignment.Id,
                StudentId = studentAssignment.StudentId,
                AssignmentId = studentAssignment.AssignmentId,
                SubmissionDate = studentAssignment.SubmissionDate,
                Grade = studentAssignment.Grade
            };
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while creating the student assignment.");
        }
    }

    [Authorize(BookStorePermissions.StudentAssignments.Delete)]
    [HttpDelete("api/studentassignments/{id}")]
    public async Task<string> DeleteAsync(Guid id)
    {
        try
        {
            var studentAssignment = await _studentAssignmentRepository.GetAsync(id);
            if (studentAssignment == null)
            {
                throw new UserFriendlyException("Student assignment not found.");
            }
            await _studentAssignmentRepository.DeleteAsync(id);
            return await Task.FromResult("Student assignment was deleted successfully.");
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while deleting the student assignment.");
        }
    }

    [Authorize(BookStorePermissions.StudentAssignments.GetList)]
    [HttpGet("api/studentassignments/{id}")]
    public async Task<StudentAssignmentDto> GetAsync(Guid id)
    {
        try
        {
            var studentAssignment = await _studentAssignmentRepository.GetAsync(id);
            if (studentAssignment == null)
            {
                throw new UserFriendlyException("Book not found.");
            }
            return new StudentAssignmentDto
            {
                Id = studentAssignment.Id,
                StudentId = studentAssignment.StudentId,
                AssignmentId = studentAssignment.AssignmentId,
                SubmissionDate = studentAssignment.SubmissionDate,
                Grade = studentAssignment.Grade
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the student assignment.");
        }
    }

    [Authorize(BookStorePermissions.StudentAssignments.Get)]
    [HttpGet("api/studentassignments")]
    public async Task<List<StudentAssignmentDto>> GetListAsync()
    {
        try
        {
            var studentAssignmentQueryable = await _studentAssignmentRepository.GetQueryableAsync();
           
            var studentAssignmentDtos = await studentAssignmentQueryable.Select(x => new StudentAssignmentDto()
            {
                Id = x.Id,
                StudentId = x.StudentId,
                AssignmentId = x.AssignmentId,
                SubmissionDate = x.SubmissionDate,
                Grade = x.Grade
            }).ToListAsync();

            if (studentAssignmentDtos == null)
            {
                throw new UserFriendlyException("Book not found.");
            }
            return studentAssignmentDtos;
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the student assignment list.");
        }
    }

    [Authorize(BookStorePermissions.StudentAssignments.Update)]
    [HttpPut("api/studentassignments/{id}")]
    public async Task UpdateAsync(Guid id, StudentAssignmentDto studentAssignmentDto)
    {
        try
        {
            var studentAssignment = await _studentAssignmentRepository.GetAsync(id);
            if (studentAssignment == null)
            {
                throw new UserFriendlyException("Book not found.");
            }
            studentAssignment.StudentId = studentAssignmentDto.StudentId;
            studentAssignment.AssignmentId = studentAssignmentDto.AssignmentId;
            studentAssignment.SubmissionDate = studentAssignmentDto.SubmissionDate;
            studentAssignment.Grade = studentAssignmentDto.Grade;
            await _studentAssignmentRepository.UpdateAsync(studentAssignment);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while updating the student assignment.");
        }
    }
}
