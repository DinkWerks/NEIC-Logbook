using Prism.Mvvm;
using Prism.Regions;
using Tracker.Core.Models.Fees;

namespace mFeeCalculator.ViewModels
{
    public class VariableChargeViewModel : BindableBase
    {
        private VariableCharge _charge;

        public VariableCharge Charge
        {
            get { return _charge; }
            set { SetProperty(ref _charge, value); }
        }

        public VariableChargeViewModel()
        {

        }
    }
}
