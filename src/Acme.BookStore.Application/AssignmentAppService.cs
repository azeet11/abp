using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Acme.BookStore.DTOs;
using Acme.BookStore.Assignments;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Guids;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Acme.BookStore.Permissions;

namespace Acme.BookStore;

[Authorize]
public class AssignmentAppService : ApplicationService
{
    private readonly IRepository<Assignment, Guid> _assignmentRepository;
    private readonly IGuidGenerator _guidGenerator;

    public AssignmentAppService(IRepository<Assignment, Guid> assignmentRepository, IGuidGenerator guidGenerator)
    {
        _assignmentRepository = assignmentRepository;
        _guidGenerator = guidGenerator;
    }

    [Authorize(BookStorePermissions.Assignments.Create)]
    [HttpPost("api/assignments")]
    public async Task<AssignmentDto> CreateAsync(AssignmentDto assignmentDto)
    {
        try
        {
            var assignment = new Assignment(
                _guidGenerator.Create(),
                assignmentDto.Title,
                assignmentDto.Description,
                assignmentDto.DueDate
            );

            await _assignmentRepository.InsertAsync(assignment);
            return new AssignmentDto
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while creating the assignment.");
        }
    }

    [Authorize(BookStorePermissions.Assignments.Delete)]
    [HttpDelete("api/assignments/{id}")]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            await _assignmentRepository.DeleteAsync(id);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while deleting the assignment.");
        }
    }

    [Authorize(BookStorePermissions.Assignments.GetList)]
    [HttpGet("api/assignments/{id}")]
    public async Task<AssignmentDto> GetAsync(Guid id)
    {
        try
        {
            var assignment = await _assignmentRepository.GetAsync(id);
            if (assignment == null)
            {
                throw new UserFriendlyException("Book not found.");
            }
            return new AssignmentDto
            {
                Id = assignment.Id,
                Title = assignment.Title,
                Description = assignment.Description,
                DueDate = assignment.DueDate
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the assignment.");
        }
    }

    //[Authorize(BookStorePermissions.Assignments.Get)]
    [HttpGet("api/assignments")]
    public async Task<List<AssignmentDto>> GetListAsync()
    {
        try
        {
            var assignmentQueryable = await _assignmentRepository.GetQueryableAsync();
            var assignmentDtos = await assignmentQueryable.Select(x => new AssignmentDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                DueDate = x.DueDate
            }).ToListAsync();
            if (assignmentDtos == null || !assignmentDtos.Any())
            {
                throw new UserFriendlyException("No assignments found.");
            }
            return assignmentDtos;
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the assignment list.");
        }
    }

    [Authorize(BookStorePermissions.Assignments.Update)]
    [HttpPut("api/assignments/{id}")]
    public async Task UpdateAsync(Guid id, AssignmentDto assignmentDto)
    {
        try
        {
            var assignment = await _assignmentRepository.GetAsync(id);
            if (assignment == null)
            {
                throw new UserFriendlyException("Book not found.");
            }
            assignment.Title = assignmentDto.Title;
            assignment.Description = assignmentDto.Description;
            assignment.DueDate = assignmentDto.DueDate;
            await _assignmentRepository.UpdateAsync(assignment);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while updating the assignment.");
        }
    }
}
