using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IOrganizationService
    {
        Organization GetOrganization(int id);
        List<Organization> GetAllOrganizations();
        void AddOrganization(Organization organization);
        void UpdateOrganization(Organization organization);
        void DeleteOrganization(Organization organization);
    }

    public class OrganizationService : IOrganizationService
    {
        private readonly IEFService _context;

        public OrganizationService(IEFService eFService)
        {
            _context = eFService;
        }

        public Organization GetOrganization(int id)
        {
            return _context.Organizations
                        .Include(o => o.OrganizationStanding)
                        .Include(o => o.Employees)
                        .Where(o => o.ID == id)
                        .FirstOrDefault();
        }

        public List<Organization> GetAllOrganizations()
        {
            return _context.Organizations
                .Include(o => o.OrganizationStanding)
                .OrderBy(o => o.OrganizationName)
                .ToList();
        }

        public void AddOrganization(Organization organization)
        {
            _context.Organizations.Add(organization);
            _context.SaveChanges();
        }

        public void UpdateOrganization(Organization organization)
        {
            _context.Organizations.Update(organization);
            _context.SaveChanges();
        }

        public void DeleteOrganization(Organization organization)
        {
            _context.Organizations.Remove(organization);
            _context.SaveChanges();
        }
    }
}
