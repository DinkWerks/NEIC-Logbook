﻿using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mOrganizationList.ViewModels
{
    public class OrganizationListViewModel : BindableBase, IRegionMemberLifetime
    {
        private IRegionManager _rm;
        private IDialogService _ds;
        private ObservableCollection<Organization> _organizations = new ObservableCollection<Organization>();
        private string _orgNameSearchText;
        private string _peidSearchText;
        private string _oldPEIDSearchText;

        public ObservableCollection<Organization> Organizations
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
        public OrganizationListViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IDialogService dialogService)
        {
            _rm = regionManager;
            _ds = dialogService;

            using (var context = new EFService())
            {
                Organizations = new ObservableCollection<Organization>(context.Organizations
                    .Include(s => s.OrganizationStanding)
                    .ToList());
            }
            
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

                    using (var context = new EFService())
                    {
                        context.Add(newOrg);
                        context.SaveChanges();
                    }
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
    }
}