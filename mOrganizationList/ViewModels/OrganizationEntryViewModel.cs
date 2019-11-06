using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mOrganizationList.ViewModels
{
    public class OrganizationEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private readonly IEventAggregator _ea;
        private readonly IDialogService _ds;
        private readonly IRegionManager _rm;
        private readonly IOrganizationService _os;
        private readonly IEFService _ef;
        private Organization _organization;
        private List<OrganizationStanding> _organizationStandings = new List<OrganizationStanding>();

        public Organization Organization
        {
            get { return _organization; }
            set { SetProperty(ref _organization, value); }
        }

        public List<OrganizationStanding> OrganizationStandings
        {
            get { return _organizationStandings; }
            set { SetProperty(ref _organizationStandings, value); }
        }

        //Constructor
        public OrganizationEntryViewModel(IEventAggregator eventAggregator, IDialogService dialogService,
            IRegionManager regionManager, IApplicationCommands applicationCommands, IOrganizationService organizationService,
            IEFService eFService) : base(applicationCommands)
        {
            _ea = eventAggregator;
            _ds = dialogService;
            _rm = regionManager;
            _os = organizationService;
            _ef = eFService;

            OrganizationStandings = _ef.OrganizationStandings.ToList();
            eventAggregator.GetEvent<PersonListSelectEvent>().Subscribe(NavigateToPersonEntry);
        }

        //Methods
        public override void SaveEntry()
        {
            _os.UpdateOrganization(Organization);
            _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Organization entry successfully saved.", Palette.AlertGreen));
        }

        public override void DeleteEntry()
        {
            _ds.Show("ConfirmationDialog",
                new DialogParameters("message=Are you sure you would like to delete this organization?"),
                r =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        _os.DeleteOrganization(Organization);
                        _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Client entry successfully deleted.", Palette.AlertGreen));
                        _deleting = true;
                        _rm.RequestNavigate("ContentRegion", "OrganizationList");
                    }
                });
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            int id = (int)navigationContext.Parameters["id"];
            Organization = _os.GetOrganization(id);
        }

        public new void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (!_deleting)
                SaveCommand.Execute();
            Organization = null;
        }
    }
}
