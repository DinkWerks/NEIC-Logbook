using JetEntityFrameworkProvider;
using System.Data.Entity;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public class EFService : DbContext, IEFService
    {
        public EFService() : base("name=ConnString")
        {
            
        }

        public DbSet<RecordSearch> tblRecordSearches { get; set; }
        public DbSet<Staff> tblStaff { get; set; }
    }
}
