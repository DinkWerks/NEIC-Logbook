using mReporting.Reporting;
using Prism.Commands;
using Prism.Mvvm;
using System.Reflection;
using System.Collections.Generic;
using Tracker.Core.Services;
using System;
using System.Linq;
using Prism.Regions;
using System.Collections.ObjectModel;

namespace mReporting.ViewModels
{
    public class ReportingHomeScreenViewModel : BindableBase, IRegionMemberLifetime
    {
        private List<IReport> _ohpReports = new List<IReport>();
        private List<IReport> _billingReport = new List<IReport>();
        private ObservableCollection<object> _parameterPayload = new ObservableCollection<object>();
        private IReport _report;
        private IRecordSearchService _rss;
        private IRegionManager _rm;

        public List<IReport> OHPReports
        {
            get { return _ohpReports; }
            set { SetProperty(ref _ohpReports, value); }
        }

        public List<IReport> BillingReports
        {
            get { return _billingReport; }
            set { SetProperty(ref _billingReport, value); }
        }

        public IReport SelectedReport
        {
            get { return _report; }
            set
            {
                SetProperty(ref _report, value);
                AddParameter();
            }
        }

        public ObservableCollection<object> ParameterPayload
        {
            get { return _parameterPayload; }
            set { SetProperty(ref _parameterPayload, value); }
        }

        public List<Type> ReportTypes { get; set; }
        public DelegateCommand ExecuteReportCommand { get; set; }
        public bool KeepAlive => false;

        //Constructor
        public ReportingHomeScreenViewModel(IRecordSearchService recordSearchService, IRegionManager regionManager)
        {
            _rss = recordSearchService;
            _rm = regionManager;

            ExecuteReportCommand = new DelegateCommand(ExecuteReport);
            GetReportsFromNamespace(Assembly.GetExecutingAssembly(), "mReporting.Reporting");
        }

        //Methods
        private void GetReportsFromNamespace(Assembly assembly, string nameSpace)
        {
            List<Type> reportTypes = assembly.GetTypes().Where(t => t.IsClass && string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToList();
            object[] repParams = new object[] { _rss };
            foreach (Type t in reportTypes)
            {
                if (t.IsPublic)
                {
                    IReport instance = (IReport)Activator.CreateInstance(t, repParams);

                    switch (instance.Category)
                    {
                        case ReportCategories.Billing:
                            BillingReports.Add(instance);
                            break;
                        case ReportCategories.OHP:
                            OHPReports.Add(instance);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void ExecuteReport()
        {
            if (SelectedReport != null)
                SelectedReport.Execute(ParameterPayload);

        }

        private void AddParameter()
        {
            if (SelectedReport.Parameters != null)
            {
                string navigationTarget = "";
                switch (SelectedReport.Parameters)
                {
                    case ParameterTypes.Date_Range:
                        navigationTarget = "DateParameters";
                        break;
                    default:
                        break;
                }

                _rm.RequestNavigate("ParameterRegion", navigationTarget);
            }
        }
    }
}
