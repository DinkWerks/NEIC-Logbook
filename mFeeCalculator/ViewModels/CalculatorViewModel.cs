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
    public class CalculatorViewModel : BindableBase, INavigationAware, IRegionMemberLifetime
    {
        private ObservableCollection<string> _versions = new ObservableCollection<string>();
        private ObservableCollection<ICharge> _charges = new ObservableCollection<ICharge>();
        private Fee _fee;
        private string _selectedVersion;

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
                Fee = new Fee(value);
            }
        }

        public ObservableCollection<ICharge> Charges
        {
            get { return _charges; }
            set { SetProperty(ref _charges, value); }
        }

        public Fee Fee
        {
            get { return _fee; }
            set { SetProperty(ref _fee, value); }
        }

        public bool KeepAlive => false;

        public CalculatorViewModel(IEventAggregator eventAggregator, IRecordSearchService recordSearchService)
        {
            LoadFeeStructures();
            if (Versions.Contains(recordSearchService.CurrentRecordSearch.FeeVersion))
            {
                SelectedVersion = recordSearchService.CurrentRecordSearch.FeeVersion;
                if (recordSearchService.CurrentRecordSearch != null && recordSearchService.CurrentRecordSearch.Fee != null)
                {
                    Fee = recordSearchService.CurrentRecordSearch.Fee;
                    Charges = recordSearchService.CurrentRecordSearch.Fee.Charges;
                }
            }

            eventAggregator.GetEvent<CalculatorCostChangedEvent>().Subscribe(UpdateTotalCost);
        }

        public void UpdateTotalCost()
        {
            Fee.CalculateProjectCost();
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

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }
    }
}
