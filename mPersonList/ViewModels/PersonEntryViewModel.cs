using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mPersonList.ViewModels
{
    public class PersonEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private IRegionManager _rm;
        private IPersonService _ps;
        private IAddressService _as;
        private IRecordSearchService _rss;
        private Person _person;
        private ObservableCollection<RecordSearch> _recordSearches = new ObservableCollection<RecordSearch>();
        private int _selectedClient;

        public List<Client> ClientList { get; set; }

        public int SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                SetProperty(ref _selectedClient, value);
            }
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
            _ps = personService;
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
            PersonModel.CurrentAssociationID = SelectedClient;
            if (SelectedClient > 0)
            {
                Client selectedClientModel = ClientList[SelectedClient - 1];
                if (string.IsNullOrWhiteSpace(selectedClientModel.OfficeName))
                    PersonModel.CurrentAssociation = selectedClientModel.ClientName;
                else
                    PersonModel.CurrentAssociation = selectedClientModel.ClientName + " - " + selectedClientModel.OfficeName;
            }
            _ps.UpdatePersonInformation(PersonModel);
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
                { "id", SelectedClient }
            };

            if (SelectedClient > 0)
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
                SelectedClient = PersonModel.CurrentAssociationID;
                PersonModel.AddressModel = _as.GetAddressByID(PersonModel.AddressID);
                RecordSearches = new ObservableCollection<RecordSearch>(
                    _rss.GetPartialRecordSearchesByCriteria("WHERE RequestorID = " + personID)
                    );
            }
        }
    }
}
