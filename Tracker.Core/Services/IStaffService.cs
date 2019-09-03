using System.Collections.Generic;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IStaffService
    {
        string ConnectionString { get; set; }
        List<Staff> GetAllStaff();
    }
}
