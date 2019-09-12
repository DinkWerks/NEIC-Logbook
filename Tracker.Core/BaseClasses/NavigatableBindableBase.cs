using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Tracker.Core.CompositeCommands;

namespace Tracker.Core.BaseClasses
{
    public class NavigatableBindableBase : BindableBase, INavigationAware
    {
        protected IRegionNavigationJournal _journal;
        private bool _constructing;

        public DelegateCommand GoBackCommand { get; set; }
        public DelegateCommand GoForwardCommand { get; set; }

        public NavigatableBindableBase(IApplicationCommands applicationCommands)
        {
            _constructing = true;
            GoBackCommand = new DelegateCommand(GoBack);
            applicationCommands.BackCompCommand.RegisterCommand(GoBackCommand);
            GoBackCommand.IsActive = true;

            GoForwardCommand = new DelegateCommand(GoForward);
            applicationCommands.ForwardCompCommand.RegisterCommand(GoForwardCommand);
            GoForwardCommand.IsActive = true;
            _constructing = false;
        }

        private void GoBack()
        {
            if (_journal.CanGoBack && !_constructing)
                    _journal.GoBack();
        }

        private void GoForward()
        {
            if (_journal.CanGoForward)
                _journal.GoForward();
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
