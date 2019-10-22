using Microsoft.EntityFrameworkCore;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mProjectList.ViewModels
{
    public class ProjectListViewModel : BindableBase
    {
        private List<Project> _projects = new List<Project>();
        private ICollectionView _projectView;
        private string _icFilePrefix;
        private string _icFileYear;
        private string _icFileEnumeration;
        private string _projectNameSearchText;
        private IEventAggregator _ea;
        private IRegionManager _rm;
        private IDialogService _ds;

        public List<Project> Projects
        {
            get { return _projects; }
            set { SetProperty(ref _projects, value); }
        }

        public ICollectionView ProjectView
        {
            get { return _projectView; }
        }

        public string ICFilePrefixSearch
        {
            get { return _icFilePrefix; }
            set
            {
                SetProperty(ref _icFilePrefix, value);
                ProjectView.Refresh();
            }
        }

        public string ICFileYearSearch
        {
            get { return _icFileYear; }
            set
            {
                SetProperty(ref _icFileYear, value);
                ProjectView.Refresh();
            }
        }

        public string ICFileEnumerationSearch
        {
            get { return _icFileEnumeration; }
            set
            {
                SetProperty(ref _icFileEnumeration, value);
                ProjectView.Refresh();
            }
        }

        public string ProjectNameSearchText
        {
            get { return _projectNameSearchText; }
            set
            {
                SetProperty(ref _projectNameSearchText, value);
                ProjectView.Refresh();
            }
        }

        public List<Prefix> PrefixChoices { get; private set; }

        public DelegateCommand CreateNewProjectCommand { get; set; }

        //Constructor
        public ProjectListViewModel(IEventAggregator eventAggregator, IRegionManager regionManager, IDialogService dialogService)
        {
            _ea = eventAggregator;
            _rm = regionManager;
            _ds = dialogService;
            PrefixChoices = new List<Prefix>(RecordSearchPrefixes.Values);

            using (var context = new EFService())
            {
                Projects = context.Projects
                    .Include(p => p.Requestor)
                    .Include(o => o.Client)
                    .ToList();
            }

        }

        //Methods
        public void NavigateToRecordSearchEntry(int navTargetID)
        {
            var parameters = new NavigationParameters
            {
                { "id", navTargetID }
            };

            if (navTargetID >= 0)
                _rm.RequestNavigate("ContentRegion", "RSEntry", parameters);
        }

        public void CreateNewRecordSearch()
        {
            
        }

        public bool RecordSearchViewFilter(object filterable)
        {
            Project recordSearch = filterable as Project;
            if (recordSearch == null)
            {
                return false;
            }

            int passedTests = 0;

            if (ProjectNameSearchText != null)
            {
                if (recordSearch.ProjectName.ToLower().Contains(ProjectNameSearchText.ToLower()))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (ICFilePrefixSearch != null)
            {
                if (recordSearch.ICTypePrefix.ToUpper() == ICFilePrefixSearch)
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (ICFileYearSearch != null)
            {
                if (recordSearch.ICYear.ToLower().Contains(ICFileYearSearch))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            if (ICFileEnumerationSearch != null)
            {
                if (recordSearch.ICEnumeration.ToString().Contains(ICFileEnumerationSearch))
                {
                    passedTests++;
                }
                else return false;
            }
            else passedTests++;

            return passedTests >= 4;
        }
    }
}
