using Prism.Commands;
using Prism.Regions;
using Tracker.Core.BaseClasses;
using Tracker.Core.CompositeCommands;

namespace Tracker.ViewModels
{
    public class HomeScreenViewModel : NavigatableBindableBase, INavigationAware
    {
        private IRegionManager _rm;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        //Constructor
        public HomeScreenViewModel(IRegionManager regionManager, IApplicationCommands applicationCommands) : base(applicationCommands)
        {
            _rm = regionManager;

            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        //Methods
        private void Navigate(string navigationTarget)
        {
            _rm.RequestNavigate("ContentRegion", navigationTarget);
        }
    }
}
