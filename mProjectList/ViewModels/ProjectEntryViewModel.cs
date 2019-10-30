using Microsoft.EntityFrameworkCore;
using mProjectList.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tracker.Core;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mProjectList.ViewModels
{
    public class ProjectEntryViewModel : RecordEntryBindableBase, INavigationAware
    {
        private readonly IEventAggregator _ea;
        private readonly IRegionManager _rm;
        private readonly IDialogService _ds;
        private readonly IContainerExtension _container;
        private Project _project;
        private List<Person> _requestorList;
        private List<Organization> _clientList;
        private List<Staff> _staffList;
        private int _selectedRequestor;
        private int _selectedClient;
        private bool _firstRun = true;
        private Calculator _calc;

        public Project Project
        {
            get { return _project; }
            set { SetProperty(ref _project, value); }
        }

        public List<Person> RequestorList
        {
            get { return _requestorList; }
            set { SetProperty(ref _requestorList, value); }
        }

        public List<Organization> ClientList
        {
            get { return _clientList; }
            set { SetProperty(ref _clientList, value); }
        }

        public List<Staff> StaffList
        {
            get { return _staffList; }
            set { SetProperty(ref _staffList, value); }
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
        public DelegateCommand RequestorChangedCommand { get; private set; }
        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand<string> CopyRequestorCommand { get; private set; }
        public DelegateCommand<string> CopyClientCommand { get; private set; }

        //Constructor
        public ProjectEntryViewModel(IEventAggregator eventAggregator, IDialogService dialogService, IRegionManager regionManager,
            IApplicationCommands applicationCommands, IContainerExtension container) : base(applicationCommands)
        {
            _ea = eventAggregator;
            _rm = regionManager;
            _ds = dialogService;
            _container = container;

            regionManager.RegisterViewWithRegion("RequestorAddress", typeof(AddressEntry));
            regionManager.RegisterViewWithRegion("BillingAddress", typeof(AddressEntry));
            
            ChangeFileNumCommand = new DelegateCommand(ChangeFileNum);
            ChangeAdditionalCountiesCommand = new DelegateCommand(ChangeAdditionalCounties);
            RequestorChangedCommand = new DelegateCommand(RequestorChanged);
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
                    { "id", Project.Requestor.ID }
                };
                if (Project.Requestor.ID > 0)
                    _rm.RequestNavigate("ContentRegion", "PersonEntry", parameters);
            }
            else if (destination == "Client")
            {
                NavigationParameters parameters = new NavigationParameters
                {
                    { "id", Project.Client.ID }
                };
                if (Project.Client.ID > 0)
                    _rm.RequestNavigate("ContentRegion", "OrganizationEntry", parameters);
            }
        }

        private void CopyRequestor(string destination)
        {
            if (Project.Requestor != null)
            {
                if (destination == "Mailing")
                    Project.MailingAddress = Project.Requestor.Address;
                else if (destination == "Billing")
                    Project.BillingAddress = Project.Requestor.Address;
            }
        }

        private void CopyAffiliation(string destination)
        {
            if (Project.Client != null)
            {
                if (destination == "Mailing")
                    Project.MailingAddress = Project.Client.Address;
                else if (destination == "Billing")
                    Project.BillingAddress = Project.Client.Address;
            }
        }

        private void RequestorChanged()
        {
            if (Project.Requestor != null && Project.Client == null)
                Project.Client = Project.Requestor.Affiliation;
        }

        //Popups
        private void ChangeFileNum()
        {
            _ds.Show("ChangeICFileDialog", null, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    Project.ICTypePrefix = r.Parameters.GetValue<string>("prefix");
                    Project.ICYear = r.Parameters.GetValue<string>("year");
                    Project.ICEnumeration = r.Parameters.GetValue<int>("enumeration");
                    Project.ICSuffix = r.Parameters.GetValue<string>("suffix");
                }
            });
        }

        private void ChangeAdditionalCounties()
        {
            DialogParameters currentCounties = new DialogParameters();
            currentCounties.Add("selected", Project.AdditionalCounties);

            _ds.Show("ChangeAdditionalCountiesDialog", currentCounties, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    Project.AdditionalCounties = r.Parameters.GetValue<ObservableCollection<County>>("counties");
                }
            });
        }

        //IO Methods
        public override void SaveEntry()
        {
            using (var context = new EFService())
            {
                Project.FeeData = ((CalculatorViewModel)_calc.DataContext).Fee.FeeData;
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
            if (_firstRun)
            {
                _calc = _container.Resolve<Calculator>();
                IRegion region = _rm.Regions["CalculatorRegion"];
                region.Add(_calc);
                _firstRun = false;
            }
            
            using (var context = new EFService())
            {
                Project = context.Projects
                    .Where(p => p.Id == (int)navigationContext.Parameters["id"])
                    .Include(p => p.Requestor)
                        .ThenInclude(r => r.Affiliation)
                    .Include(p => p.Client)
                    .Include(p => p.Processor)
                    .Include(p => p.FeeData)
                    .FirstOrDefault();

                FeeX projectFee = new FeeX(Project.FeeVersion, Project.FeeData ?? new FeeData());
                _ea.GetEvent<ProjectEntryChangedEvent>().Publish(projectFee);

                RequestorList = context.People.OrderBy(s => s.LastName).ToList();
                ClientList = context.Organizations.OrderBy(s => s.OrganizationName).ToList();
                StaffList = context.Staff.OrderBy(s => s.Name).ToList();
            }
        }
    }
}
