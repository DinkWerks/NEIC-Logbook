using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IProjectService
    {
        Project GetProject(int id, bool fullLoad = false);
        List<Project> GetAllProjects(bool tracking = true);
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
                return _context.Projects.Include(p => p.Requestor).Include(p => p.Client).ToList();
            else
                return _context.Projects.Include(p => p.Requestor).Include(p => p.Client).AsNoTracking().ToList();
        }

        public List<Project> GetProjectsDateRange(DateTime startDate, DateTime endDate, bool tracking = true, bool fullLoad = false)
        {
            if (tracking)
                return _context.Projects
                        .Include(p => p.Requestor)
                        .Include(p => p.Client)
                        .Include(p => p.FeeData)
                        .Where(p => startDate > p.DateReceived && p.DateReceived < endDate)
                        .ToList();
            else
                return _context.Projects
                        .Include(p => p.Requestor)
                        .Include(p => p.Client)
                        .Include(p => p.FeeData)
                        .Where(p => startDate > p.DateReceived && p.DateReceived < endDate)
                        .AsNoTracking()
                        .ToList();
        }

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
}
