using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tracker.Core;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Events.CustomPayloads;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mPersonList.ViewModels
{
    public class PersonEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private IRegionManager _rm;
        private IEventAggregator _ea;
        private IPersonService _ps;
        private IClientService _cs;
        private IAddressService _as;
        private IRecordSearchService _rss;
        private Person _person;
        private ObservableCollection<RecordSearch> _recordSearches = new ObservableCollection<RecordSearch>();
        private Client _selectedClient;

        public List<Client> ClientList { get; set; }

        private int _initialClient;
        public int InitialClient
        {
            get { return _initialClient; }
            set { SetProperty(ref _initialClient, value); }
        }

        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { SetProperty(ref _selectedClient, value); }
        }

        public Person PersonModel
        {
            get { return _person; }
            set
            {
                SetProperty(ref _person, value);
            }
        }

        public ObservableCollection<RecordSearch> RecordSearches
        {
            get { return _recordSearches; }
            set
            {
                SetProperty(ref _recordSearches, value);
            }
        }

        public DelegateCommand NavigateToClientCommand { get; private set; }

        public InteractionRequest<IConfirmation> DeleteConfirmationRequest { get; set; }

        //Constructor
        public PersonEntryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IPersonService personService,
            IAddressService addressService, IRecordSearchService recordSearchService, IClientService clientService,
            IApplicationCommands applicationCommands) : base(applicationCommands)
        {
            _rm = regionManager;
            _ea = eventAggregator;
            _ps = personService;
            _cs = clientService;
            _as = addressService;
            _rss = recordSearchService;

            ClientList = clientService.CompleteClientList;

            SaveCommand = new DelegateCommand(SaveEntry);
            applicationCommands.SaveCompCommand.RegisterCommand(SaveCommand);
            DeleteCommand = new DelegateCommand(DeleteEntry);
            applicationCommands.DeleteCompCommand.RegisterCommand(DeleteCommand);

            NavigateToClientCommand = new DelegateCommand(NavigateToClient);
            DeleteConfirmationRequest = new InteractionRequest<IConfirmation>();
            eventAggregator.GetEvent<RecordSearchListSelectEvent>().Subscribe(NavigateToRecordSearch);
        }

        //Methods
        public override void SaveEntry()
        {
            int addressID = _as.UpdateAddress(PersonModel.AddressModel);
            PersonModel.AddressID = addressID;
            PersonModel.AddressModel.AddressID = addressID;

            //Add in Person's Affiliation
            if (SelectedClient != null)
            {
                PersonModel.CurrentAssociationID = SelectedClient.ID;
                PersonModel.CurrentAssociation = SelectedClient.ToString();
            }

            _ps.UpdatePersonInformation(PersonModel);
            _ea.GetEvent<StatusUpdateEvent>().Publish(new StatusPayload("Person entry successfully saved.", Palette.AlertGreen));
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
                       _ps.RemovePerson(PersonModel.ID, PersonModel.AddressModel.AddressID);
                       _rm.RequestNavigate("ContentRegion", "PersonList");
                   }
               }
           );
        }

        private void NavigateToClient()
        {
            var parameters = new NavigationParameters
            {
                { "id", SelectedClient.ID }
            };

            if (SelectedClient.ID > 0)
                _rm.RequestNavigate("ContentRegion", "ClientEntry", parameters);
        }

        private void NavigateToRecordSearch(int navTarget)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTarget }
            };

            if (navTarget > 0)
                _rm.RequestNavigate("ContentRegion", "RSEntry", parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            int personID = (int)navigationContext.Parameters["id"];
            if (personID > 0)
            {
                PersonModel = _ps.GetPersonByID(personID);
                SelectedClient = _cs.GetClientByID(PersonModel.CurrentAssociationID);
                InitialClient = ClientList.FindIndex(c => c.ToString() == SelectedClient.ToString());
                PersonModel.AddressModel = _as.GetAddressByID(PersonModel.AddressID);
                RecordSearches = new ObservableCollection<RecordSearch>(
                    _rss.GetPartialRecordSearchesByCriteria("WHERE RequestorID = " + personID)
                    );
            }
        }
    }
}
