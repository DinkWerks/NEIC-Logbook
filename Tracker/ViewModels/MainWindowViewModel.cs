using Prism.Mvvm;
using Prism.Regions;
using Prism.Commands;
using Tracker.Views;

namespace Tracker.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "NEIC Logbook";
        private IRegionManager _rm;
        private IRegionNavigationJournal _journal;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }
        public DelegateCommand GoBackCommand { get; set; }
        public MainWindowViewModel(IRegionManager regionManager, RegionNavigationService regionNavigationService)
        {
            _rm = regionManager;
            _journal = regionNavigationService.Journal;
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(HomeScreen));
            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
        }

        private void GoBack()
        {
            _journal.GoBack();
        }
        private void Navigate(string navigationTarget)
        {
            _rm.RequestNavigate("ContentRegion", navigationTarget);
        }
    }
}
