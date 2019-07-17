using Prism.Mvvm;
using Prism.Regions;
using Prism.Commands;
using Tracker.Views;

namespace Tracker.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string _title = "NEIC Record Searc Database";
        private IRegionNavigationService _ns;
        private IRegionManager _rm;

        public DelegateCommand<string> NavigateCommand { get; private set; }
        public DelegateCommand GoBackCommand { get; private set; }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _rm = regionManager;
            //regionManager.RegisterViewWithRegion("ContentRegion", typeof(RSEntry));
            regionManager.RegisterViewWithRegion("ContentRegion", typeof(HomeScreen));
            NavigateCommand = new DelegateCommand<string>(Navigate);
            GoBackCommand = new DelegateCommand(GoBack);
        }

        private void GoBack()
        {
            _ns.Journal.GoBack();
        }

        private void Navigate(string navigationTarget)
        {
            _rm.RequestNavigate("ContentRegion", navigationTarget);
        }
    }
}
