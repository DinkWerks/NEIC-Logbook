using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Tracker.ViewModels
{
    public class HomeScreenViewModel : BindableBase, INavigationAware
    {
        private IRegionManager _rm;
        private IRegionNavigationJournal _journal;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public HomeScreenViewModel(IRegionManager regionManager)
        {
            _rm = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigationTarget)
        {
            _rm.RequestNavigate("ContentRegion", navigationTarget);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _journal = navigationContext.NavigationService.Journal;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            
        }
    }
}
