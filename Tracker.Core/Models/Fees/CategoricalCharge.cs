using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Tracker.Core.Models.Fees
{
    public class CategoricalCharge : BindableBase, ICharge
    {
        private int _index;
        private string _name;
        private string _type = "categorical";
        private string _description;
        private string _unitName;
        private string _unitNamePlural;
        private decimal _count;
        private List<ISubcharge> _costCategories;
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

        public string Type
        {
            get { return _type; }
        }

        public string DBField
        {
            get { return _dbField; }
            set { SetProperty(ref _dbField, value); }
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
                TotalCost = FindCost(value);
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
            private set { SetProperty(ref _totalCost, value); }
        }

        public List<ISubcharge> CostCategories
        {
            get { return _costCategories; }
            set { SetProperty(ref _costCategories, value); }
        }

        public CategoricalCharge(XElement rawCostCategories)
        {
            CostCategories = ProcessSubcharges(rawCostCategories);
        }

        private List<ISubcharge> ProcessSubcharges(XElement xmlData)
        {
            IEnumerable<CategorySubcharge> categorySubcharges = from item in xmlData.Elements("Category")
                                                                where (string)item.Attribute("type") == "category"
                                                                select new CategorySubcharge(
                                                                    (int)item.Element("Minimum"),
                                                                    (int)item.Element("Maximum"),
                                                                    (decimal)item.Element("Cost")
                                                                    );
            IEnumerable<VariableCategorySubcharge> variableSubcharges = from item in xmlData.Elements("Category")
                                                                        where (string)item.Attribute("type") == "variable"
                                                                        select new VariableCategorySubcharge(
                                                                            (int)item.Element("Minimum"),
                                                                            (int)item.Element("Maximum"),
                                                                            (decimal)item.Element("Cost"),
                                                                            (decimal)item.Element("FlatCost")
                                                                            );
            IEnumerable<HybridSubcharge> hybridSubcharges = from item in xmlData.Elements("Category")
                                                            where (string)item.Attribute("type") == "hybrid"
                                                            select new HybridSubcharge(
                                                                (int)item.Element("Minimum"),
                                                                (int)item.Element("Maximum"),
                                                                (decimal)item.Element("Cost"),
                                                                (int)item.Element("GroupSize"),
                                                                (decimal)item.Element("FlatCost")
                                                                );
            List<ISubcharge> processedSubcharges = new List<ISubcharge>();
            categorySubcharges.ToList().ForEach(processedSubcharges.Add);
            variableSubcharges.ToList().ForEach(processedSubcharges.Add);
            hybridSubcharges.ToList().ForEach(processedSubcharges.Add);
            return processedSubcharges;
        }

        private decimal FindCost(decimal count)
        {
            foreach (ISubcharge charge in CostCategories)
            {
                if (charge.CheckValue(count))
                {
                    return charge.GetCost(count);
                }
            }
            return 0;
        }
    }
}
