using Prism.Events;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models;
using Tracker.Core.Models.Fees;

namespace mProjectList.ViewModels
{
    public class CalculatorViewModel : BindableBase
    {
        private FeeX _fee;
        private ObservableCollection<FeeStructure> _versions = new ObservableCollection<FeeStructure>();
        private FeeStructure _selectedVersion;
        private bool _isloaded = false;

        public FeeX Fee
        {
            get { return _fee; }
            set { SetProperty(ref _fee, value); }
        }

        public ObservableCollection<FeeStructure> Versions
        {
            get { return _versions; }
            set { SetProperty(ref _versions, value); }
        }

        public FeeStructure SelectedVersion
        {
            get { return _selectedVersion; }
            set 
            {
                SetProperty(ref _selectedVersion, value);
                if (_isloaded)
                {
                    foreach (ICharge c in Fee.Charges)
                        c.Reset();
                    Fee.CalculateProjectCost();
                    Fee.FeeStructure = value;
                }
            }
        }

        //Constructor
        public CalculatorViewModel(IEventAggregator eventAggregator)
        {
            Versions = new ObservableCollection<FeeStructure>(FeeStructures.Structures.Values);

            eventAggregator.GetEvent<ProjectEntryChangedEvent>().Subscribe(OnParentNavigatedTo);
            eventAggregator.GetEvent<CalculatorCostChangedEvent>().Subscribe(UpdateTotalCostAndCharge);
            eventAggregator.GetEvent<CalculatorModifierChangedEvent>().Subscribe(UpdateTotalCost);
        }

        //Methods
        public void UpdateTotalCost()
        {
            Fee.CalculateProjectCost();
        }

        public void UpdateTotalCostAndCharge(ChargePayload payload)
        {
            Fee.CalculateProjectCost();

            if (payload.Type == "bool")
                if (payload.Count == 1)
                    Fee.FeeData.GetType().GetProperty(payload.DBField).SetValue(Fee.FeeData, true);
                else
                    Fee.FeeData.GetType().GetProperty(payload.DBField).SetValue(Fee.FeeData, false);
            else
                Fee.FeeData.SetFromDecimal(payload.DBField, payload.Count);
        }

        public void OnParentNavigatedTo(FeeX fee)
        {
            _isloaded = false;
            Fee = fee;
            SelectedVersion = fee.FeeStructure;
            Fee.CalculateProjectCost();
            _isloaded = true;
        }
    }
}
