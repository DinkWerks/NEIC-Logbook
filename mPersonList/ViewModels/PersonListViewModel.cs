using mPersonList.Notifications;
using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Data;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mPersonList.ViewModels
{
    public class PersonListViewModel : BindableBase, IRegionMemberLifetime
    {
        private IRegionManager _rm;
        private IPersonService _ps;
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

        public InteractionRequest<INewPersonNotification> NewPersonRequest { get; private set; }
        public DelegateCommand NewClientCommand { get; private set; }
        public bool KeepAlive => false;

        //Constructor
        public PersonListViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IPersonService personService)
        {
            _rm = regionManager;
            _ps = personService;

            People = personService.GetAllPartialPeople();
            PeopleView.Refresh();
            PeopleView.Filter = PersonNameSearchFilter;

            NewPersonRequest = new InteractionRequest<INewPersonNotification>();
            NewClientCommand = new DelegateCommand(CreateNewPerson);
            eventAggregator.GetEvent<PersonListSelectEvent>().Subscribe(NavigateToPersonEntry);
        }

        //Methods
        private void NavigateToPersonEntry(int navTargetID)
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
            NewPersonRequest.Raise(new NewPersonNotification { Title = "Create a New Person Entry" }, r =>
                {
                    if (r.Confirmed)
                    {
                        int newPersonID = _ps.AddNewPerson(new Person()
                        {
                            FirstName = r.FirstName,
                            LastName = r.LastName
                        });

                        NavigateToPersonEntry(newPersonID);
                    }
                }
            );
        }

        private bool PersonNameSearchFilter(object item)
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
