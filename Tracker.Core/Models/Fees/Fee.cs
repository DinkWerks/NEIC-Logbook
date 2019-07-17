using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Tracker.Core.Models.Fees
{
    public class Fee : BindableBase
    {
        private string _feeVersion;
        private ObservableCollection<ICharge> _charges = new ObservableCollection<ICharge>();
        private decimal _totalProjectCost;

        public string FeeVersion
        {
            get { return _feeVersion; }
            set { SetProperty(ref _feeVersion, value); }
        }

        public ObservableCollection<ICharge> Charges
        {
            get { return _charges; }
            set { SetProperty(ref _charges, value); }
        }

        public decimal TotalProjectCost
        {
            get { return _totalProjectCost; }
            set { SetProperty(ref _totalProjectCost, value); }
        }

        public Fee(string version)
        {
            if (string.IsNullOrEmpty(version))
                FeeVersion = Properties.Settings.Default.FeeType;
            else
                FeeVersion = version;
            LoadFeeData(FeeVersion);
        }

        public void LoadFeeData(string fileName)
        {
            XElement xmlFile = XElement.Load($"{@"Resources\FeeStructures\" + fileName + ".xml"}");

            //Gather list of charges for DB
            IEnumerable<FeeSeparator> separators = from item in xmlFile.Descendants("Fee")
                                                   where (string)item.Attribute("type") == "separator"
                                                   select new FeeSeparator()
                                                   {
                                                       Index = (int)item.Element("Index")
                                                   };
            IEnumerable<VariableCharge> variableCharges = from item in xmlFile.Descendants("Fee")
                                                          where (string)item.Attribute("type") == "variable"
                                                          select new VariableCharge()
                                                          {
                                                              Index = (int)item.Element("Index"),
                                                              Name = (string)item.Element("Name"),
                                                              Description = (string)item.Element("Description"),
                                                              UnitName = (string)item.Element("UnitName"),
                                                              Cost = (decimal)item.Element("Cost")
                                                          };
            IEnumerable<BooleanCharge> booleanCharges = from item in xmlFile.Descendants("Fee")
                                                        where (string)item.Attribute("type") == "boolean"
                                                        select new BooleanCharge()
                                                        {
                                                            Index = (int)item.Element("Index"),
                                                            Name = (string)item.Element("Name"),
                                                            Description = (string)item.Element("Description"),
                                                            Cost = (decimal)item.Element("Cost")
                                                        };
            IEnumerable<CategoricalCharge> categoricalCharges = from item in xmlFile.Descendants("Fee")
                                                                where (string)item.Attribute("type") == "categorical"
                                                                select new CategoricalCharge(item.Element("CostCategories"))
                                                                {
                                                                    Index = (int)item.Element("Index"),
                                                                    Name = (string)item.Element("Name"),
                                                                    Description = (string)item.Element("Description")
                                                                };
            //Foreach charge add to Charges
            ObservableCollection<ICharge> sortedCharges = new ObservableCollection<ICharge>();
            separators.ToList().ForEach(sortedCharges.Add);
            variableCharges.ToList().ForEach(sortedCharges.Add);
            booleanCharges.ToList().ForEach(sortedCharges.Add);
            categoricalCharges.ToList().ForEach(sortedCharges.Add);
            sortedCharges.OrderBy(x => x.Index).ToList().ForEach(Charges.Add);
        }

        public void CalculateProjectCost()
        {
            decimal runningTotal = 0;
            foreach (ICharge charge in Charges)
            {
                runningTotal += charge.TotalCost;
            }
            TotalProjectCost = runningTotal;
        }
    }
}
