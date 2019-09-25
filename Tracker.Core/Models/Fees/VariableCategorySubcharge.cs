using Prism.Mvvm;

namespace Tracker.Core.Models.Fees
{
    /// <summary>
    /// Variable subcharges' per unit cost is determined on if the value fits in specific ranges.
    /// </summary>
    class VariableCategorySubcharge : BindableBase, ISubcharge
    {
        private int _minimum;
        private int _maximum;
        private decimal _flatCost;

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

        public VariableCategorySubcharge(int minimum, int maximum, decimal cost, decimal flatCost)
        {
            Minimum = minimum;
            Maximum = maximum;
            Cost = cost;
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
            return (Cost * (count - Minimum + 1)) + _flatCost;
        }


    }
}
