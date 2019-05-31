using mClientList.Views;
using mClientList.ViewModels;
using mClientList.Interfaces;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace mClientList
{
    public class mClientListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<ClientList>();
            containerRegistry.Register<ClientEntry>();
        }
    }
}