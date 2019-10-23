using mDialogs.ViewModels;
using mDialogs.Views;
using Prism.Ioc;
using Prism.Modularity;

namespace mDialogs
{
    public class mDialogsModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterDialog<ConfirmationDialog, ConfirmationDialogViewModel>();
            containerRegistry.RegisterDialog<NewOrganizationDialog, NewOrganizationDialogViewModel>();
            containerRegistry.RegisterDialog<NewPersonDialog, NewPersonDialogViewModel>();
            containerRegistry.RegisterDialog<NewProjectDialog, NewProjectDialogViewModel>();
        }
    }
}