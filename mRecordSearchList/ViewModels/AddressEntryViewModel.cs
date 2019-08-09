using Prism.Mvvm;
using Tracker.Core.Models;

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
