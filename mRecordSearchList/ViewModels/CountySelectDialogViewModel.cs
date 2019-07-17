using mRecordSearchList.Notifications;
using Prism.Commands;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.ViewModels
{
    public class CountySelectDialogViewModel : BindableBase, IInteractionRequestAware
    {
        private IEnumerable<County> _selectableCounties = new List<County>();
        private ObservableCollection<County> _selectedCounties = new ObservableCollection<County>();
        private RecordSearch _recordSearch;
        private IAdditionalCountyNotification _notification;
        private IRecordSearchService _rss;

        public IEnumerable<County> SelectableCounties
        {
            get { return _selectableCounties; }
            set
            {
                SetProperty(ref _selectableCounties, value);
            }
        }

        public ObservableCollection<County> SelectedCounties
        {
            get { return _selectedCounties; }
            set { SetProperty(ref _selectedCounties, value); }
        }

        public RecordSearch RecordSearch
        {
            get { return _recordSearch; }
            set { SetProperty(ref _recordSearch, value); }
        }

        public INotification Notification
        {
            get { return _notification; }
            set
            {
                SetProperty(ref _notification, (IAdditionalCountyNotification)value);
                // Need to execute here as this fires when the notification is raised.
                // Constructor fires before the RSEntry
                RecordSearch = _rss.CurrentRecordSearch;
                foreach (County county in SelectableCounties)
                {
                    if (RecordSearch.AdditionalCounties.Contains(county))
                        county.IsChecked = true;
                }
            }
        }

        public DelegateCommand AcceptCommand { get; private set; }
        public DelegateCommand CancelCommand { get; private set; }
        public Action FinishInteraction { get; set; }

        // Constructor
        public CountySelectDialogViewModel(IRecordSearchService recordSearchService)
        {
            _rss = recordSearchService;
            SelectableCounties = from c in Counties.Values
                                 where c.ICCurator is "NEIC"
                                 select c;

            CancelCommand = new DelegateCommand(Cancel);
            AcceptCommand = new DelegateCommand(Accept);
        }

        // Methods
        private void Cancel()
        {
            _notification.Confirmed = false;
            FinishInteraction.Invoke();
        }

        private void Accept()
        {
            _notification.AdditionalCounties = new ObservableCollection<County>(from c in SelectableCounties
                                                                                where c.IsChecked is true
                                                                                select c);
            _notification.Confirmed = true;
            FinishInteraction?.Invoke();
        }
    }
}
