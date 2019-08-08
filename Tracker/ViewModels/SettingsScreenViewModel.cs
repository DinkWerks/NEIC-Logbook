using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using Tracker.Core.Settings;
using System.Collections.Generic;
using System.IO;

namespace Tracker.ViewModels
{
    public class SettingsScreenViewModel : BindableBase
    {
        private List<string> _feeStructures = new List<string>();

        public Settings Settings { get; private set; }

        public List<string> FeeStructures
        {
            get { return _feeStructures; }
            set { SetProperty(ref _feeStructures, value); }
        }

        public DelegateCommand LocateDatabaseCommand { get; private set; }
        public DelegateCommand SaveSettingsCommand { get; private set; }
        
        //Constructor
        public SettingsScreenViewModel()
        {
            Settings = Settings.Instance;
            ListFeeStructures();

            LocateDatabaseCommand = new DelegateCommand(LocateDatabase);
            SaveSettingsCommand = new DelegateCommand(SaveSettings);
        }

        //Methods
        private void LocateDatabase()
        {
            OpenFileDialog selectFile = new OpenFileDialog
            {
                Filter = "Access databases (*.mdb;*.accdb)|*.mdb;*.accdb|All files (*.*)|*.*",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyComputer)
            };

            if (selectFile.ShowDialog() == true)
            {
                Settings.DatabaseAddress.Value = selectFile.FileName;
            }
        }

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
        }
    }
}
