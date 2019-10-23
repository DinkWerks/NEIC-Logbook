﻿using mFeeCalculator.Views;
using Microsoft.EntityFrameworkCore;
using mProjectList.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mProjectList.ViewModels
{
    public class ProjectEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private IEventAggregator _ea;
        private IRegionManager _rm;
        private IDialogService _ds;
        private Project _project;
        private int _selectedRequestor;
        private int _selectedClient;

        public Project Project
        {
            get { return _project; }
            set { SetProperty(ref _project, value); }
        }

        public int SelectedRequestor
        {
            get { return _selectedRequestor; }
            set
            {
                //LoadNewRequestor(value);
                SetProperty(ref _selectedRequestor, value);
            }
        }

        public int SelectedClient
        {
            get { return _selectedClient; }
            set
            {
                //LoadNewClient(value);
                SetProperty(ref _selectedClient, value);
            }
        }

        //Commands
        public DelegateCommand ChangeFileNumCommand { get; private set; }
        public DelegateCommand ChangeAdditionalCountiesCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand<string> CopyRequestorCommand { get; private set; }
        public DelegateCommand<string> CopyClientCommand { get; private set; }

        //Constructor
        public ProjectEntryViewModel(IEventAggregator eventAggregator, IDialogService dialogService, IRegionManager regionManager,
            IApplicationCommands applicationCommands) : base(applicationCommands)
        {
            _ea = eventAggregator;
            _rm = regionManager;
            _ds = dialogService;

            regionManager.RegisterViewWithRegion("RequestorAddress", typeof(AddressEntry));
            regionManager.RegisterViewWithRegion("BillingAddress", typeof(AddressEntry));
            //regionManager.RegisterViewWithRegion("CalculatorRegion", typeof(Calculator));

            ChangeFileNumCommand = new DelegateCommand(ChangeFileNum);
            ChangeAdditionalCountiesCommand = new DelegateCommand(ChangeAdditionalCounties);
            NavigateCommand = new DelegateCommand<string>(Navigate);
            CopyRequestorCommand = new DelegateCommand<string>(CopyRequestor);
            CopyClientCommand = new DelegateCommand<string>(CopyAffiliation);
        }

        //Methods
        private void Navigate(string destination)
        {
            if (destination == "Requestor")
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    { "id", SelectedRequestor }
                };
                if (SelectedClient > 0)
                    _rm.RequestNavigate("ContentRegion", "PersonEntry", parameters);
            }
            else if (destination == "Client")
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    { "id", SelectedClient }
                };
                if (SelectedClient > 0)
                    _rm.RequestNavigate("ContentRegion", "ClientEntry", parameters);
            }
        }

        private void CopyRequestor(string destination)
        {
            if (destination == "Mailing")
                CopyAddress(Project.MailingAddress, Project.Requestor.Address);
            else if (destination == "Billing")
                CopyAddress(Project.BillingAddress, Project.Requestor.Address);
        }

        private void CopyAffiliation(string destination)
        {
            if (destination == "Mailing")
                CopyAddress(Project.MailingAddress, Project.Client.Address);
            else if (destination == "Billing")
                CopyAddress(Project.BillingAddress, Project.Client.Address);
        }

        private void CopyAddress(Address toReplace, Address toCopy)
        {
            toReplace.AddressName = toCopy.AddressName;
            toReplace.AttentionTo = toCopy.AttentionTo;
            toReplace.AddressLine1 = toCopy.AddressLine1;
            toReplace.AddressLine2 = toCopy.AddressLine2;
            toReplace.City = toCopy.City;
            toReplace.State = toCopy.State;
            toReplace.ZIP = toCopy.ZIP;
            toReplace.Notes = toCopy.Notes;
        }

        //Popups
        private void ChangeFileNum()
        {
            _ds.Show("ChangeICFileDialog", null, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    Project.ICTypePrefix = r.Parameters.GetValue<string>("");
                    Project.ICYear = r.Parameters.GetValue<string>("");
                    Project.ICEnumeration = r.Parameters.GetValue<int>("");
                    Project.ICSuffix = r.Parameters.GetValue<string>("");
                }
            });
        }

        private void ChangeAdditionalCounties()
        {
            _ds.Show("ChangeAdditionalCountiesDialog", null, r =>
            {
                if (r.Result == ButtonResult.OK)
                {

                }
            });
        }

        //IO Methods
        public override void SaveEntry()
        {
            using (var context = new EFService())
            {
                context.Update(Project);
                context.SaveChanges();
                _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Project entry succesfully saved.", Palette.AlertGreen));
            }
        }

        public override void DeleteEntry()
        {
            _ds.Show("ConfirmationDialog", new DialogParameters("message=Are you sure you would like to delete this project?"), r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    using (var context = new EFService())
                    {
                        context.Remove(Project);
                        context.SaveChanges();

                        _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Project entry succesfully deleted.", Palette.AlertGreen));
                        _deleting = true;
                        _rm.RequestNavigate("ContentRegion", "ProjectList");
                    }
                }
            });
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            using(var context = new EFService())
            {
                Project = context.Projects
                    .Where(p => p.Id == (int)navigationContext.Parameters["id"])
                    .Include(p => p.Requestor)
                        .ThenInclude(r => r.Affiliation)
                    .Include(p => p.Client)
                    .FirstOrDefault();
            }
        }
    }
}