using mPeopleList.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace mPeopleList
{
    public class mPeopleListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PeopleList>();
            containerRegistry.RegisterForNavigation<PersonEntry>();
        }
    }
}