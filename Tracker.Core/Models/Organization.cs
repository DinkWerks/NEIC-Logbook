using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Models
{
    public class Organization : BindableBase
    {
        private int _id;
        private string _oldPEID;
        private string _newPEID;
        private string _clientName;
        private string _officeName;
        private string _phone;
        private string _email;
        private string _website;
        private ClientStanding _standing;
        private int _addressID;
        private Address _addressModel;
        private string _notes;

        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string OldPEID
        {
            get { return _oldPEID; }
            set { SetProperty(ref _oldPEID, value); }
        }

        public string NewPEID
        {
            get { return _newPEID; }
            set { SetProperty(ref _newPEID, value); }
        }

        public string ClientName
        {
            get { return _clientName; }
            set { SetProperty(ref _clientName, value); }
        }

        public string OfficeName
        {
            get { return _officeName; }
            set { SetProperty(ref _officeName, value); }
        }

        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string Website
        {
            get { return _website; }
            set { SetProperty(ref _website, value); }
        }

        public ClientStanding Standing
        {
            get { return _standing; }
            set { SetProperty(ref _standing, value); }
        }

        public int AddressID
        {
            get { return _addressID; }
            set { SetProperty(ref _addressID, value); }
        }

        public Address AddressModel
        {
            get { return _addressModel; }
            set { SetProperty(ref _addressModel, value); }
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
            if (string.IsNullOrWhiteSpace(OfficeName))
                return ClientName;
            else
                return ClientName + " - " + OfficeName;
        }
    }
}
