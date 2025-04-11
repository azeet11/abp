using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.BookStore.DTOs;
using Acme.BookStore.Students;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Acme.BookStore.Permissions;

namespace Acme.BookStore;

[Authorize]
public class StudentAppService : ApplicationService
{
    private readonly IRepository<Student, Guid> _studentRepository;
    private readonly IGuidGenerator _guidGenerator;

    public StudentAppService(IRepository<Student, Guid> studentRepository, IGuidGenerator guidGenerator)
    {
        _studentRepository = studentRepository;
        _guidGenerator = guidGenerator;
    }

    [Authorize(BookStorePermissions.Students.Create)]
    [HttpPost("api/students")]
    public async Task<StudentDto> CreateAsync(StudentDto studentDto)
    {
        try
        {
            var student = new Student(
                _guidGenerator.Create(),
                studentDto.Name,
                studentDto.Email
            );

            await _studentRepository.InsertAsync(student);
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while creating the student.");
        }
    }

    [Authorize(BookStorePermissions.Students.Delete)]
    [HttpDelete("api/students/{id}")]
    public async Task<string> DeleteAsync(Guid id)
    {
        try
        {
            var student = await _studentRepository.GetAsync(id);
            if (student == null)
            {
                throw new UserFriendlyException("Book not found.");
            }
            await _studentRepository.DeleteAsync(id);
            return await Task.FromResult("Student was deleted successfully.");
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while deleting the student.");
        }
    }

    [Authorize(BookStorePermissions.Students.GetList)]
    [HttpGet("api/students/{id}")]
    public async Task<StudentDto> GetAsync(Guid id)
    {
        try
        {
            var student = await _studentRepository.GetAsync(id);
            if (student == null)
            {
                throw new UserFriendlyException("Book not found.");
            }
            return new StudentDto
            {
                Id = student.Id,
                Name = student.Name,
                Email = student.Email
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the student.");
        }
    }

    [Authorize(BookStorePermissions.Students.Get)]
    [HttpGet("api/students")]
    public async Task<List<StudentDto>> GetListAsync()
    {
        try
        {
            var studentQueryable = await _studentRepository.GetQueryableAsync();
            var students = await studentQueryable.Select(x => new StudentDto()
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email
            }).ToListAsync();

            if (students == null)
            {
                throw new UserFriendlyException("Book not found.");
            }

            return students;
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the student list.");
        }
    }

    [Authorize(BookStorePermissions.Students.Update)]
    [HttpPut("api/students/{id}")]
    public async Task UpdateAsync(Guid id, StudentDto studentDto)
    {
        try
        {
            var student = await _studentRepository.GetAsync(id);
            if (student == null)
            {
                throw new UserFriendlyException("Book not found.");
            }
            student.Name = studentDto.Name;
            student.Email = studentDto.Email;
            await _studentRepository.UpdateAsync(student);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while updating the student.");
        }
    }
}
