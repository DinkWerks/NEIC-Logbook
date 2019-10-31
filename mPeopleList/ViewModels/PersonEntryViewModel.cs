﻿using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.ObjectModel;
using System.Linq;
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
        private ObservableCollection<Organization> _orgList = new ObservableCollection<Organization>();
        private IDialogService _ds;

        public Person Person
        {
            get { return _person; }
            set { SetProperty(ref _person, value); }
        }

        public ObservableCollection<Organization> OrgList
        {
            get { return _orgList; }
            set { SetProperty(ref _orgList, value); }
        }

        public DelegateCommand NavigateToOrganizationCommand { get; set; }

        public PersonEntryViewModel(IRegionManager regionManager, IEventAggregator eventAggregator,
            IDialogService dialogService, IApplicationCommands applicationCommands) : base(applicationCommands)
        {
            _rm = regionManager;
            _ea = eventAggregator;
            _ds = dialogService;

            NavigateToOrganizationCommand = new DelegateCommand(NavigateToOrganization);

            _ea.GetEvent<ProjectListSelectEvent>().Subscribe(NavigateToProject);
        }

        //Methods
        public override void SaveEntry()
        {
            using (var context = new EFService())
            {
                var x = context.ChangeTracker.Entries();
                context.Update(Person);
                context.SaveChanges();
                _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Person entry successfully saved.", Palette.AlertGreen));
            }
        }

        public override void DeleteEntry()
        {
            _ds.Show("ConfirmationDialog",
                new DialogParameters("message=Are you sure you would like to delete this person?"),
                r =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        using (var context = new EFService())
                        {
                            context.Remove(Person);
                            context.SaveChanges();
                            _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Person entry successfully deleted.", Palette.AlertGreen));
                            _deleting = true;
                            _rm.RequestNavigate("ContentRegion", "PeopleList");
                        }
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

            using (var context = new EFService())
            {
                Person = context.People
                                .Where(p => p.ID == personID)
                                .Include(o => o.Affiliation)
                                .Include(p => p.RecentProjects)
                                    .Take(10)
                                .FirstOrDefault();
                var x = context.ChangeTracker.Entries();
                OrgList = new ObservableCollection<Organization>(context.Organizations.ToList());
           }
        }
    }
}
