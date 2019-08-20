using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using System;
using Tracker.Core.Settings;
using System.Collections.Generic;
using System.IO;
using Tracker.Core.Services;

namespace Tracker.ViewModels
{
    public class SettingsScreenViewModel : BindableBase
    {
        private List<string> _feeStructures = new List<string>();
        private IRecordSearchService _rs;
        private IPersonService _ps;
        private IFeeService _fs;
        private IClientService _cs;
        private IAddressService _as;

        public Settings Settings { get; private set; }

        public List<string> FeeStructures
        {
            get { return _feeStructures; }
            set { SetProperty(ref _feeStructures, value); }
        }

        public DelegateCommand LocateDatabaseCommand { get; private set; }
        public DelegateCommand SaveSettingsCommand { get; private set; }

        //Constructor
        public SettingsScreenViewModel(IRecordSearchService recordSearchService, IPersonService personService, IFeeService feeService, IClientService clientService, IAddressService addressService)
        {
            _rs = recordSearchService;
            _ps = personService;
            _fs = feeService;
            _cs = clientService;
            _as = addressService;
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
                _rs.SetConnectionString();
                _ps.SetConnectionString();
                _fs.SetConnectionString();
                _cs.SetConnectionString();
                _as.SetConnectionString();

                _cs.CompleteClientList = _cs.GetAllPartialClients();
                _ps.CompletePeopleList = _ps.GetAllPartialPeople();
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
