using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Tracker.Core.CompositeCommands;

namespace Tracker.Core.BaseClasses
{
    public class NavigatableBindableBase : BindableBase
    {
        private IRegionNavigationJournal _journal;

        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand ForwardCommand { get; private set; }

        public NavigatableBindableBase(IApplicationCommands applicationCommands)
        {
            BackCommand = new DelegateCommand(GoBack);
            applicationCommands.BackCompCommand.RegisterCommand(BackCommand);
            ForwardCommand = new DelegateCommand(GoForward);
            applicationCommands.ForwardCompCommand.RegisterCommand(ForwardCommand);
        }

        private void GoBack()
        {
            _journal.GoBack();
        }

        private void GoForward()
        {
            _journal.GoForward();
        }
    }
}
