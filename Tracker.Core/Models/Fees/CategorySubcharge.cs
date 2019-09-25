using Prism.Mvvm;

namespace Tracker.Core.Models.Fees
{
    /// <summary>
    /// Category subcharges total cost increases if the unit count reaces falls between certain thresholds. (Ex. 1-2 units cost $200, 2-4 cost $350)
    /// </summary>
    public class CategorySubcharge : BindableBase, ISubcharge
    {
        private int _minimumCount;
        private int _maximumCount;

        public int Minimum
        {
            get { return _minimumCount; }
            set { SetProperty(ref _minimumCount, value); }
        }

        public int Maximum
        {
            get { return _maximumCount; }
            set { SetProperty(ref _maximumCount, value); }
        }

        public decimal Cost { get; set; }

        public CategorySubcharge(int minimum, int maximum, decimal cost)
        {
            Minimum = minimum;
            Maximum = maximum;
            Cost = cost;
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
            return Cost;
        }
    }
}
