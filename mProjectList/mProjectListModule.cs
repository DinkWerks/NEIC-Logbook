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
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ProjectList>();
            containerRegistry.RegisterForNavigation<ProjectEntry>();
            containerRegistry.RegisterForNavigation<Calculator>();
        }
    }
}