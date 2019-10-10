using Prism.Interactivity.InteractionRequest;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Tracker.Core.Models;
using Tracker.Core.BaseClasses;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Events;
using Tracker.Core.Services;
using Tracker.Core.CompositeCommands;
using mFeeCalculator.Views;
using mRecordSearchList.Views;
using mRecordSearchList.Notifications;
using Tracker.Core;
using Tracker.Core.StaticTypes;
using System.Collections.ObjectModel;
using System.Windows.Data;
using System.Linq;

namespace mRecordSearchList.ViewModels
{
    public class RSEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private readonly IEventAggregator _ea;
        private readonly IRegionManager _rm;
        private readonly IRecordSearchService _rss;
        private readonly IPersonService _ps;
        private readonly IClientService _cs;
        private readonly IStaffService _ss;
        private ObservableCollection<Person> _peopleList;
        private ObservableCollection<Client> _clientList;
        private ObservableCollection<Staff> _staffList;
        private ObservableCollection<ProjectNumber> _activeProjectNumbers;
        private RecordSearch _recordSearch;
        private int _selectedRequestor;
        private int _selectedClient;
        private bool _isLoaded = false;
        private ProjectNumber _selectedProjectNumber;


        public ObservableCollection<Person> PeopleList
        {
            get { return _peopleList; }
            set { SetProperty(ref _peopleList, value); }
        }

        public ObservableCollection<Client> ClientList
        {
            get { return _clientList; }
            set { SetProperty(ref _clientList, value); }
        }

        public ObservableCollection<Staff> StaffList
        {
            get { return _staffList; }
            set { SetProperty(ref _staffList, value); }
        }

        public ObservableCollection<ProjectNumber> ActiveProjectNumbers
        {
            get { return _activeProjectNumbers; }
            set { SetProperty(ref _activeProjectNumbers, value); }
        }

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

        public ProjectNumber SelectedProjectNumber
        {
            get { return _selectedProjectNumber; }
            set { SetProperty(ref _selectedProjectNumber, value); }
        }

        //Commands
        public DelegateCommand ChangeFileNumCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand CountySelectPopupCommand { get; private set; }
        public DelegateCommand<string> CopyRequestorCommand { get; private set; }
        public DelegateCommand<string> CopyClientCommand { get; private set; }

        //Requests
        public InteractionRequest<IChangeICFileNumberNotification> ChangeFileNumRequest { get; set; }
        public InteractionRequest<IAdditionalCountyNotification> CountySelectRequest { get; set; }
        public InteractionRequest<IConfirmation> DeleteConfirmationRequest { get; set; }

        // Constructor
        public RSEntryViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IPersonService personService, IClientService clientService,
            IRecordSearchService recordSearchService, IStaffService staffService, IApplicationCommands applicationCommands) : base(applicationCommands)
        {
            _ea = eventAggregator;
            _rm = regionManager;
            _rss = recordSearchService;
            _ps = personService;
            _cs = clientService;
            _ss = staffService;

            regionManager.RegisterViewWithRegion("RequestorAddress", typeof(AddressEntry));
            regionManager.RegisterViewWithRegion("BillingAddress", typeof(AddressEntry));
            regionManager.RegisterViewWithRegion("CalculatorRegion", typeof(Calculator));

            ChangeFileNumCommand = new DelegateCommand(ChangeFileNum);
            NavigateCommand = new DelegateCommand<string>(Navigate);
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
            _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Project entry succesfully saved.", Palette.AlertGreen));
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
                        _ea.GetEvent<RSListModifiedEvent>().Publish(new ListModificationPayload("delete", RecordSearch.ID));
                        _rss.RemoveRecordSearch(RecordSearch.ID,
                            RecordSearch.MailingAddress.Id,
                            RecordSearch.BillingAddress.Id,
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
                NavigationParameters parameters = new NavigationParameters
                {
                    { "id", SelectedRequestor }
                };
                if (SelectedClient > 0)
                    _rm.RequestNavigate("ContentRegion", "PersonEntry", parameters);
            }
            else if (destination == "Client")
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    { "id", SelectedClient }
                };
                if (SelectedClient > 0)
                    _rm.RequestNavigate("ContentRegion", "ClientEntry", parameters);
            }
        }

        private void CopyRequestor(string destination)
        {
            if (destination == "Mailing")
                RecordSearch.MailingAddress = CopyAddress(RecordSearch.MailingAddress, RecordSearch.Requestor.Address);
            else if (destination == "Billing")
                RecordSearch.BillingAddress = CopyAddress(RecordSearch.BillingAddress, RecordSearch.Requestor.Address);
        }

        private void CopyAffiliation(string destination)
        {
            if (destination == "Mailing")
                RecordSearch.MailingAddress = CopyAddress(RecordSearch.MailingAddress, RecordSearch.ClientModel.AddressModel);
            else if (destination == "Billing")
                RecordSearch.MailingAddress = CopyAddress(RecordSearch.BillingAddress, RecordSearch.ClientModel.AddressModel);
        }

        private Address CopyAddress(Address toReplace, Address toCopy)
        {
            try
            {
                return new Address()
                {
                    Id = toReplace.Id,
                    AddressName = toReplace.AddressName,
                    AttentionTo = toReplace.AttentionTo,
                    AddressLine1 = toReplace.AddressLine1,
                    AddressLine2 = toReplace.AddressLine2,
                    City = toReplace.City,
                    State = toReplace.State,
                    ZIP = toReplace.ZIP,
                    Notes = toReplace.Notes,
                };
            }
            catch
            {
                return toReplace;
            }
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
            _isLoaded = false;
            int rsID = (int)navigationContext.Parameters["id"];
            if (rsID > 0)
            {
                _rss.GetRecordSearchByID(rsID, true);
                RecordSearch = _rss.CurrentRecordSearch;
                RecordSearch.Status = RecordSearch.CalculateStatus();

                //Set dropdown menus here
                PeopleList = new ObservableCollection<Person>(_ps.CompletePeopleList);
                ClientList = new ObservableCollection<Client>(_cs.CompleteClientList);
                StaffList = new ObservableCollection<Staff>(_ss.CompleteStaffList.OrderBy(s => s.Name));
                ActiveProjectNumbers = new ObservableCollection<ProjectNumber>(ProjectNumbers.ActiveProjectNumbers);

                //Sets the Dropdown menu selection for requestor and client
                SelectedRequestor = RecordSearch.RequestorID;
                SelectedClient = RecordSearch.ClientID;
                SelectedProjectNumber = RecordSearch.ProjectNumber;
                ActiveProjectNumbers = new ObservableCollection<ProjectNumber>(ProjectNumbers.ActiveProjectNumbers);

                _isLoaded = true;
                _ea.GetEvent<RSEntryChangedEvent>().Publish();
            }
        }
    }
}
