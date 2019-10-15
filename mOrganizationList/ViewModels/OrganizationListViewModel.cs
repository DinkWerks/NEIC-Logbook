using Prism.Commands;
using Prism.Events;
using Prism.Interactivity.InteractionRequest;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mOrganizationList.ViewModels
{
    public class OrganizationListViewModel : BindableBase
    {
        private IRegionManager _rm;
        private IDialogService _ds;
        private List<Organization> _organizations = new List<Organization>();
        private string _orgNameSearchText;
        private string _peidSearchText;
        private string _oldPEIDSearchText;

        public List<Organization> Organizations
        {
            get { return _organizations; }
            set { SetProperty(ref _organizations, value); }
        }

        public ICollectionView OrgView
        {
            get { return CollectionViewSource.GetDefaultView(Organizations); }
        }

        public string PEIDSearchText
        {
            get { return _peidSearchText; }
            set
            {
                SetProperty(ref _peidSearchText, value);
                OrgView.Refresh();
            }
        }

        public string OldPEIDSearchText
        {
            get { return _oldPEIDSearchText; }
            set
            {
                SetProperty(ref _oldPEIDSearchText, value);
                OrgView.Refresh();
            }
        }

        public string OrgNameSearchText
        {
            get { return _orgNameSearchText; }
            set
            {
                SetProperty(ref _orgNameSearchText, value);
                OrgView.Refresh();
            }
        }
        public DelegateCommand NewOrganizationCommand { get; private set; }
        public bool KeepAlive => false;

        //Constructor
        public OrganizationListViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IEFService efService, IDialogService dialogService)
        {
            _rm = regionManager;
            _ds = dialogService;

            Organizations = efService.Organizations.ToList();
            OrgView.Filter = ClientNameSearchFilter;

            NewOrganizationCommand = new DelegateCommand(CreateNewOrganization);
            eventAggregator.GetEvent<ClientListSelectEvent>().Subscribe(NavigateToClientEntry);
        }

        //Methods
        private void CreateNewOrganization()
        {
            _ds.Show("NewOrganizationDialog", new DialogParameters("OrgName="), r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    string newName = r.Parameters.GetValue<string>("OrgName");
                    using (var context = new EFService())
                    {
                        context.Add(new Organization
                        {
                            OrganizationName = newName,
                            OrganizationStanding = OrganizationStandings.GoodStanding
                        });
                        context.SaveChanges();

                        //And Navigate Here
                    }
                }
            }
            );
        }

        public void NavigateToClientEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID >= 0)
                _rm.RequestNavigate("ContentRegion", "OrganizationEntry", parameters);
        }

        public bool ClientNameSearchFilter(object item)
        {
            Client client = item as Client;
            if (client == null)
            {
                return false;
            }

            int passedTests = 0;

            if (!string.IsNullOrWhiteSpace(OrgNameSearchText))
            {
                if (client.ClientName.ToLower().Contains(OrgNameSearchText.ToLower()))
                    passedTests++;
                else
                    return false;
            }
            else passedTests++;

            if (!string.IsNullOrWhiteSpace(PEIDSearchText))
            {
                if (!string.IsNullOrWhiteSpace(client.NewPEID) && client.NewPEID.ToLower().Contains(PEIDSearchText.ToLower()))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (!string.IsNullOrWhiteSpace(OldPEIDSearchText))
            {
                if (!string.IsNullOrWhiteSpace(client.OldPEID) && client.OldPEID.ToLower().Contains(OldPEIDSearchText.ToLower()))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            return passedTests >= 3;
        }
    }
}
