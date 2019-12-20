using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracker.Core.DTO;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IProjectService
    {
        Project GetProject(int id, bool fullLoad = false);
        List<Project> GetAllProjects(bool tracking = true);
        List<ProjectListDTO> GetProjectListDTOs();
        Task<List<ProjectListDTO>> GetProjectListDTOsAsync();
        List<Project> GetProjectsDateRange(DateTime startDate, DateTime endDate, bool tracking = true, bool fullLoad = false);
        void AddProject(Project project);
        void UpdateProject(Project project);
        void DeleteProject(Project project);
    }

    public class ProjectService : IProjectService
    {
        public readonly IEFService _context;

        public ProjectService(IEFService eFService)
        {
            _context = eFService;
        }

        public Project GetProject(int id, bool fullLoad = false)
        {
            if (fullLoad)
            {
                Project project = _context.Projects
                                    .Where(p => p.Id == id)
                                    .Include(p => p.Requestor)
                                        .ThenInclude(r => r.Affiliation)
                                    .Include(p => p.Client)
                                    .Include(p => p.Processor)
                                    .Include(p => p.FeeData)
                                    .FirstOrDefault();
                project.MailingAddress.Updated = false;
                project.BillingAddress.Updated = false;
                return project;
            }
            else
            {
                Project project = _context.Projects.Find(id);
                project.MailingAddress.Updated = false;
                project.BillingAddress.Updated = false;
                return project;
            }
        }

        public List<Project> GetAllProjects(bool tracking = true)
        {
            if (tracking)
                return _context.Projects.Include(p => p.Requestor).Include(p => p.Client).OrderProjectsBy(ProjectOrderOptions.FileNumber).ToList();
            else
                return _context.Projects.Include(p => p.Requestor).Include(p => p.Client).OrderProjectsBy(ProjectOrderOptions.FileNumber).AsNoTracking().ToList();
        }

        public List<ProjectListDTO> GetProjectListDTOs()
        {
            return _context.Projects.Select(p => new ProjectListDTO
            {
                Id = p.Id,
                ICTypePrefix = p.ICTypePrefix,
                ICYear = p.ICYear,
                ICEnumeration = p.ICEnumeration,
                ICSuffix = p.ICSuffix,
                ProjectName = p.ProjectName,
                Status = p.Status,
                MainCounty = p.MainCounty,
                PLSS = p.PLSS,
                LastUpdated = p.LastUpdated
            }).OrderProjectListDTOsBy(ProjectOrderOptions.FileNumber)
            .AsNoTracking()
            .ToList();
        }

        public async Task<List<ProjectListDTO>> GetProjectListDTOsAsync()
        {
            return await _context.Projects.Select(p => new ProjectListDTO
            {
                Id = p.Id,
                ICTypePrefix = p.ICTypePrefix,
                ICYear = p.ICYear,
                ICEnumeration = p.ICEnumeration,
                ICSuffix = p.ICSuffix,
                ProjectName = p.ProjectName,
                Status = p.Status,
                MainCounty = p.MainCounty,
                PLSS = p.PLSS,
                LastUpdated = p.LastUpdated
            }).OrderProjectListDTOsBy(ProjectOrderOptions.FileNumber)
            .AsNoTracking()
            .ToListAsync();
        }

        public List<Project> GetProjectsDateRange(DateTime startDate, DateTime endDate, bool tracking = true, bool fullLoad = false)
        {
            if (tracking)
                return _context.Projects
                        .Include(p => p.Requestor)
                        .Include(p => p.Client)
                        .Include(p => p.FeeData)
                        .Where(p => startDate <= p.DateOfResponse && p.DateOfResponse <= endDate)
                        .ToList();
            else
                return _context.Projects
                        .Include(p => p.Requestor)
                        .Include(p => p.Client)
                        .Include(p => p.FeeData)
                        .Where(p => startDate <= p.DateOfResponse && p.DateOfResponse <= endDate)
                        .AsNoTracking()
                        .ToList();
        }
        
        //CUD
        public void AddProject(Project project)
        {
            _context.Projects.Add(project);
            _context.SaveChanges();
        }

        public void UpdateProject(Project project)
        {
            _context.Projects.Update(project);
            _context.SaveChanges();
        }

        public void DeleteProject(Project project)
        {
            _context.Projects.Remove(project);
            _context.SaveChanges();
        }
    }
    public enum ProjectOrderOptions
    {
        FileNumber, ProjectNumber
    }

    public static class ProjectServiceExtensions
    {
        public static IQueryable<Project> OrderProjectsBy(this IQueryable<Project> projects, ProjectOrderOptions orderOption)
        {
            switch (orderOption)
            {
                case ProjectOrderOptions.FileNumber:
                    return projects.OrderBy(p => p.ICTypePrefix).ThenBy(p => p.ICYear).ThenBy(p => p.ICEnumeration).ThenBy(p => p.ICSuffix);
                case ProjectOrderOptions.ProjectNumber:
                    return projects.OrderBy(p => p.ProjectNumber);
                default:
                    return projects.OrderBy(p => p.Id);
            }
        }

        public static IQueryable<ProjectListDTO> OrderProjectListDTOsBy(this IQueryable<ProjectListDTO> projects, ProjectOrderOptions orderOption)
        {
            switch (orderOption)
            {
                case ProjectOrderOptions.FileNumber:
                    return projects.OrderBy(p => p.ICTypePrefix).ThenBy(p => p.ICYear).ThenBy(p => p.ICEnumeration).ThenBy(p => p.ICSuffix);
                default:
                    return projects.OrderBy(p => p.Id);
            }
        }
    }
}
