using mStaffList.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace mStaffList
{
    public class mStaffListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StaffList>();
        }
    }
}