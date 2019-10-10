using Microsoft.EntityFrameworkCore;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IEFService
    {
        DbSet<Address> Addresses { get; set; }
        DbSet<Person> People { get; set; }
        DbSet<Staff> Staffs { get; set; }
    }
}
