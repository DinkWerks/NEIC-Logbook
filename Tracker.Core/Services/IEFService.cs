using Microsoft.EntityFrameworkCore;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IEFService
    {
        DbSet<Project> Projects { get; set; }
        DbSet<Organization> Organizations { get; set; }
        DbSet<Person> People { get; set; }
        DbSet<Address> Addresses { get; set; }
        DbSet<Staff> Staffs { get; set; }
    }
}
