using mRecordSearchList.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace mRecordSearchList
{
    public class mRecordSearchListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<RSList>();
            containerRegistry.RegisterForNavigation<RSEntry>();
            containerRegistry.RegisterForNavigation<AddressEntry>();
        }
    }
}