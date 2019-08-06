using mPersonList.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace mPersonList
{
    public class mPersonListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PersonList>();
            containerRegistry.RegisterForNavigation<PersonEntry>();
        }
    }
}