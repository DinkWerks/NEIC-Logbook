using Prism.Mvvm;
using System.Collections.Generic;

namespace Tracker.Core.Models
{
    public class Staff : BindableBase
    {
        private int _id;
        private string _name;
        private bool _isActive;

        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetProperty(ref _isActive, value);
            }
        }

        public ICollection<Project> StaffProjects { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
