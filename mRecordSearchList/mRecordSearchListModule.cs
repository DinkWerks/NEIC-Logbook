using mRecordSearchList.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace mRecordSearchList
{
    public class mRecordSearchListModule : IModule
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
            containerRegistry.RegisterForNavigation<RSList>();
            containerRegistry.RegisterForNavigation<RSEntry>();
            containerRegistry.RegisterForNavigation<AddressEntry>();
        }
    }
}