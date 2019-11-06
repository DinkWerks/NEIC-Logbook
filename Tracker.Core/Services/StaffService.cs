using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IStaffService
    {
        Staff GetStaff(int id, bool fullLoad = false);
        List<Staff> GetAllStaff(bool fullLoad = false);
        void AddStaff(Staff staff);
        void UpdateStaff(Staff staff);
        void DeleteStaff(Staff staff);
    }

    public class StaffService : IStaffService
    {
        public readonly IEFService _context;

        public StaffService(IEFService eFService)
        {
            _context = eFService;
        }

        public Staff GetStaff(int id, bool fullLoad = false)
        {
            if (fullLoad)
            {
                return _context.Staff
                    .Where(s => s.ID == id)
                    .Include(s => s.StaffProjects)
                    .FirstOrDefault();
            }
            else
                return _context.Staff.Find(id);
        }

        public List<Staff> GetAllStaff(bool fullLoad = false)
        {
            //TODO Make full load pull staff projects in a separate query that can limit the amount of projects pulled.
            if (fullLoad)
            {
                return _context.Staff
                    .Include(s => s.StaffProjects)
                    .ToList();
            }
            else
                return _context.Staff.ToList();
        }

        public void AddStaff(Staff staff)
        {
            _context.Staff.Add(staff);
            _context.SaveChanges();
        }

        public void UpdateStaff(Staff staff)
        {
            _context.Staff.Update(staff);
            _context.SaveChanges();
        }

        public void DeleteStaff(Staff staff)
        {
            _context.Staff.Remove(staff);
            _context.SaveChanges();
        }
    }
}
