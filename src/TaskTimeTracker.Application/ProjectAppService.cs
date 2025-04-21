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

namespace TaskTimeTracker;

[Authorize(TaskTimeTrackerPermissions.Projects.Default)]
public class ProjectAppService : ApplicationService, ITransientDependency
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
            throw new UserFriendlyException("An error occurred while creating the project.");
        }
    }

    [Authorize(TaskTimeTrackerPermissions.Projects.Default)]
    [HttpGet("api/projects")]
    public async Task<List<ProjectDto>> GetListAsync()
    {
        try
        {
            var projectQueryable = await _projectRepository.GetQueryableAsync();
            var projects = await projectQueryable.Select(p => new ProjectDto()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                UserId = p.UserId,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Status = p.Status
            }).ToListAsync();

            if (projects == null || !projects.Any())
            {
                throw new UserFriendlyException("No projects found.");
            }

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
            var project = await _projectRepository.GetAsync(id);
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                UserId = project.UserId,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                Status = project.Status
            };
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