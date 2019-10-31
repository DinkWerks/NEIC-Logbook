using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Tracker.Core.Models
{
    public class Person : BindableBase
    {
        private int _id;
        private string _firstName;
        private string _lastName;
        private Organization _affiliation;
        private string _phone1;
        private string _phone2;
        private string _email;
        private string _disclosureLevel;
        private string _note;
        private int _addressID;
        private Address _address;

        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        public Organization Affiliation
        {
            get { return _affiliation; }
            set { SetProperty(ref _affiliation, value); }
        }

        public string Phone1
        {
            get { return _phone1; }
            set { SetProperty(ref _phone1, value); }
        }

        public string Phone2
        {
            get { return _phone2; }
            set { SetProperty(ref _phone2, value); }
        }

        public string Email
        {
            get { return _email; }
            set { SetProperty(ref _email, value); }
        }

        public string DisclosureLevel
        {
            get { return _disclosureLevel; }
            set { SetProperty(ref _disclosureLevel, value); }
        }

        public string Note
        {
            get { return _note; }
            set { SetProperty(ref _note, value); }
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

        public ICollection<Project> RecentProjects { get; set; }

        public Person()
        {

        }

        public override string ToString()
        {
            return FirstName + " " + LastName;
        }
    }
}
