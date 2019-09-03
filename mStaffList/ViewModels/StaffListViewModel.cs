using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mStaffList.ViewModels
{
    public class StaffListViewModel : BindableBase
    {
        private ObservableCollection<Staff> _staffMembers;

        public ObservableCollection<Staff> StaffMembers
        {
            get { return _staffMembers; }
            set { SetProperty(ref _staffMembers, value); }
        }

        public StaffListViewModel(IStaffContext staffContext)
        {
            StaffMembers = 
        }
    }
}
