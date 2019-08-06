using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mClientList.ViewModels
{
    public class ClientEntryViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _rm;
        private IClientService _cs;
        private IPersonService _ps;
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

        public DelegateCommand<string> NavigateCommand { get; private set; }

        //Constructor
        public ClientEntryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IClientService clientService, IPersonService personService)
        {
            _rm = regionManager;
            _cs = clientService;
            _ps = personService;
            
            NavigateCommand = new DelegateCommand<string>(Navigate);

            eventAggregator.GetEvent<PersonListSelectEvent>().Subscribe(NavigateToPersonEntry);
        }

        //Methods
        private void Save()
        {
            _cs.UpdateClientInformation(ClientModel);
        }

        public void Navigate(string navTarget)
        {
            _rm.RequestNavigate("ContentRegion", "ClientList");
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
            //TODO What is this for? Yoinked from the Passing Parameters sample.
            Client client = navigationContext.Parameters["client"] as Client;
            if (client != null)
            {
                return ClientModel != null && ClientModel.ClientName == client.ClientName;
            }
            else
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
            Save();
        }
    }
}
