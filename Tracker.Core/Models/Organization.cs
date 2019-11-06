using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Models
{
    public class Organization : BindableBase
    {
        private int _id;
        private string _oldPEID;
        private string _newPEID;
        private string _organizationName;
        private string _phone;
        private string _email;
        private string _website;
        private int? _organizationStandingId;
        private OrganizationStanding _standing;
        private int _addressID;
        private Address _address;
        private string _notes;

        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [MaxLength(10)]
        public string OldPEID
        {
            get { return _oldPEID; }
            set { SetProperty(ref _oldPEID, value); }
        }

        [MaxLength(10)]
        public string NewPEID
        {
            get { return _newPEID; }
            set { SetProperty(ref _newPEID, value); }
        }

        [MaxLength(200)]
        public string OrganizationName
        {
            get { return _organizationName; }
            set { SetProperty(ref _organizationName, value); }
        }

        [MaxLength(15)]
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        [MaxLength(200)]
        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        [MaxLength(200)]
        public string Website
        {
            get { return _website; }
            set { SetProperty(ref _website, value); }
        }

        public int? OrganizationStandingId
        {
            get { return _organizationStandingId; }
            set { SetProperty(ref _organizationStandingId, value); }
        }

        public OrganizationStanding OrganizationStanding
        {
            get { return _standing; }
            set { SetProperty(ref _standing, value); }
        }

        public int AddressID
        {
            get { return _addressID; }
            set { SetProperty(ref _addressID, value); }
        }

        public Address Address
        {
            get { return _address; }
            set { SetProperty(ref _address, value); }
        }

        public string Notes
        {
            get { return _notes; }
            set { SetProperty(ref _notes, value); }
        }

        public ICollection<Person> Employees { get; set; }

        public Organization()
        {

        }

        public override string ToString()
        {
            return OrganizationName;
        }
    }
}
