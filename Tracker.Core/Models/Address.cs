using Microsoft.EntityFrameworkCore;
using Prism.Mvvm;

namespace Tracker.Core.Models
{
    [Owned]
    public class Address : BindableBase
    {
        private string _addressName;
        private string _attentionTo;
        private string _addressLine1;
        private string _addressLine2;
        private string _city;
        private string _state;
        private string _zip;
        private string _notes;


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
        
        public Address(Address address)
        {
            AddressName = address.AddressName;
            AttentionTo = address.AttentionTo;           
            AddressLine1 = address.AddressLine1;
            AddressLine2 = address.AddressLine2;
            City = address.City;
            State = address.State;
            ZIP = address.ZIP;
            Notes = address.Notes;
        }

        public bool ValidateMinimalCompleteness()
        {
            return !(string.IsNullOrWhiteSpace(AddressLine1) &&
                string.IsNullOrWhiteSpace(City) &&
                string.IsNullOrWhiteSpace(State) &&
                string.IsNullOrWhiteSpace(ZIP));
        }
    }
}
