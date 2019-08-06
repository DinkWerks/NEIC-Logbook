using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mPersonList.ViewModels
{
    public class PersonEntryViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
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

        public DelegateCommand SaveCommand { get; private set; }
        public InteractionRequest<IConfirmation> ConfirmationRequest { get; set; }
        public DelegateCommand NavigateToClientCommand { get; private set; }

        public bool KeepAlive => false;

        public PersonEntryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IPersonService personService,
            IAddressService addressService, IRecordSearchService recordSearchService, IClientService clientService)
        {
            _rm = regionManager;
            _ps = personService;
            _as = addressService;
            _rss = recordSearchService;

            ClientList = clientService.CompleteClientList;
            SaveCommand = new DelegateCommand(SavePerson);
            ConfirmationRequest = new InteractionRequest<IConfirmation>();
            NavigateToClientCommand = new DelegateCommand(NavigateToClient);
            eventAggregator.GetEvent<RecordSearchListSelectEvent>().Subscribe(NavigateToRecordSearch);
        }

        private void SavePerson()
        {
            int addressID = _as.UpdateAddress(PersonModel.AddressModel);
            PersonModel.AddressID = addressID;
            PersonModel.AddressModel.AddressID = addressID;
            PersonModel.CurrentAssociationID = SelectedClient;
            Client selectedClientModel = ClientList[SelectedClient - 1];
            if (string.IsNullOrWhiteSpace(selectedClientModel.OfficeName))
                PersonModel.CurrentAssociation = selectedClientModel.ClientName;
            else
                PersonModel.CurrentAssociation = selectedClientModel.ClientName + " - " + selectedClientModel.OfficeName;
            _ps.UpdatePersonInformation(PersonModel);
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            SavePerson();
        }
    }
}
