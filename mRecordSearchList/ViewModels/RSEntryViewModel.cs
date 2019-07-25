using System.Collections.Generic;
using Prism.Mvvm;
using Prism.Interactivity.InteractionRequest;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Tracker.Core.Models;
using Tracker.Core.Events.CustomPayloads;
using Tracker.Core.Events;
using Tracker.Core.Services;
using mFeeCalculator.Views;
using mRecordSearchList.Views;
using mRecordSearchList.Notifications;
using System.Collections.ObjectModel;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.ViewModels
{
    public class RSEntryViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private IRegionManager _rm;
        private IEventAggregator _ea;
        private IRecordSearchService _rss;
        private IPersonService _ps;
        private IClientService _cs;
        private RecordSearch _recordSearch;
        private int _selectedRequestor;
        private int _selectedClient;
        private bool _isLoaded = false;
        private IRegionNavigationJournal _journal;

        public List<Person> PeopleList { get; set; }
        public List<Client> ClientList { get; set; }

        public RecordSearch RecordSearch
        {
            get { return _recordSearch; }
            set
            {
                SetProperty(ref _recordSearch, value);
            }
        }

        public int SelectedRequestor
        {
            get { return _selectedRequestor; }
            set
            {
                LoadNewRequestor(value);
                SetProperty(ref _selectedRequestor, value);
            }
        }

        public int SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                LoadNewClient(value);
                SetProperty(ref _selectedClient, value);
            }
        }

        public DelegateCommand SaveCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public InteractionRequest<IAdditionalCountyNotification> CountySelectRequest { get; set; }
        public DelegateCommand CountySelectPopupCommand { get; private set; }
        public DelegateCommand<string> CopyRequestorCommand { get; private set; }
        public DelegateCommand<string> CopyAffiliationCommand { get; private set; }
        public bool KeepAlive => false;

        // Constructor
        public RSEntryViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IPersonService personService, IClientService clientService,
            IRecordSearchService recordSearchService)
        {
            _rm = regionManager;
            _ea = eventAggregator;
            _rss = recordSearchService;
            _ps = personService;
            _cs = clientService;
            PeopleList = personService.CompletePeopleList;
            ClientList = clientService.CompleteClientList;

            regionManager.RegisterViewWithRegion("RequestorAddress", typeof(AddressEntry));
            regionManager.RegisterViewWithRegion("BillingAddress", typeof(AddressEntry));
            regionManager.RegisterViewWithRegion("CalculatorRegion", typeof(Calculator));

            SaveCommand = new DelegateCommand(SaveRS);
            CountySelectRequest = new InteractionRequest<IAdditionalCountyNotification>();
            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
            CountySelectPopupCommand = new DelegateCommand(RaiseCountySelectPopup);
            CopyRequestorCommand = new DelegateCommand<string>(CopyRequestor);
            CopyAffiliationCommand = new DelegateCommand<string>(CopyAffiliation);
            eventAggregator.GetEvent<AdditionalCountySelectionEvent>().Subscribe(ChangeAdditionalCounties);
        }

        // Methods
        private void SaveRS()
        {
            _rss.UpdateRecordSearch(RecordSearch);
        }

        private void Navigate(string destination)
        {
            if (destination == "Requestor")
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("id", SelectedRequestor);
                if (SelectedClient > 0)
                    _rm.RequestNavigate("ContentRegion", "PersonEntry", parameters);
            }
            else if (destination == "Client")
            {
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("id", SelectedClient);
                if (SelectedClient > 0)
                    _rm.RequestNavigate("ContentRegion", "ClientEntry", parameters);
            }
        }

        private void GoBack()
        {
            _journal.GoBack();
        }

        private void CopyRequestor(string destination)
        {
            if (destination == "Mailing")
                RecordSearch.MailingAddress = RecordSearch.Requestor.AddressModel;
            else if (destination == "Billing")
                RecordSearch.BillingAddress = RecordSearch.Requestor.AddressModel;
        }

        private void CopyAffiliation(string destination)
        {
            if (destination == "Mailing")
                RecordSearch.MailingAddress = RecordSearch.ClientModel.AddressModel;
            else if (destination == "Billing")
                RecordSearch.BillingAddress = RecordSearch.ClientModel.AddressModel;
        }

        private void LoadNewRequestor(int value)
        {
            if (value > 0 && _isLoaded)
            {
                RecordSearch.Requestor = _ps.GetPersonByID(value);
                RecordSearch.RequestorID = RecordSearch.Requestor.ID;
                SelectedClient = RecordSearch.Requestor.CurrentAssociationID;
            }
        }

        private void LoadNewClient(int value)
        {
            if (value > 0 && _isLoaded)
            {
                RecordSearch.ClientModel = _cs.GetClientByID(value);
                RecordSearch.ClientID = RecordSearch.ClientModel.ID;
            }
        }

        private void RaiseCountySelectPopup()
        {
            CountySelectRequest.Raise(new AdditionalCountyNotification { Title = "Select Additional Counties" }, r =>
            {
                if (r.Confirmed)
                {
                    RecordSearch.AdditionalCounties = r.AdditionalCounties;
                }
            });
        }

        private void ChangeAdditionalCounties(AdditionalCountySelectionPayload payload)
        {
            if (payload.IsAdded is true)
            {
                if (RecordSearch.AdditionalCounties.Contains(payload.CountyPayload))
                    RecordSearch.AdditionalCounties.Add(payload.CountyPayload);
            }
            if (payload.IsAdded is false)
            {
                if (!RecordSearch.AdditionalCounties.Contains(payload.CountyPayload))
                    RecordSearch.AdditionalCounties.Remove(payload.CountyPayload);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;
            int rsID = (int)navigationContext.Parameters["id"];
            if (rsID > 0)
            {
                _rss.GetRecordSearchByID(rsID, true);
                RecordSearch = _rss.CurrentRecordSearch;
                SelectedRequestor = RecordSearch.RequestorID;
                SelectedClient = RecordSearch.ClientID;
                _isLoaded = true;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            SaveRS();
        }
    }
}
