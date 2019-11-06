using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IProjectService
    {
        Project GetProject(int id, bool fullLoad = false);
        List<Project> GetAllProjects();

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
                return _context.Projects
                    .Where(p => p.Id == id)
                    .Include(p => p.Requestor)
                        .ThenInclude(r => r.Affiliation)
                    .Include(p => p.Client)
                    .Include(p => p.Processor)
                    .Include(p => p.FeeData)                   
                    .FirstOrDefault();
            }
            else
                return _context.Projects.Find(id);
        }

        public List<Project> GetAllProjects()
        {
            return _context.Projects
                .Include(p => p.Requestor)
                .Include(p => p.Client)
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
