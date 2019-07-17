﻿using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Tracker.Core.Events;
using Tracker.Core.StaticTypes;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mClientList.ViewModels
{
    public class ClientListViewModel : BindableBase
    {
        private IRegionManager _rm;
        private IClientService _cs;
        private List<Client> _clients = new List<Client>();
        private string _clientNameSearchText;
        private string _peidSearchText;

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

        public string ClientNameSearchText
        {
            get { return _clientNameSearchText; }
            set
            {
                SetProperty(ref _clientNameSearchText, value);
                ClientView.Refresh();
            }
        }

        public ClientListViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IClientService clientService)
        {
            _rm = regionManager;
            _cs = clientService;

            Clients = _cs.GetAllPartialClients();
            ClientView.Filter=ClientNameSearchFilter;

            eventAggregator.GetEvent<ClientListSelectEvent>().Subscribe(NavigateToClientEntry);
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
            if (ClientNameSearchText != null && PEIDSearchText != null)
            {
                if (client.ClientName.ToLower().Contains(ClientNameSearchText.ToLower()) && client.OldPEID.ToString().Contains(PEIDSearchText.ToLower()))
                {
                    return true;
                }
                return false;
            }
            else if (ClientNameSearchText != null && PEIDSearchText == null)
            {
                if (client.ClientName.ToLower().Contains(ClientNameSearchText.ToLower()))
                {
                    return true;
                }
                return false;
            }
            else if (ClientNameSearchText == null && PEIDSearchText != null)
            {
                if (client.OldPEID.ToString().Contains(PEIDSearchText.ToLower()))
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
