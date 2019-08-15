using System;
using Prism;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Tracker.Core.CompositeCommands;

namespace Tracker.Core.BaseClasses
{
    // TODO Verify that this should be abstract.
    public abstract class RecordEntryBindableBase : BindableBase, IActiveAware, IRegionMemberLifetime
    {
        private bool _isActive;
        private IApplicationCommands _applicationCommands;
        protected bool _deleting = false;


        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetProperty(ref _isActive, value);
                OnIsActiveChanged();
            }
        }

        //Commands
        public DelegateCommand SaveCommand { get; protected set; }
        public DelegateCommand DeleteCommand { get; protected set; }

        public bool KeepAlive => false;

        public event EventHandler IsActiveChanged;

        //Constructor
        public RecordEntryBindableBase(IApplicationCommands applicationCommands)
        {
            _applicationCommands = applicationCommands;

            SaveCommand = new DelegateCommand(SaveEntry);
            applicationCommands.SaveCompCommand.RegisterCommand(SaveCommand);
            DeleteCommand = new DelegateCommand(DeleteEntry);
            applicationCommands.DeleteCompCommand.RegisterCommand(DeleteCommand);
        }

        //Methods
        public abstract void SaveEntry();

        public abstract void DeleteEntry();

        private void OnIsActiveChanged()
        {
            SaveCommand.IsActive = IsActive;
            DeleteCommand.IsActive = IsActive;
            IsActiveChanged?.Invoke(this, new EventArgs());
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            if (!_deleting)
                SaveCommand.Execute();
            _applicationCommands.SaveCompCommand.UnregisterCommand(SaveCommand);
            _applicationCommands.DeleteCompCommand.UnregisterCommand(DeleteCommand);
        }
    }
}
