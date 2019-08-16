using System.Windows;
using System.Collections.Generic;
using Prism.Interactivity.InteractionRequest;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Tracker.Core.Models;
using Tracker.Core.BaseClasses;
using Tracker.Core.Events.CustomPayloads;
using Tracker.Core.Events;
using Tracker.Core.Services;
using Tracker.Core.CompositeCommands;
using mFeeCalculator.Views;
using mRecordSearchList.Views;
using mRecordSearchList.Notifications;


namespace mRecordSearchList.ViewModels
{
    public class RSEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private IRegionManager _rm;
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

        //Commands
        public DelegateCommand ChangeFileNumCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }
        public DelegateCommand CountySelectPopupCommand { get; private set; }
        public DelegateCommand<string> CopyRequestorCommand { get; private set; }
        public DelegateCommand<string> CopyClientCommand { get; private set; }

        //Requests
        public InteractionRequest<IChangeICFileNumberNotification> ChangeFileNumRequest { get; set; }
        public InteractionRequest<IAdditionalCountyNotification> CountySelectRequest { get; set; }
        public InteractionRequest<IConfirmation> DeleteConfirmationRequest { get; set; }

        // Constructor
        public RSEntryViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IPersonService personService, IClientService clientService,
            IRecordSearchService recordSearchService, IApplicationCommands applicationCommands) : base(applicationCommands)
        {
            _rm = regionManager;
            _rss = recordSearchService;
            _ps = personService;
            _cs = clientService;
            PeopleList = personService.CompletePeopleList;
            ClientList = clientService.CompleteClientList;

            regionManager.RegisterViewWithRegion("RequestorAddress", typeof(AddressEntry));
            regionManager.RegisterViewWithRegion("BillingAddress", typeof(AddressEntry));
            regionManager.RegisterViewWithRegion("CalculatorRegion", typeof(Calculator));

            ChangeFileNumCommand = new DelegateCommand(ChangeFileNum);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
            CountySelectPopupCommand = new DelegateCommand(RaiseCountySelectPopup);
            CopyRequestorCommand = new DelegateCommand<string>(CopyRequestor);
            CopyClientCommand = new DelegateCommand<string>(CopyAffiliation);

            ChangeFileNumRequest = new InteractionRequest<IChangeICFileNumberNotification>();
            CountySelectRequest = new InteractionRequest<IAdditionalCountyNotification>();
            DeleteConfirmationRequest = new InteractionRequest<IConfirmation>();

            eventAggregator.GetEvent<AdditionalCountySelectionEvent>().Subscribe(ChangeAdditionalCounties);
        }

        // Methods
        public override void SaveEntry()
        {
            _rss.UpdateRecordSearch(RecordSearch);
        }

        public override void DeleteEntry()
        {
            DeleteConfirmationRequest.Raise(new Confirmation
            {
                Title = "Confirm Delete",
                Content = "Are you sure you would like to delete this record?"
            },
                r =>
                {
                    if (r.Confirmed)
                    {
                        _deleting = true;
                        _rss.RemoveRecordSearch(RecordSearch.ID,
                            RecordSearch.MailingAddress.AddressID,
                            RecordSearch.BillingAddress.AddressID,
                            RecordSearch.Fee.ID);
                        _rm.RequestNavigate("ContentRegion", "RSList");
                    }
                }
            );
        }

        private void ChangeFileNum()
        {
            ChangeFileNumRequest.Raise(new ChangeICFileNumberNotification { Title = "Change the Entry's File Number" }, r =>
            {
                object[] newFileNumber = new object[4];
                if (r.Confirmed)
                {
                    RecordSearch.ICTypePrefix = r.Prefix;
                    RecordSearch.ICYear = r.Year;
                    RecordSearch.ICEnumeration = r.Enumeration;
                    RecordSearch.ICSuffix = r.Suffix;
                }
            });
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
                RecordSearch.Status = RecordSearch.CalculateStatus();
                SelectedRequestor = RecordSearch.RequestorID;
                SelectedClient = RecordSearch.ClientID;
                _isLoaded = true;
            }
        }
    }
}
