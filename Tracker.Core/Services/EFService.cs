using JetEntityFrameworkProvider;
using System.Data.Entity;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public class EFService : DbContext, IEFService
    {
        public EFService() : base(new JetConnection(@"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + Settings.Settings.Instance.DatabaseAddress) + "; providerName = JetEntityFrameworkProvider;")
        {

        }

        public DbSet<Staff> tblStaff { get; set; }
    }
}
