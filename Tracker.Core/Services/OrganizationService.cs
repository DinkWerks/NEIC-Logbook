using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IOrganizationService
    {
        Organization GetOrganization(int id);
        ObservableCollection<Organization> GetAllOrganizations();
        Organization UpdateOrganization(Organization organization);
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

        public ObservableCollection<Organization> GetAllOrganizations()
        {
            return new ObservableCollection<Organization>(_context.Organizations.AsNoTracking().ToList());
        }

        public Organization UpdateOrganization(Organization organization)
        {
            var org = _context.Organizations.Find(organization.ID);
            org.OrganizationName = organization.OrganizationName;
            _context.SaveChanges();
            return org;
        }
    }
}
