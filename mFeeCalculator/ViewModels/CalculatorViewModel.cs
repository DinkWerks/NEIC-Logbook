using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System.Collections.ObjectModel;
using System.IO;
using Tracker.Core.Events;
using Tracker.Core.Models.Fees;
using Tracker.Core.Services;

namespace mFeeCalculator.ViewModels
{
    public class CalculatorViewModel : BindableBase, IRegionMemberLifetime
    {
        private ObservableCollection<string> _versions = new ObservableCollection<string>();
        private ObservableCollection<ICharge> _charges = new ObservableCollection<ICharge>();
        private Fee _fee;
        private string _selectedVersion;
        private IRecordSearchService _rs;
        private bool _loaded = false;

        public ObservableCollection<string> Versions
        {
            get { return _versions; }
            set { SetProperty(ref _versions, value); }
        }

        public string SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                SetProperty(ref _selectedVersion, value);
                if (_loaded)
                {
                    int oldFeeID = FeeModel.ID;
                    FeeModel = new Fee(value) { ID = oldFeeID };
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
            if (Versions.Contains(recordSearchService.CurrentRecordSearch.Fee.FeeVersion))
            {
                SelectedVersion = recordSearchService.CurrentRecordSearch.Fee.FeeVersion;
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
            foreach (string feeStructure in feeStructures)
            {
                string fileName = Path.GetFileName(feeStructure);
                Versions.Add(fileName.Remove(fileName.Length - 4));
            }
        }
    }
}
