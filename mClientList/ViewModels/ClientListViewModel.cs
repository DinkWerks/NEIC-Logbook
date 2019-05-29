using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tracker.Core.StaticTypes;

namespace mClientList.ViewModels
{
    public class ClientListViewModel : BindableBase
    {
        private ObservableCollection<ClientEntryViewModel> _clientEntries = new ObservableCollection<ClientEntryViewModel>();

        public ObservableCollection<ClientEntryViewModel> ClientEntries
        {
            get { return _clientEntries; }
            set { SetProperty(ref _clientEntries, value); }
        }

        public ClientListViewModel()
        {
            AddTestClients();
        }

        private void AddTestClients()
        {
            for (int i=0; i<11; i++)
            ClientEntries.Add(new ClientEntryViewModel());
        }
    }
}
