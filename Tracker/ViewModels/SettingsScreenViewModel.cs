using Microsoft.Win32;
using Prism;
using Prism.Events;
using Prism.Commands;
using Prism.Mvvm;
using System;
using Tracker.Core.Settings;
using System.Collections.Generic;
using System.IO;
using Tracker.Core;
using Tracker.Core.Services;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Prism.Regions;

namespace Tracker.ViewModels
{
    public class SettingsScreenViewModel : BindableBase, IActiveAware, INavigationAware
    {
        private List<string> _feeStructures = new List<string>();
        private readonly IEventAggregator _ea;
        private bool _isActive;
        

        public Settings Settings { get; private set; }

        public List<string> FeeStructures
        {
            get { return _feeStructures; }
            set { SetProperty(ref _feeStructures, value); }
        }

        public bool IsActive
        {
            get { return _isActive; }
            set
            {
                SetProperty(ref _isActive, value);
                OnIsActiveChanged();
            }
        }

        public DelegateCommand SaveSettingsCommand { get; private set; }
        

        public event EventHandler IsActiveChanged;
        //Constructor
        public SettingsScreenViewModel(IEventAggregator eventAggregator, IApplicationCommands applicationCommands)
        {
            _ea = eventAggregator;
            Settings = Settings.Instance;
            ListFeeStructures();

            SaveSettingsCommand = new DelegateCommand(SaveSettings);
            applicationCommands.SaveCompCommand.RegisterCommand(SaveSettingsCommand);
        }

        //Methods

        private void ListFeeStructures()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, @"Resources\FeeStructures\");
            var feeStructures = Directory.GetFiles(filePath);

            foreach (string feeStructure in feeStructures)
            {
                string fileName = Path.GetFileName(feeStructure);
                FeeStructures.Add(fileName.Remove(fileName.Length - 4));
            }
        }

        private void SaveSettings()
        {
            Settings.SaveSettings();
            _ea.GetEvent<StatusEvent>().Publish(new StatusPayload("Settings saved.", Palette.AlertGreen));
        }

        private void OnIsActiveChanged()
        {
            SaveSettingsCommand.IsActive = IsActive;
            IsActiveChanged?.Invoke(this, new EventArgs());
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            SaveSettings();
        }
    }
}
