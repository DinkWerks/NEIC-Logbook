using mReporting.Reporting;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.Services;

namespace mReporting.ViewModels
{
    public class ReportingHomeScreenViewModel : BindableBase
    {
        private string _selectedReport = "default";

        private IRecordSearchService _rss;

        public string SelectedReport
        {
            get { return _selectedReport; }
            set { SetProperty(ref _selectedReport, value); }
        }

        public DelegateCommand TestReportCommand { get; set; }

        public ReportingHomeScreenViewModel(IRecordSearchService recordSearchService)
        {
            _rss = recordSearchService;
            TestReportCommand = new DelegateCommand(TestReport);
        }

        private void TestReport()
        {
            BillingExport report = new BillingExport(_rss);

        }
    }
}
