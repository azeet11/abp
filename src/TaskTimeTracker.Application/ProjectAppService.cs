using System;
using TaskTimeTracker.Entities;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TaskTimeTracker.DTOs;
using Volo.Abp;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using TaskTimeTracker.Permissions;
using Microsoft.Extensions.Logging;
using TaskTimeTracker.Interfaces;

namespace TaskTimeTracker;

[Authorize(TaskTimeTrackerPermissions.Projects.Default)]
public class ProjectAppService : ApplicationService, ITransientDependency, IProjectAppService
{
    private readonly IRepository<Project, Guid> _projectRepository;
    private readonly IGuidGenerator _guidGenerator;

    public ProjectAppService(IRepository<Project, Guid> projectRepository, IGuidGenerator guidGenerator)
    {
        _projectRepository = projectRepository;
        _guidGenerator = guidGenerator;
    }

    [Authorize(TaskTimeTrackerPermissions.Projects.Create)]
    [HttpPost("api/projects")]
    public async Task<ProjectDto> CreateAsync(ProjectDto project)
    {
        try
        {
            var newProject = new Project
                (
                    _guidGenerator.Create(),
                    project.Name,
                    project.Description,
                    project.UserId,
                    project.StartDate,
                    project.EndDate,
                    project.Status
                );

            await _projectRepository.InsertAsync(newProject);

            return new ProjectDto
            {
                Id = newProject.Id,
                Name = newProject.Name,
                Description = newProject.Description,
                UserId = newProject.UserId,
                StartDate = newProject.StartDate,
                EndDate = newProject.EndDate,
                Status = newProject.Status
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Project could not be created."); // Log the exception

            throw new UserFriendlyException("An error occurred while creating the project.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.Projects.Default)]
    [HttpGet("api/projects")]
    public async Task<List<ProjectDto>> GetListAsync([FromQuery] PagedAndFilteredResultRequestDto input)
    {
        try
        {
            var projectQueryable = await _projectRepository.GetQueryableAsync();

            // Apply general filter (Name or Description)
            if (!string.IsNullOrWhiteSpace(input.Filter))
            {
                projectQueryable = projectQueryable.Where(p =>
                    p.Name.Contains(input.Filter) ||
                    p.Description.Contains(input.Filter));
            }

            // Filter by UserId
            if (input.UserId.HasValue)
            {
                projectQueryable = projectQueryable.Where(p => p.UserId == input.UserId.Value);
            }

            // Filter by Status
            if (!string.IsNullOrWhiteSpace(input.Status))
            {
                projectQueryable = projectQueryable.Where(p => p.Status == input.Status);
            }

            // Apply pagination
            var projects = await projectQueryable
                .OrderBy(p => p.Name) // Optional: Order by Name
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .Select(p => new ProjectDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    UserId = p.UserId,
                    StartDate = p.StartDate,
                    EndDate = p.EndDate,
                    Status = p.Status
                })
                .ToListAsync();

            return projects;
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the projects.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.Projects.Default)]
    [HttpGet("api/projects/{id}")]
    public async Task<ProjectDto> GetAsync(Guid id)
    {
        try
        {
            var project = await (await _projectRepository.GetQueryableAsync()).Where(p => p.Id == id).Select(x => new ProjectDto
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                UserId = x.UserId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Status = x.Status
            }).FirstOrDefaultAsync();

            if (project == null)
            {
                throw new UserFriendlyException("Project not found.");
            }
            return project;
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while retrieving the project.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.Projects.Update)]
    [HttpPut("api/projects/{id}")]
    public async Task<ProjectDto> UpdateAsync(Guid id, ProjectDto project)
    {
        try
        {
            var existingProject = await _projectRepository.GetAsync(id);
            existingProject.Name = project.Name;
            existingProject.Description = project.Description;
            existingProject.UserId = project.UserId;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.Status = project.Status;
            await _projectRepository.UpdateAsync(existingProject);
            return new ProjectDto
            {
                Id = existingProject.Id,
                Name = existingProject.Name,
                Description = existingProject.Description,
                UserId = existingProject.UserId,
                StartDate = existingProject.StartDate,
                EndDate = existingProject.EndDate,
                Status = existingProject.Status
            };
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while updating the project.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.Projects.Delete)]
    [HttpDelete("api/projects/{id}")]
    public async Task DeleteAsync(Guid id)
    {
        try
        {
            var existingProject = await _projectRepository.GetAsync(id);
            if (existingProject == null)
            {
                throw new UserFriendlyException("Project not found.");
            }
            await _projectRepository.DeleteAsync(id);
        }
        catch (UserFriendlyException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new UserFriendlyException("An error occurred while deleting the project.");
        }
    }
}