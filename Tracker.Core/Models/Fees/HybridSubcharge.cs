using Prism.Mvvm;

namespace Tracker.Core.Models.Fees
{
    /// <summary>
    /// Hybrid subcharges total costs increase linearly as units fall between specific ranges. (Ex. Charges increase $400 for every 50 units.)
    /// </summary>
    class HybridSubcharge : BindableBase, ISubcharge
    {
        private int _minimum;
        private int _maximum;
        private decimal _flatCost;
        private int _groupSize;

        public int Minimum
        {
            get { return _minimum; }
            set { SetProperty(ref _minimum, value); }
        }

        public int Maximum
        {
            get { return _maximum; }
            set { SetProperty(ref _maximum, value); }
        }

        public decimal Cost { get; set; }

        public int Count { get; set; }

        public HybridSubcharge(int minimum, int maximum, decimal cost, int groupSize, decimal flatCost)
        {
            Minimum = minimum;
            Maximum = maximum;
            Cost = cost;
            _groupSize = groupSize;
            _flatCost = flatCost;
        }

        public bool CheckValue(decimal value)
        {
            if (value >= Minimum && value <= Maximum)
            {
                return true;
            }
            return false;
        }

        public decimal GetCost(decimal count)
        {
            return (Cost * ((count - Minimum)/_groupSize)) + _flatCost;
        }
    }
}
