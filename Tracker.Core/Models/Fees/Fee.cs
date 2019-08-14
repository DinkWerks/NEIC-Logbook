using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Tracker.Core.Models.Fees
{
    public class Fee : BindableBase
    {
        private int _id;
        private string _feeVersion;
        private ObservableCollection<ICharge> _charges = new ObservableCollection<ICharge>();
        private decimal _totalProjectCost;
        private decimal _adjustment = 0;
        public string _adjustmentExplanation;

        public int ID
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

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

        public decimal Adjustment
        {
            get { return _adjustment; }
            set { SetProperty(ref _adjustment, value); }
        }
        public string AdjustmentExplanation
        {
            get { return _adjustmentExplanation; }
            set { SetProperty(ref _adjustmentExplanation, value); }
        }

        public decimal TotalProjectCost
        {
            get { return _totalProjectCost; }
            set { SetProperty(ref _totalProjectCost, value); }
        }

        public Fee(string version)
        {
            if (string.IsNullOrEmpty(version))
                FeeVersion = Settings.Settings.Instance.DefaultFeeStructure.Value;
            else
                FeeVersion = version;
            LoadFeeData(FeeVersion);
        }

        public void LoadFeeData(string fileName)
        {
            //TODO make resistant to unlocated filenames, try and default
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
                                                              DBField = (string)item.Element("DBField"),
                                                              Description = (string)item.Element("Description"),
                                                              UnitName = (string)item.Element("UnitName"),
                                                              UnitNamePlural = (string)item.Element("UnitNamePlural"),
                                                              Cost = (decimal)item.Element("Cost")
                                                          };
            IEnumerable<BooleanCharge> booleanCharges = from item in xmlFile.Descendants("Fee")
                                                        where (string)item.Attribute("type") == "boolean"
                                                        select new BooleanCharge()
                                                        {
                                                            Index = (int)item.Element("Index"),
                                                            Name = (string)item.Element("Name"),
                                                            DBField = (string)item.Element("DBField"),
                                                            Description = (string)item.Element("Description"),
                                                            Cost = (decimal)item.Element("Cost")
                                                        };
            IEnumerable<CategoricalCharge> categoricalCharges = from item in xmlFile.Descendants("Fee")
                                                                where (string)item.Attribute("type") == "categorical"
                                                                select new CategoricalCharge(item.Element("CostCategories"))
                                                                {
                                                                    Index = (int)item.Element("Index"),
                                                                    Name = (string)item.Element("Name"),
                                                                    DBField = (string)item.Element("DBField"),
                                                                    Description = (string)item.Element("Description"),
                                                                    UnitName = (string)item.Element("UnitName"),
                                                                    UnitNamePlural = (string)item.Element("UnitNamePlural"),
                                                                };
            //Foreach charge add to Charges
            ObservableCollection<ICharge> sortedCharges = new ObservableCollection<ICharge>();

            //TODO Can I replace this with group by statements?
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
            TotalProjectCost = runningTotal + Adjustment;
        }

        public string GetFieldNames(string queryType = "select")
        {
            switch (queryType)
            {
                case "select":
                    string fields = "";
                    foreach (ICharge c in Charges)
                    {
                        if (c.Type != "separator")
                            fields += c.DBField + ", ";
                    }
                    return fields.Substring(0, fields.Length-2);
                case "update":
                    string updateFields = "";
                    foreach (ICharge c in Charges)
                    {
                        if (c.Type != "separator")
                            updateFields += c.DBField + " = @" + c.DBField + ", ";
                    }
                    return updateFields.Substring(0, updateFields.Length - 2);
                default:
                    return string.Empty;
            }

        }
    }
}
