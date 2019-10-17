using mOrganizationList.ViewModels;
using mOrganizationList.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace mOrganizationList
{
    public class mOrganizationListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<OrganizationList>();
            containerRegistry.RegisterForNavigation<OrganizationEntry>();
        }
    }
}