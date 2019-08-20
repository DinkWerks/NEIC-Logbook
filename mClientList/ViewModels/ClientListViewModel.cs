using mClientList.Notifications;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mClientList.ViewModels
{
    public class ClientListViewModel : BindableBase, IRegionMemberLifetime
    {
        private IRegionManager _rm;
        private IClientService _cs;
        private List<Client> _clients = new List<Client>();
        private string _clientNameSearchText;
        private string _peidSearchText;
        private string _oldPEIDSearchText;

        public List<Client> Clients
        {
            get { return _clients; }
            set { SetProperty(ref _clients, value); }
        }

        public ICollectionView ClientView
        {
            get { return CollectionViewSource.GetDefaultView(Clients); }
        }

        public string PEIDSearchText
        {
            get { return _peidSearchText; }
            set
            {
                SetProperty(ref _peidSearchText, value);
                ClientView.Refresh();
            }
        }
        
        public string OldPEIDSearchText
        {
            get { return _oldPEIDSearchText; }
            set { SetProperty(ref _oldPEIDSearchText, value); }
        }

        public string ClientNameSearchText
        {
            get { return _clientNameSearchText; }
            set
            {
                SetProperty(ref _clientNameSearchText, value);
                ClientView.Refresh();
            }
        }

        public InteractionRequest<ICreateNewClientNotification> NewClientRequest { get; private set; }
        public DelegateCommand NewClientCommand { get; private set; }
        public bool KeepAlive => false;

        //Constructor
        public ClientListViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IClientService clientService)
        {
            _rm = regionManager;
            _cs = clientService;

            Clients = _cs.GetAllPartialClients();
            ClientView.Filter = ClientNameSearchFilter;


            NewClientRequest = new InteractionRequest<ICreateNewClientNotification>();
            NewClientCommand = new DelegateCommand(CreateNewClient);
            eventAggregator.GetEvent<ClientListSelectEvent>().Subscribe(NavigateToClientEntry);
        }

        //Methods
        private void CreateNewClient()
        {
            NewClientRequest.Raise(new CreateNewClientNotification { Title = "Create a New Client Entry" }, r =>
                {
                    if (r.Confirmed)
                    {
                        int newClientID = _cs.AddNewClient(new Client()
                        {
                            ClientName = r.ClientName,
                            OfficeName = r.OfficeName,
                        });

                        NavigateToClientEntry(newClientID);
                    }
                }
            );
        }

        public void NavigateToClientEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID >= 0)
                _rm.RequestNavigate("ContentRegion", "ClientEntry", parameters);
        }

        public bool ClientNameSearchFilter(object item)
        {
            Client client = item as Client;
            if (client == null)
            {
                return false;
            }

            int passedTests = 0;

            if (ClientNameSearchText != null)
            {
                if (client.ClientName.ToLower().Contains(ClientNameSearchText.ToLower()))
                    passedTests++;
                else
                    return false;
            }
            else passedTests++;

            if (PEIDSearchText != null)
            {
                if (client.NewPEID.ToLower().Contains(PEIDSearchText.ToLower()))
                    passedTests++;
                else
                    return false;
            }
            else passedTests++;

            if (OldPEIDSearchText != null)
            {
                if (client.OldPEID.ToLower().Contains(OldPEIDSearchText.ToLower()))
                    passedTests++;
                else
                    return false;
            }
            else passedTests++;

            return passedTests >= 3;
        }
    }
}
