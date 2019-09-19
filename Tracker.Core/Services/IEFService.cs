using System.Data.Entity;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IEFService
    {
        DbSet<Staff> tblStaff { get; set; }
    }
}
