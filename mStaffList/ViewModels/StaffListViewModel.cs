using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using Tracker.Core;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mStaffList.ViewModels
{
    public class StaffListViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private readonly IStaffService _ss;
        private readonly IRecordSearchService _rss;
        private readonly IEventAggregator _ea;
        private readonly IRegionManager _rm;
        private readonly IEFService _ef;
        private ObservableCollection<Staff> _staffMembers;
        private Staff _selectedStaff;
        private ObservableCollection<RecordSearch> _staffRecordSearches;
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
                    StaffRecordSearches = new ObservableCollection<RecordSearch>(_rss.GetPartialRecordSearchesByCriteria("WHERE Processor = \"" + value.Name + "\""));
                SetProperty(ref _selectedStaff, value);
            }
        }

        public ObservableCollection<RecordSearch> StaffRecordSearches
        {
            get { return _staffRecordSearches; }
            set { SetProperty(ref _staffRecordSearches, value); }
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
        public StaffListViewModel(IStaffService staffService, IEFService efService, IRecordSearchService recordSearchService, IEventAggregator eventAggregator, IRegionManager regionManager)
        {
            _ss = staffService;
            _rss = recordSearchService;
            _ea = eventAggregator;
            _rm = regionManager;
            _ef = efService;

            StaffMembers = new ObservableCollection<Staff>(staffService.CompleteStaffList);

            AddPersonCommand = new DelegateCommand<string>(AddPerson);
            DeletePersonCommand = new DelegateCommand(DeletePerson);

            _ea.GetEvent<RecordSearchListSelectEvent>().Subscribe(NavigateToRSEntry);
        }

        //Methods
        private void AddPerson(string name)
        {
            Staff newMember = new Staff()
            {
                Name = name,
                IsActive = true
            };

            newMember.ID = _ss.AddStaffMember(newMember);
            StaffMembers.Add(newMember);
            NewPersonName = "";

            _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Staff Member Added.", Palette.AlertGreen));
        }

        private void UpdatePerson()
        {
            if (SelectedStaff.IsChanged)
            {
                _ss.UpdateStaffMember(SelectedStaff);
                _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Staff Member Updated.", Palette.AlertGreen));
                SelectedStaff.IsChanged = false;
            }
        }

        private void DeletePerson()
        {
            _ss.DeleteStaffMember(SelectedStaff.ID);
            StaffMembers = new ObservableCollection<Staff>(_ss.CompleteStaffList);
            _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Staff Member Deleted.", Palette.AlertGreen));
        }

        private void NavigateToRSEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID >= 0)
                _rm.RequestNavigate("ContentRegion", "RSEntry", parameters);
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
