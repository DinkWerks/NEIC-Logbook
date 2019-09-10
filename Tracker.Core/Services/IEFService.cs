using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IEFService
    {
        DbSet<Staff> tblStaff { get; set; }
    }
}
