using Prism.Mvvm;

namespace Tracker.Core.Models.Fees
{
    public class VariableCharge : BindableBase, ICharge
    {
        private int _index;
        private string _name;
        private string _type = "variable";
        private string _description;
        private string _unitName;
        private string _unitNamePlural;
        private decimal _count;
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

        public string UnitName
        {
            get { return _unitName; }
            set { SetProperty(ref _unitName, value); }
        }

        public string UnitNamePlural
        {
            get { return _unitNamePlural; }
            set { SetProperty(ref _unitNamePlural, value); }
        }

        public decimal Count
        {
            get { return _count; }
            set {
                SetProperty(ref _count, value);
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
            get { return _totalCost;  }
            private set { SetProperty(ref _totalCost, Count * Cost); }
        }
    }
}
