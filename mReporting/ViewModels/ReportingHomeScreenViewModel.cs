using mReporting.Reporting;
using Prism.Commands;
using Prism.Mvvm;
using System.Reflection;
using System.Collections.Generic;
using Tracker.Core.Services;
using System;
using System.Linq;

namespace mReporting.ViewModels
{
    public class ReportingHomeScreenViewModel : BindableBase
    {
        private string _selectedReport = "default";
        private List<IReport> _reports = new List<IReport>();
        private IReport _report;
        private IRecordSearchService _rss;

        public List<IReport>Reports
        {
            get { return _reports; }
            set { SetProperty(ref _reports, value); }
        }

        public IReport SelectedReport
        {
            get { return _report; }
            set { SetProperty(ref _report, value); }
        }

        public List<Type> ReportTypes { get; set; }

        public DelegateCommand TestReportCommand { get; set; }
        
        //Constructor
        public ReportingHomeScreenViewModel(IRecordSearchService recordSearchService)
        {
            _rss = recordSearchService;
            
            TestReportCommand = new DelegateCommand(TestReport);
            GetReportsFromNamespace(Assembly.GetExecutingAssembly(), "mReporting.Reporting");
            
        }

        //Methods
        private void GetReportsFromNamespace(Assembly assembly, string nameSpace)
        {
            List<Type> reportTypes = assembly.GetTypes().Where(t => t.IsClass && string.Equals(t.Namespace, nameSpace, StringComparison.Ordinal)).ToList();
            object[] repParams = new object[] { _rss };
            foreach (Type t in reportTypes)
            {
                IReport instance = (IReport)Activator.CreateInstance(t, repParams);
                Reports.Add(instance);
            }
        }

        private void TestReport()
        {
            //BillingExport report = new BillingExport(_rss);
            if (SelectedReport != null)
                SelectedReport.Execute(null);
        }
    }
}
