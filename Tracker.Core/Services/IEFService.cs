using Microsoft.EntityFrameworkCore;
using Tracker.Core.Models;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Services
{
    public interface IEFService
    {
        DbSet<Project> Projects { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Person> People { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<Staff> Staff { get; set; }
        DbSet<FeeData> FeeData { get; set; }
        DbSet<OrganizationStanding> OrganizationStandings { get; set; }

        int SaveChanges();
    }
}
