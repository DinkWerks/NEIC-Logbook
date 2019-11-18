using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Data;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mOrganizationList.ViewModels
{
    public class OrganizationListViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _rm;
        private IDialogService _ds;
        private IOrganizationService _os;
        private List<Organization> _organizations = new List<Organization>();
        private ICollectionView _orgView;
        private string _orgNameSearchText;
        private string _peidSearchText;
        private string _oldPEIDSearchText;
        private bool _firstRun = true;

        public List<Organization> Organizations
        {
            get { return _organizations; }
            set { SetProperty(ref _organizations, value); }
        }

        public ICollectionView OrgView
        {
            get { return CollectionViewSource.GetDefaultView(Organizations); }
            set { SetProperty(ref _orgView, value); }
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

        //Constructor
        public OrganizationListViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IDialogService dialogService, IOrganizationService organizationService)
        {
            _rm = regionManager;
            _ds = dialogService;
            _os = organizationService;

            Organizations = _os.GetAllOrganizations();
            OrgView = CollectionViewSource.GetDefaultView(Organizations);
            OrgView.Filter = OrgNameSearchFilter;

            NewOrganizationCommand = new DelegateCommand(CreateNewOrganization);
            eventAggregator.GetEvent<OrgListSelectEvent>().Subscribe(NavigateToOrgEntry);
        }

        //Methods
        private void CreateNewOrganization()
        {
            _ds.Show("NewOrganizationDialog", new DialogParameters("OrgName="), r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    string newName = r.Parameters.GetValue<string>("OrgName");
                    Organization newOrg = new Organization
                    {
                        OrganizationName = newName,
                        Address = new Address()
                    };

                    _os.AddOrganization(newOrg);
                    NavigateToOrgEntry(newOrg.ID);
                }
            });
        }

        public void NavigateToOrgEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID >= 0)
                _rm.RequestNavigate("ContentRegion", "OrganizationEntry", parameters);
        }

        public bool OrgNameSearchFilter(object item)
        {
            Organization org = item as Organization;
            if (org == null)
            {
                return false;
            }

            int passedTests = 0;

            if (!string.IsNullOrWhiteSpace(OrgNameSearchText))
            {
                if (org.OrganizationName.ToLower().Contains(OrgNameSearchText.ToLower()))
                    passedTests++;
                else
                    return false;
            }
            else passedTests++;

            if (!string.IsNullOrWhiteSpace(PEIDSearchText))
            {
                if (!string.IsNullOrWhiteSpace(org.NewPEID) && org.NewPEID.ToLower().Contains(PEIDSearchText.ToLower()))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (!string.IsNullOrWhiteSpace(OldPEIDSearchText))
            {
                if (!string.IsNullOrWhiteSpace(org.OldPEID) && org.OldPEID.ToLower().Contains(OldPEIDSearchText.ToLower()))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            return passedTests >= 3;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (_firstRun)
            {
                _firstRun = false;
            }
            else
            {
                Organizations = _os.GetAllOrganizations();
                OrgView = CollectionViewSource.GetDefaultView(Organizations);
                OrgView.Filter = OrgNameSearchFilter;
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
