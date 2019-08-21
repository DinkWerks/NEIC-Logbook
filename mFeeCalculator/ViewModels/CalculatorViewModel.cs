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
    public class CalculatorViewModel : BindableBase, IRegionMemberLifetime
    {
        private ObservableCollection<FeeSchedule> _versions = new ObservableCollection<FeeSchedule>();
        private ObservableCollection<ICharge> _charges = new ObservableCollection<ICharge>();
        private Fee _fee;
        private FeeSchedule _selectedVersion;
        private IRecordSearchService _rs;
        private bool _loaded = false;

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
                if (_loaded)
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

        public bool KeepAlive => false;

        public CalculatorViewModel(IEventAggregator eventAggregator, IRecordSearchService recordSearchService)
        {
            _rs = recordSearchService;
            LoadFeeStructures();

            //Select Fee Version and load Fee/Charges
            FeeSchedule fs = Versions.Where(v => v.Version == recordSearchService.CurrentRecordSearch.Fee.FeeVersion).First();
            if (fs != null)
            {
                SelectedVersion = fs;
                if (recordSearchService.CurrentRecordSearch != null && recordSearchService.CurrentRecordSearch.Fee != null)
                {
                    FeeModel = recordSearchService.CurrentRecordSearch.Fee;
                }
            }

            _loaded = true;
            eventAggregator.GetEvent<CalculatorCostChangedEvent>().Subscribe(UpdateTotalCost);
        }

        public void UpdateTotalCost()
        {
            FeeModel.CalculateProjectCost();
        }

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
    }
}
