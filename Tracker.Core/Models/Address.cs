using Microsoft.EntityFrameworkCore;
using Prism.Mvvm;
using System.ComponentModel.DataAnnotations.Schema;

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
        private bool _updated;

        public string AddressName
        {
            get { return _addressName; }
            set
            {
                SetProperty(ref _addressName, value);
                Updated = true;
            }
        }

        public string AttentionTo
        {
            get { return _attentionTo; }
            set
            {
                SetProperty(ref _attentionTo, value);
                Updated = true;
            }
        }

        public string AddressLine1
        {
            get { return _addressLine1; }
            set
            {
                SetProperty(ref _addressLine1, value);
                Updated = true;
            }
        }

        public string AddressLine2
        {
            get { return _addressLine2; }
            set
            {
                SetProperty(ref _addressLine2, value);
                Updated = true;
            }
        }

        public string City
        {
            get { return _city; }
            set
            {
                SetProperty(ref _city, value);
                Updated = true;
            }
        }

        public string State
        {
            get { return _state; }
            set
            {
                SetProperty(ref _state, value);
                Updated = true;
            }
        }

        public string ZIP
        {
            get { return _zip; }
            set
            {
                SetProperty(ref _zip, value);
                Updated = true;
            }
        }

        public string Notes
        {
            get { return _notes; }
            set
            {
                SetProperty(ref _notes, value);
                Updated = true;
            }
        }

        [NotMapped]
        public bool Updated
        {
            get { return _updated; }
            set { SetProperty(ref _updated, value); }
        }
        
        public Address()
        {

        }

        public Address(Address address)
        {
            if (address != null)
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
        }

        public bool ValidateMinimalCompleteness()
        {
            return !(string.IsNullOrWhiteSpace(AddressLine1) &&
                string.IsNullOrWhiteSpace(City) &&
                string.IsNullOrWhiteSpace(State) &&
                string.IsNullOrWhiteSpace(ZIP)
                );
        }
    }
}
