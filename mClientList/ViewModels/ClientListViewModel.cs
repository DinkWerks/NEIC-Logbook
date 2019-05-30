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
        public CET = CollectionViewSource.GetDefaultView();
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
            set { SetProperty(ref _clientNameSearchText, value); }
        }

        public ClientListViewModel()
        {
            AddTestClients();
        }

        private void FilterClientNames(object obj)
        {

        }

        private void AddTestClients()
        {
            for (int i = 0; i < 11; i++)
                ClientEntries.Add(new ClientEntryViewModel());
        }
    }
}
