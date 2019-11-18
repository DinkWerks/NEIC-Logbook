using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using Tracker.Core;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mPeopleList.ViewModels
{
    public class PersonEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private IRegionManager _rm;
        private IEventAggregator _ea;
        private Person _person;
        private List<Organization> _orgList = new List<Organization>();
        private IDialogService _ds;
        private IPersonService _ps;
        private IOrganizationService _os;

        public Person Person
        {
            get { return _person; }
            set { SetProperty(ref _person, value); }
        }

        public List<Organization> OrgList
        {
            get { return _orgList; }
            set { SetProperty(ref _orgList, value); }
        }

        public DelegateCommand NavigateToOrganizationCommand { get; set; }

        public PersonEntryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
            IDialogService dialogService, IApplicationCommands applicationCommands, IPersonService personService, IOrganizationService organizationService) : base(applicationCommands)
        {
            _rm = regionManager;
            _ea = eventAggregator;
            _ds = dialogService;
            _ps = personService;
            _os = organizationService;

            NavigateToOrganizationCommand = new DelegateCommand(NavigateToOrganization);

            _ea.GetEvent<ProjectListSelectEvent>().Subscribe(NavigateToProject);
        }

        //Methods
        public override void SaveEntry()
        {
            _ps.UpdatePerson(Person);
            _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Person entry successfully saved.", Palette.AlertGreen));
        }

        public override void DeleteEntry()
        {
            _ds.Show("ConfirmationDialog",
                new DialogParameters("message=Are you sure you would like to delete this person?"),
                r =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        _ps.DeletePerson(Person);
                        _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Person entry successfully deleted.", Palette.AlertGreen));
                        _deleting = true;
                        _rm.RequestNavigate("ContentRegion", "PeopleList");
                    }
                });
        }

        private void NavigateToOrganization()
        {
            var parameters = new NavigationParameters
            {
                { "id", Person.Affiliation.ID }
            };

            if (Person.Affiliation.ID > 0)
                _rm.RequestNavigate("ContentRegion", "OrganizationEntry", parameters);
        }

        private void NavigateToProject(int navTarget)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTarget }
            };

            if (navTarget > 0)
                _rm.RequestNavigate("ContentRegion", "ProjectEntry", parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            int personID = (int)navigationContext.Parameters["id"];
            Person = _ps.GetPersonFull(personID);
            OrgList = _os.GetAllOrganizations();
        }


        public new void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (!_deleting)
                SaveCommand.Execute();
            Person = null;
            OrgList = null;
        }
    }
}
