using mFeeCalculator.Reports;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Models.Fees;
using Tracker.Core.Services;

namespace mFeeCalculator.ViewModels
{
    public class CalculatorViewModel : BindableBase
    {
        private ObservableCollection<FeeSchedule> _versions = new ObservableCollection<FeeSchedule>();
        private ObservableCollection<ICharge> _charges = new ObservableCollection<ICharge>();
        private string _icFileNum;
        private Fee _fee;
        private FeeSchedule _selectedVersion;
        private IRecordSearchService _rs;
        private bool _isLoaded = false;

        public ObservableCollection<FeeSchedule> Versions
        {
            get { return _versions; }
            set { SetProperty(ref _versions, value); }
        }

        public FeeSchedule SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                SetProperty(ref _selectedVersion, value);
                //Clears all fee data if the RS Version is changed, keeps the same ID
                if (_isLoaded)
                {
                    int oldFeeID = FeeModel.ID;
                    FeeModel = new Fee(value.Version) { ID = oldFeeID };
                    _rs.CurrentRecordSearch.Fee = FeeModel;
                }
            }
        }

        public ObservableCollection<ICharge> Charges
        {
            get { return _charges; }
            set { SetProperty(ref _charges, value); }
        }

        public Fee FeeModel
        {
            get { return _fee; }
            set {
                SetProperty(ref _fee, value);
                Charges = FeeModel.Charges;
            }
        }


        public DelegateCommand ExportCommand { get; set; }
        //Constructor
        public CalculatorViewModel(IEventAggregator eventAggregator, IRecordSearchService recordSearchService)
        {
            _rs = recordSearchService;

            LoadFeeStructures();
            LoadFeeData();

            _isLoaded = true;

            ExportCommand = new DelegateCommand(ExportFee);
            eventAggregator.GetEvent<RSEntryChangedEvent>().Subscribe(LoadFeeData);
            eventAggregator.GetEvent<CalculatorCostChangedEvent>().Subscribe(UpdateTotalCost);
        }

        public void UpdateTotalCost()
        {
            FeeModel.CalculateProjectCost();
        }

        private void ExportFee()
        {
            Export report = new Export(FeeModel, _rs.CurrentRecordSearch.GetFileNumberFormatted());
        }

        /// <summary>
        /// Gathers the metadata for all fee structure documents in the FeeStructure directory.
        /// </summary>
        public void LoadFeeStructures()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            var filePath = Path.Combine(currentDirectory, @"Resources\FeeStructures\");
            var feeStructures = Directory.GetFiles(filePath);
            foreach (string path in feeStructures)
            {
                XElement xmlFile = XElement.Load(path);
                FeeSchedule newItem = (from fs in xmlFile.Descendants("Meta")
                                       select new FeeSchedule(
                                           path,
                                           (string)fs.Element("Version"),
                                           (string)fs.Element("Name"),
                                           DateTime.Parse((string)fs.Element("Date"))
                                           )
                                       ).Single();
                Versions.Add(newItem);
            }
        }

        /// <summary>
        /// Event driven loading for updating the FeeModel and SelectedVersion properties with the currently open record search.
        /// </summary>
        private void LoadFeeData()
        {
            _isLoaded = false;
            FeeSchedule fs = Versions.Where(v => v.Version == _rs.CurrentRecordSearch.Fee.FeeVersion).First();

            if (fs != null)
            {
                SelectedVersion = fs;
                if (_rs.CurrentRecordSearch != null && _rs.CurrentRecordSearch.Fee != null)
                {
                    FeeModel = _rs.CurrentRecordSearch.Fee;
                }
            }

            _isLoaded = true;
        }
    }
}
