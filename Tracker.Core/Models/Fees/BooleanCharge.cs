using Prism.Mvvm;
using Tracker.Core.Events;

namespace Tracker.Core.Models.Fees
{
    public class BooleanCharge : BindableBase, ICharge
    {
        private int _index;
        private string _name;
        private string _type = "boolean";
        private string _description;
        private bool _isIncurred;
        private decimal _cost;
        private decimal _totalCost;
        private string _dbField;

        public int Index
        {
            get { return _index; }
            set { SetProperty(ref _index, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string DBField
        {
            get { return _dbField; }
            set { SetProperty(ref _dbField, value); }
        }

        public string Type
        {
            get { return _type; }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public bool IsIncurred
        {
            get { return _isIncurred; }
            set
            {
                SetProperty(ref _isIncurred, value);
                if (value)
                    TotalCost = Cost;
                else
                    TotalCost = 0;
            }
        }

        public decimal Cost
        {
            get { return _cost; }
            set { SetProperty(ref _cost, value); }
        }

        public decimal TotalCost
        {
            get { return _totalCost; }
            set { SetProperty(ref _totalCost, value); }
        }

    }
}
