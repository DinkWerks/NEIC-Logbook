using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using Tracker.Core.Models.Fees;

namespace Tracker.Core.Models
{
    public class FeeX : BindableBase
    {
        private string _feeVersion;
        private FeeStructure _feeStructure;
        private FeeData _feeData;
        private ObservableCollection<ICharge> _charges = new ObservableCollection<ICharge>();
        private decimal _subtotal;
        private decimal _totalProjectCost;

        public string FeeVersion
        {
            get { return _feeVersion; }
            set { SetProperty(ref _feeVersion, value); }
        }

        public FeeStructure FeeStructure
        {
            get { return _feeStructure; }
            set { SetProperty(ref _feeStructure, value); }
        }

        public FeeData FeeData
        {
            get { return _feeData; }
            set { SetProperty(ref _feeData, value); }
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

        public decimal Subtotal
        {
            get { return _subtotal; }
            set { SetProperty(ref _subtotal, value); }
        }

        //Constructor
        public FeeX(string version, FeeData feeData)
        {
            if (string.IsNullOrWhiteSpace(version))
                FeeStructure = FeeStructures.Structures[Settings.Settings.Instance.DefaultFeeStructure.Value];
            else
                FeeStructure = FeeStructures.Structures[version];

            FeeData = feeData;

            StructureCharges(FeeStructure.Version);
            CalculateProjectCost();
        }

        //Methods
        public void StructureCharges(string fileName)
        {
            //TODO make resistant to unlocated filenames, try and default
            XElement xmlFile = XElement.Load($"{@"Resources\FeeStructures\" + fileName + ".xml"}");
            //var t = (decimal)(int)FeeData.GetType().GetProperty("GISFeatures").GetValue(FeeData);

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
                                                              Cost = (decimal)item.Element("Cost"),
                                                              Count = FeeData.GetAsDecimal((string)item.Element("DBField")),
                                                          };
            IEnumerable<BooleanCharge> booleanCharges = from item in xmlFile.Descendants("Fee")
                                                        where (string)item.Attribute("type") == "boolean"
                                                        select new BooleanCharge()
                                                        {
                                                            Index = (int)item.Element("Index"),
                                                            Name = (string)item.Element("Name"),
                                                            DBField = (string)item.Element("DBField"),
                                                            Description = (string)item.Element("Description"),
                                                            Cost = (decimal)item.Element("Cost"),
                                                            IsIncurred = (bool)FeeData.GetType().GetProperty((string)item.Element("DBField")).GetValue(FeeData)
                                                        };
            IEnumerable<CategoricalCharge> categoricalCharges = from item in xmlFile.Descendants("Fee")
                                                                where (string)item.Attribute("type") == "categorical"
                                                                select new CategoricalCharge(item.Element("CostCategories"))
                                                                {
                                                                    Index = (int)item.Element("Index"),
                                                                    Name = (string)item.Element("Name"),
                                                                    DBField = (string)item.Element("DBField"),
                                                                    Count = FeeData.GetAsDecimal((string)item.Element("DBField")),
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
            decimal surcharge = 0;

            foreach (ICharge charge in Charges)
            {
                runningTotal += charge.TotalCost;
                if (FeeData.IsRapidResponse && charge.Name == "Staff Time")
                    surcharge += charge.TotalCost * 0.5m;
            }

            if (FeeData.IsPriority)
                surcharge += runningTotal * 0.5m;
            if (FeeData.IsEmergency)
                surcharge += runningTotal;

            Subtotal = runningTotal + FeeData.Adjustment;
            TotalProjectCost = runningTotal + FeeData.Adjustment + surcharge;
        }
    }
}
