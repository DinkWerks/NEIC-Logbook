using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mClientList.ViewModels
{
    public class ClientEntryViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private IRegionManager _rm;
        private IClientService _cs;
        private IPersonService _ps;
        private ObservableCollection<Person> _associates = new ObservableCollection<Person>();
        private Client _client;
        private bool _deleting = false;

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

        public bool KeepAlive => false;

        //Constructor
        public ClientEntryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IClientService clientService, IPersonService personService)
        {
            _rm = regionManager;
            _cs = clientService;
            _ps = personService;
            
            DeleteEntryCommand = new DelegateCommand(DeleteEntry);

            DeleteConfirmationRequest = new InteractionRequest<IConfirmation>();
            eventAggregator.GetEvent<PersonListSelectEvent>().Subscribe(NavigateToPersonEntry);
        }

        //Methods
        private void Save()
        {
            _cs.UpdateClientInformation(ClientModel);
        }

        public void DeleteEntry()
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

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
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

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if(!_deleting)
                Save();
        }
    }
}
