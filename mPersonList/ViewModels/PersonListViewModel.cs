using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mPersonList.ViewModels
{
    public class PersonListViewModel : BindableBase
    {
        private IRegionManager _rm;
        private List<Person> _people = new List<Person>();
        private string _personNameSearchText;
        private string _affilliationSearchText;

        public List<Person> People
        {
            get { return _people; }
            set { SetProperty(ref _people, value); }
        }

        public ICollectionView PeopleView
        {
            get { return CollectionViewSource.GetDefaultView(People); }
        }

        public string PersonNameSearchText
        {
            get { return _personNameSearchText; }
            set
            {
                SetProperty(ref _personNameSearchText, value);
                PeopleView.Refresh();
            }
        }

        public string AffiliationSearchText
        {
            get { return _affilliationSearchText; }
            set
            {
                SetProperty(ref _affilliationSearchText, value);
                PeopleView.Refresh();
            }
        }

        public PersonListViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IPersonService personService)
        {
            _rm = regionManager;

            People = personService.GetAllPartialPeople();
            PeopleView.Refresh();
            PeopleView.Filter = PersonNameSearchFilter;

            eventAggregator.GetEvent<PersonListSelectEvent>().Subscribe(NavigateToPersonEntry);
        }

        public void NavigateToPersonEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID > 0)
                _rm.RequestNavigate("ContentRegion", "PersonEntry", parameters);
        }

        public bool PersonNameSearchFilter(object item)
        {
            Person person = item as Person;
            if (person == null)
            {
                return false;
            }

            string name = person.FirstName.ToLower() + " " + person.LastName.ToLower();
            string affiliation = person.CurrentAssociationID.ToString();
            if (PersonNameSearchText != null && AffiliationSearchText != null)
            {
                if (name.Contains(PersonNameSearchText.ToLower()) && affiliation.Contains(AffiliationSearchText.ToLower()))
                {
                    return true;
                }
                return false;
            }
            else if (PersonNameSearchText != null && AffiliationSearchText == null)
            {
                if (name.Contains(PersonNameSearchText.ToLower()))
                {
                    return true;
                }
                return false;
            }
            else if (PersonNameSearchText == null && AffiliationSearchText != null)
            {
                if (affiliation.Contains(AffiliationSearchText.ToLower()))
                {
                    return true;
                }
                return false;
            }
            return true;
        }
    }
}
