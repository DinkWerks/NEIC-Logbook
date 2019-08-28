using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Regions;
using System.Collections.ObjectModel;
using Tracker.Core;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mClientList.ViewModels
{
    public class ClientEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private IRegionManager _rm;
        private IEventAggregator _ea;
        private IClientService _cs;
        private IPersonService _ps;
        private IAddressService _as;
        private ObservableCollection<Person> _associates = new ObservableCollection<Person>();
        private Client _client;

        public ObservableCollection<Person> Associates
        {
            get { return _associates; }
            set { SetProperty(ref _associates, value);  }
        }

        public Client ClientModel
        {
            get { return _client; }
            set { SetProperty(ref _client, value); }
        }

        public DelegateCommand DeleteEntryCommand { get; private set; }

        public InteractionRequest<IConfirmation> DeleteConfirmationRequest { get; set; }

        //Constructor
        public ClientEntryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IClientService clientService, 
            IPersonService personService, IAddressService addressService, IApplicationCommands applicationCommands) : base(applicationCommands)
        {
            _rm = regionManager;
            _ea = eventAggregator;
            _cs = clientService;
            _ps = personService;
            _as = addressService;

            DeleteEntryCommand = new DelegateCommand(DeleteEntry);

            DeleteConfirmationRequest = new InteractionRequest<IConfirmation>();
            eventAggregator.GetEvent<PersonListSelectEvent>().Subscribe(NavigateToPersonEntry);
        }

        //Methods
        public override void SaveEntry()
        {
            int addressID = _as.UpdateAddress(ClientModel.AddressModel);
            ClientModel.AddressID = addressID;
            ClientModel.AddressModel.AddressID = addressID;
            _cs.UpdateClientInformation(ClientModel);
            _ea.GetEvent<StatusUpdateEvent>().Publish(new StatusPayload("Client entry successfully saved.", Palette.AlertGreen));
        }

        public override void DeleteEntry()
        {
            DeleteConfirmationRequest.Raise(new Confirmation {
                Title = "Confirm Delete",
                Content = "Are you sure you would like to delete this record?"
            }, 
                r =>
                {
                    if (r.Confirmed)
                    {
                        _deleting = true;
                        _cs.RemoveClient(ClientModel.ID, ClientModel.AddressModel.AddressID);
                        _rm.RequestNavigate("ContentRegion", "ClientList");
                    }
                }
            );
        }

        public void NavigateToPersonEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID > 0 )
                _rm.RequestNavigate("ContentRegion", "PersonEntry", parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            int clientID = (int)navigationContext.Parameters["id"];
            if (clientID > 0)
            {
                ClientModel = _cs.GetClientByID(clientID);
                Associates = new ObservableCollection<Person>(_ps.GetPartialPeopleByCriteria("WHERE CurrentAssociationID = " + clientID));
            }
        }
    }
}
