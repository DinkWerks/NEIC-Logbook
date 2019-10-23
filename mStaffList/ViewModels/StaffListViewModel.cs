using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Tracker.Core;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mStaffList.ViewModels
{
    public class StaffListViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly IEventAggregator _ea;
        private readonly IRegionManager _rm;
        private ObservableCollection<Staff> _staffMembers;
        private Staff _selectedStaff;
        private ObservableCollection<Project> _staffProjects;
        private string _newPersonName;

        public ObservableCollection<Staff> StaffMembers
        {
            get { return _staffMembers; }
            set { SetProperty(ref _staffMembers, value); }
        }

        public Staff SelectedStaff
        {
            get { return _selectedStaff; }
            set
            {
                if (_selectedStaff != null)
                    UpdatePerson();
                if (value != null && value.Name != string.Empty)
                    using(var context = new EFService())
                    {
                        StaffProjects = new ObservableCollection<Project>(
                            context.Projects
                                .Where(p => p.Processor == value)
                                .OrderBy(p => p.DateReceived)
                                .Take(10)
                            );
                    }
                SetProperty(ref _selectedStaff, value);
            }
        }

        public ObservableCollection<Project> StaffProjects
        {
            get { return _staffProjects; }
            set { SetProperty(ref _staffProjects, value); }
        }

        public string NewPersonName
        {
            get { return _newPersonName; }
            set { SetProperty(ref _newPersonName, value); }
        }

        public DelegateCommand<string> AddPersonCommand { get; private set; }
        public DelegateCommand DeletePersonCommand { get; private set; }
        public bool KeepAlive => false;

        //Constructor
        public StaffListViewModel(IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _ea = eventAggregator;
            _rm = regionManager;

            using (var context = new EFService())
            {
                StaffMembers = new ObservableCollection<Staff>(context.Staff.ToList());
            }
            
            AddPersonCommand = new DelegateCommand<string>(AddPerson);
            DeletePersonCommand = new DelegateCommand(DeletePerson);

            _ea.GetEvent<ProjectListSelectEvent>().Subscribe(NavigateToRSEntry);
        }

        //Methods
        private void AddPerson(string name)
        {
            using (var context = new EFService())
            {
                Staff newMember = new Staff()
                {
                    Name = name,
                    IsActive = true
                };

                context.Staff.Add(newMember);
                context.SaveChanges();
                StaffMembers = new ObservableCollection<Staff>(context.Staff.ToList());
            }

            NewPersonName = "";
            _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Staff Member Added.", Palette.AlertGreen));
        }

        private void UpdatePerson()
        {
            using (var context = new EFService())
            {
                context.Update(SelectedStaff);
                context.SaveChanges();
                _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Staff Member Updated.", Palette.AlertGreen));
            }
        }

        private void DeletePerson()
        {
            using (var context = new EFService())
            {
                context.Remove(SelectedStaff);
                _selectedStaff = null;
                context.SaveChanges();
                StaffMembers = new ObservableCollection<Staff>(context.Staff.ToList());
                _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Staff Member Deleted.", Palette.AlertGreen));
            }
        }

        private void NavigateToRSEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID >= 0)
                _rm.RequestNavigate("ContentRegion", "ProjectEntry", parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (SelectedStaff != null)
            {
                UpdatePerson();
            }
        }
    }
}
