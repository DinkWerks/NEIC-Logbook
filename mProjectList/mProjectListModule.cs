using mProjectList.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace mProjectList
{
    public class mProjectListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            //TODO Move Home Screen to it's own module and do this there.
            //Calls for the navigation of the home screen after initialization of the main window.
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RequestNavigate("ContentRegion", "HomeScreen");
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ProjectList>();
            containerRegistry.RegisterForNavigation<ProjectEntry>();
            containerRegistry.Register<Calculator>();
        }
    }
}