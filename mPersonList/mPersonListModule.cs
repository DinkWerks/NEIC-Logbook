using mPersonList.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;
using Tracker.Core.Services;
using Unity;

namespace mPersonList
{
    public class mPersonListModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            Application.Current.Resources.Add("IoC", containerProvider);
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<PersonList>();
            containerRegistry.RegisterForNavigation<PersonEntry>();
        }
    }
}