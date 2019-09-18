using Prism.Mvvm;

namespace Tracker.Core.Models.Fees
{
    // TODO Rethink this. It's a cheap hack to call the separators a charge so that they can be mixed into the Fee's Charges Collection. All of these properties except for Index are unused.
    // Maybe make groupings an attribute in the XML or something.
    public class FeeSeparator : BindableBase, ICharge
    {
        private int _index;
        private string _name;
        private string _type = "separator";
        private string _description;
        private decimal _cost;
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

        public string DBField { get; set; }

        public string Type
        {
            get { return _type; }
            set { SetProperty(ref _type, value); }
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public decimal Cost
        {
            get { return _cost; }
            set { SetProperty(ref _cost, value); }
        }

        public decimal TotalCost { get; }

        public decimal RoundTotal(decimal value)
        {
            return decimal.Round(value, 2);
        }
    }
}
