using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Tracker.Core.StaticTypes;

namespace mClientList.ViewModels
{
    public class ClientListViewModel : BindableBase
    {
        private ObservableCollection<ClientEntryViewModel> _clientEntries = new ObservableCollection<ClientEntryViewModel>();

        private string _clientNameSearchText;
        private string _peidSearchText;

        public ObservableCollection<ClientEntryViewModel> ClientEntries
        {
            get { return _clientEntries; }
            set { SetProperty(ref _clientEntries, value); }
        }

        public string PEIDSearchText
        {
            get { return _peidSearchText; }
            set { SetProperty(ref _peidSearchText, value); }
        }

        public string ClientNameSearchText
        {
            get { return _clientNameSearchText; }
            set
            {
                SetProperty(ref _clientNameSearchText, value);
                ClientCollectionView.Refresh();
            }
        }
        public ICollectionView ClientCollectionView
        {
            get { return CollectionViewSource.GetDefaultView(ClientEntries); }
        }

        public ClientListViewModel()
        {
            AddTestClients();
            ClientCollectionView.Filter = ClientNameSearchFilter;
        }

        public bool ClientNameSearchFilter(object item)
        {
            var ClientToTest = item as ClientEntryViewModel;
            if (ClientToTest == null)
            {
                return false;
            }
            if (ClientNameSearchText == null)
            {
                return true;
            }
            if (ClientToTest.ClientName.ToLower().Contains(ClientNameSearchText.ToLower()))
            {
                return true;
            }

            return false;
        }

        private void AddTestClients()
        {
            ClientEntries.Add(new ClientEntryViewModel());

            //for (int i = 0; i < 10; i++)
            //ClientEntries.Add(new ClientEntryViewModel());
        }
    }
}
