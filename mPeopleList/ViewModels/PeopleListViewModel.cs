using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mPeopleList.ViewModels
{
    public class PeopleListViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager _rm;
        private readonly IDialogService _ds;
        private readonly IPersonService _ps;
        private List<Person> _people = new List<Person>();
        private ICollectionView _peopleView;
        private string _personNameSearchText;
        private string _affilliationSearchText;
        private bool _firstRun = true;

        public List<Person> People
        {
            get { return _people; }
            set { SetProperty(ref _people, value); }
        }

        public ICollectionView PeopleView
        {
            get { return _peopleView; }
            set { SetProperty(ref _peopleView, value); }
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

        public DelegateCommand NewPersonCommand { get; private set; }

        //Constructor
        public PeopleListViewModel(IRegionManager regionManager, IDialogService dialogService, IEventAggregator eventAggregator, IPersonService personService)
        {
            _rm = regionManager;
            _ds = dialogService;
            _ps = personService;

            People = _ps.GetPeople();
            PeopleView = CollectionViewSource.GetDefaultView(People);
            PeopleView.Filter = PersonNameSearchFilter;

            NewPersonCommand = new DelegateCommand(CreateNewPerson);
            eventAggregator.GetEvent<PersonListSelectEvent>().Subscribe(NavigateToPeopleEntry);
        }

        //Methods
        private void NavigateToPeopleEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID > 0)
                _rm.RequestNavigate("ContentRegion", "PersonEntry", parameters);
        }

        private void CreateNewPerson()
        {
            _ds.Show("NewPersonDialog", new DialogParameters(), r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    Person newPerson = new Person()
                    {
                        FirstName = r.Parameters.GetValue<string>("fname"),
                        LastName = r.Parameters.GetValue<string>("lname"),
                        Address = new Address()
                    };

                    _ps.AddPerson(newPerson);

                    NavigateToPeopleEntry(newPerson.ID);
                }
            });
        }

        private bool PersonNameSearchFilter(object item)
        {
            Person person = item as Person;
            if (person == null)
            {
                return false;
            }

            string name = person.FirstName.ToLower() + " " + person.LastName.ToLower();
            string affiliation = string.Empty;
            if (person.Affiliation != null)
                affiliation = person.Affiliation.OrganizationName.ToLower();

            if (PersonNameSearchText != null && AffiliationSearchText != null)
            {
                if (name.Contains(PersonNameSearchText.ToLower()) && !string.IsNullOrWhiteSpace(affiliation) && affiliation.Contains(AffiliationSearchText.ToLower()))
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
                if (!string.IsNullOrWhiteSpace(affiliation) && affiliation.Contains(AffiliationSearchText.ToLower()))
                {
                    return true;
                }
                return false;
            }
            return true;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (_firstRun)
            {
                _firstRun = false;
            }
            else
            {
                People = _ps.GetPeople(tracking: false);
                PeopleView = CollectionViewSource.GetDefaultView(People);
                PeopleView.Filter = PersonNameSearchFilter;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
