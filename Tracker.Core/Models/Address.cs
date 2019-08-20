using Prism.Mvvm;

namespace Tracker.Core.Models
{
    public class Address : BindableBase
    {
        private int _addressID;
        private string _addressName;
        private string _attentionTo;
        private string _addressLine1;
        private string _addressLine2;
        private string _city;
        private string _state;
        private string _zip;
        private string _notes;

        public int AddressID
        {
            get { return _addressID; }
            set { SetProperty(ref _addressID, value); }
        }

        public string AddressName
        {
            get { return _addressName; }
            set { SetProperty(ref _addressName, value); }
        }

        public string AttentionTo
        {
            get { return _attentionTo; }
            set { SetProperty(ref _attentionTo, value); }
        }

        public string AddressLine1
        {
            get { return _addressLine1; }
            set { SetProperty(ref _addressLine1, value); }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            set { SetProperty(ref _addressLine2, value); }
        }

        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        public string ZIP
        {
            get { return _zip; }
            set { SetProperty(ref _zip, value); }
        }

        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public Address()
        {

        }

        public Address(int id, Address oldAddress)
        {
            AddressID = id;
            AddressName = oldAddress.AddressName;
            AttentionTo = oldAddress.AttentionTo;
            AddressLine1 = oldAddress.AddressLine1;
            AddressLine2 = oldAddress.AddressLine2;
            City = oldAddress.City;
            State = oldAddress.State;
            ZIP = oldAddress.ZIP;
            Notes = oldAddress.Notes;
        }
    }
}
