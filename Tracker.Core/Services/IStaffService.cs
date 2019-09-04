using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IStaffService
    {
        string ConnectionString { get; set; }
        List<Staff> CompleteStaffList { get; set; }
        void SetConnectionString();
        List<Staff> GetAllStaff();
        void DeleteStaffMember(int id);
        int AddStaffMember(Staff newMember);
        void UpdateStaffMember(Staff staff);
    }
}
