using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mRecordSearchList.ViewModels
{
    public class AddressEntryViewModel : BindableBase
    {
        private Address _address;
        public Address Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }
        public AddressEntryViewModel()
        {
        }
    }
}
