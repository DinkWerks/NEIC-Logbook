using mReporting.Reporting;
using mReporting.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace mReporting
{
    public class mReportingModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ReportingHomeScreen>();
        }
    }
}