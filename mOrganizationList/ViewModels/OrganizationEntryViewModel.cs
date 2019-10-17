using Microsoft.EntityFrameworkCore;
using Prism.Events;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Linq;
using Tracker.Core;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mOrganizationList.ViewModels
{
    public class OrganizationEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private readonly IEFService _ef;
        private readonly IEventAggregator _ea;
        private readonly IDialogService _ds;
        private readonly IRegionManager _rm;
        private Organization _organization;

        public Organization Organization
        {
            get { return _organization; }
            set { SetProperty(ref _organization, value); }
        }

        //Constructor
        public OrganizationEntryViewModel(IEFService efService, IEventAggregator eventAggregator, IDialogService dialogService,
            IRegionManager regionManager, IApplicationCommands applicationCommands) : base(applicationCommands)
        {
            _ef = efService;
            _ea = eventAggregator;
            _ds = dialogService;
            _rm = regionManager;

            eventAggregator.GetEvent<PersonListSelectEvent>().Subscribe(NavigateToPersonEntry);
        }

        //Methods
        public override void SaveEntry()
        {
            using (var context = new EFService())
            {
                context.Update(Organization);
                context.SaveChanges();
                _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Client entry successfully saved.", Palette.AlertGreen));
            }
        }

        public override void DeleteEntry()
        {
            _ds.Show("ConfirmationDialog",
                new DialogParameters("message=Are you sure you would like to delete this organization?"),
                r =>
                {
                    if (r.Result == ButtonResult.OK)
                    {
                        using (var context = new EFService())
                        {
                            context.Remove(Organization);
                            context.SaveChanges();
                            _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Client entry successfully deleted.", Palette.AlertGreen));
                            _deleting = true;
                            _rm.RequestNavigate("ContentRegion", "OrganizationList");
                        }
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
            Organization = _ef.Organizations
                .Where(o => o.ID == (int)navigationContext.Parameters["id"])
                .Include(s => s.OrganizationStanding)
                .Include(e => e.Employees)
                .FirstOrDefault();
        }
    }
}
