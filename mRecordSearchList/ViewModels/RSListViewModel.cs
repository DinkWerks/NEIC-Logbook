using mRecordSearchList.Notifications;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.ViewModels
{
    public class RSListViewModel : NavigatableBindableBase, INavigationAware
    {
        private readonly IRecordSearchService _rss;
        private readonly IRegionManager _rm;
        private List<RecordSearch> _recordSearches = new List<RecordSearch>();
        private string _rsidPrefixSearch;
        private string _rsidYearSearch;
        private string _rsidEnumerationSearch;
        private string _projectNameSearchText;
        private IRegionNavigationJournal _journal;
        private ICollectionView _recordSearchesView;

        public List<RecordSearch> RecordSearches
        {
            get { return _recordSearches; }
            set
            {
                SetProperty(ref _recordSearches, value);
            }
        }

        public ICollectionView RecordSearchesView
        {
            get { return _recordSearchesView; }
        }

        public List<Prefix> PrefixChoices { get; private set; }

        public string RSIDPrefixSearch
        {
            get { return _rsidPrefixSearch; }
            set
            {
                SetProperty(ref _rsidPrefixSearch, value);
                RecordSearchesView.Refresh();
            }
        }

        public string RSIDYearSearch
        {
            get { return _rsidYearSearch; }
            set
            {
                SetProperty(ref _rsidYearSearch, value);
                RecordSearchesView.Refresh();
            }
        }

        public string RSIDEnumerationSearch
        {
            get { return _rsidEnumerationSearch; }
            set
            {
                SetProperty(ref _rsidEnumerationSearch, value);
                RecordSearchesView.Refresh();
            }
        }

        public string ProjectNameSearchText
        {
            get { return _projectNameSearchText; }
            set
            {
                SetProperty(ref _projectNameSearchText, value);
                RecordSearchesView.Refresh();
            }
        }

        public InteractionRequest<ICreateNewRSNotification> NewRSRequest { get; private set; }
        public DelegateCommand CreateNewRSCommand { get; private set; }

        //Constructor
        public RSListViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IRecordSearchService recordSearchService, 
            IApplicationCommands applicationCommands) :base(applicationCommands)
        {
            _rm = regionManager;
            _rss = recordSearchService;
            PrefixChoices = new List<Prefix>(RecordSearchPrefixes.Values);

            RecordSearches = _rss.GetAllPartialRecordSearches();
            _recordSearchesView = CollectionViewSource.GetDefaultView(RecordSearches);

            RecordSearchesView.Filter = RecordSearchViewFilter;
            NewRSRequest = new InteractionRequest<ICreateNewRSNotification>();
            CreateNewRSCommand = new DelegateCommand(CreateNewRecordSearch);

            eventAggregator.GetEvent<RecordSearchListSelectEvent>().Subscribe(NavigateToRecordSearchEntry);
            eventAggregator.GetEvent<RSListModifiedEvent>().Subscribe(ModifyRSList);
        }

        //Methods
        public void NavigateToRecordSearchEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID >= 0)
                _rm.RequestNavigate("ContentRegion", "RSEntry", parameters);
        }

        public void CreateNewRecordSearch()
        {
            object[] newRSData = new object[7];
            NewRSRequest.Raise(new CreateNewRSNotification { Title = "Create a New Record Search" }, r =>
             {
                 if (r.Confirmed)
                 {
                     newRSData[0] = r.Prefix;
                     newRSData[1] = r.Year;
                     newRSData[2] = r.Enumeration;
                     newRSData[3] = r.Suffix;
                     newRSData[4] = r.ProjectName;
                     newRSData[5] = DateTime.Now.Date;
                     newRSData[6] = DateTime.Now.Date;
                     int lastEntryID = _rss.AddNewRecordSearch(newRSData);
                     ModifyRSList(new ListModificationPayload("add", lastEntryID));

                     NavigateToRecordSearchEntry(lastEntryID);
                 }
             }
            );
        }

        private void ModifyRSList(ListModificationPayload payload)
        {
            switch (payload.Action)
            {
                case "add":
                    RecordSearches.Add(_rss.GetRecordSearchByID(payload.ID));
                    break;
                case "delete":
                    RecordSearches.RemoveAll(rs => rs.ID == payload.ID);
                    break;
            }
            RecordSearchesView.Refresh();
        }

        public bool RecordSearchViewFilter(object filterable)
        {
            RecordSearch recordSearch = filterable as RecordSearch;
            if (recordSearch == null)
            {
                return false;
            }

            int passedTests = 0;

            if (ProjectNameSearchText != null)
            {
                if (recordSearch.ProjectName.ToLower().Contains(ProjectNameSearchText.ToLower()))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (RSIDPrefixSearch != null)
            {
                if (recordSearch.ICTypePrefix.ToUpper() == RSIDPrefixSearch)
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (RSIDYearSearch != null)
            {
                if (recordSearch.ICYear.ToLower().Contains(RSIDYearSearch))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (RSIDEnumerationSearch != null)
            {
                if (recordSearch.ICEnumeration.ToString().Contains(RSIDEnumerationSearch))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            return passedTests >= 4;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
