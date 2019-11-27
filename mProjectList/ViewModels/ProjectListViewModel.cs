using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using Tracker.Core.DTO;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mProjectList.ViewModels
{
    public class ProjectListViewModel : BindableBase, INavigationAware
    {
        private List<ProjectListDTO> _projects = new List<ProjectListDTO>();
        private ICollectionView _projectView;
        private string _icFilePrefix;
        private string _icFileYear;
        private string _icFileEnumeration;
        private string _projectNameSearchText;
        private IEventAggregator _ea;
        private IRegionManager _rm;
        private IDialogService _ds;
        private IProjectService _ps;
        private bool _firstRun = false;
        private string _statusSearch;
        private int _filterCount;

        public List<ProjectListDTO> Projects
        {
            get { return _projects; }
            set { SetProperty(ref _projects, value); }
        }

        public ICollectionView ProjectView
        {
            get { return _projectView; }
            set { SetProperty(ref _projectView, value); }
        }

        public string ICFilePrefixSearch
        {
            get { return _icFilePrefix; }
            set
            {
                SetProperty(ref _icFilePrefix, value);
                ProjectView.Refresh();
                UpdateFilterCount();
            }
        }

        public string ICFileYearSearch
        {
            get { return _icFileYear; }
            set
            {
                SetProperty(ref _icFileYear, value);
                ProjectView.Refresh();
                UpdateFilterCount();
            }
        }

        public string ICFileEnumerationSearch
        {
            get { return _icFileEnumeration; }
            set
            {
                SetProperty(ref _icFileEnumeration, value);
                ProjectView.Refresh();
                UpdateFilterCount();
            }
        }

        public string ProjectNameSearchText
        {
            get { return _projectNameSearchText; }
            set
            {
                SetProperty(ref _projectNameSearchText, value);
                ProjectView.Refresh();
                UpdateFilterCount();
            }
        }

        public string StatusSearch
        {
            get { return _statusSearch; }
            set
            {
                SetProperty(ref _statusSearch, value);
                ProjectView.Refresh();
                UpdateFilterCount();
            }
        }

        public int FilterCount
        {
            get { return _filterCount; }
            set { SetProperty(ref _filterCount, value); }
        }

        public List<Prefix> PrefixChoices { get; private set; }
        public string[] Statuses {get; private set;}
        public DelegateCommand CreateNewProjectCommand { get; set; }

        //Constructor
        public ProjectListViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IProjectService projectService, IDialogService dialogService)
        {
            _ea = eventAggregator;
            _rm = regionManager;
            _ds = dialogService;
            _ps = projectService;
            PrefixChoices = new List<Prefix>(ProjectPrefixes.Values);

            //Projects = _ps.GetAllProjects(tracking: false);
            Projects = _ps.GetProjectListDTOs();
            ProjectView = CollectionViewSource.GetDefaultView(Projects);
            ProjectView.Filter = ProjectViewFilter;

            Statuses = new string[]
            {
                "",
                "Complete",
                "Awaiting Payment",
                "Overdue Payment",
                "Awaiting Billing",
                "Awaiting Response",
                "Overdue Response",
                "Entered"
            };

            CreateNewProjectCommand = new DelegateCommand(CreateNewProject);
            eventAggregator.GetEvent<ProjectListSelectEvent>().Subscribe(NavigateToProjectEntry);
        }

        //Methods
        public void NavigateToProjectEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID >= 0)
                _rm.RequestNavigate("ContentRegion", "ProjectEntry", parameters);
        }

        public void CreateNewProject()
        {
            _ds.Show("NewProjectDialog", null, r =>
            {
                if (r.Result == ButtonResult.OK)
                {
                    Project newProject = new Project()
                    {
                        ICTypePrefix = r.Parameters.GetValue<Prefix>("prefix").ToString(),
                        ICYear = r.Parameters.GetValue<string>("year"),
                        ICEnumeration = r.Parameters.GetValue<int>("enumeration"),
                        ICSuffix = r.Parameters.GetValue<string>("suffix"),
                        ProjectName = r.Parameters.GetValue<string>("pname"),
                        MailingAddress = new Address(),
                        BillingAddress = new Address(),
                        FeeData = new FeeData()
                    };

                    _ps.AddProject(newProject);
                    NavigateToProjectEntry(newProject.Id);
                }
            });
        }

        public bool ProjectViewFilter(object filterable)
        {
            ProjectListDTO project = filterable as ProjectListDTO;
            if (project == null)
            {
                return false;
            }

            int passedTests = 0;

            if (ProjectNameSearchText != null)
            {
                if (project.ProjectName.ToLower().Contains(ProjectNameSearchText.ToLower()))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (ICFilePrefixSearch != null)
            {
                if (project.ICTypePrefix.ToUpper() == ICFilePrefixSearch)
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (ICFileYearSearch != null)
            {
                if (project.ICYear.ToLower().Contains(ICFileYearSearch))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (ICFileEnumerationSearch != null)
            {
                if (project.ICEnumeration.ToString().Contains(ICFileEnumerationSearch))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (!string.IsNullOrWhiteSpace(StatusSearch))
            {
                if (project.Status == StatusSearch)
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            return passedTests >= 5;
        }

        public void UpdateFilterCount()
        {
            FilterCount = ProjectView.Cast<object>().Count();
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (_firstRun)
            {
                _firstRun = false;
            }
            else
            {
                //Projects = _ps.GetAllProjects(tracking: false);
                Projects = _ps.GetProjectListDTOs();
                ProjectView = CollectionViewSource.GetDefaultView(Projects);
                ProjectView.Filter = ProjectViewFilter;
            }
            FilterCount = Projects.Count();
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
