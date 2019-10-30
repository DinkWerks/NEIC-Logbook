﻿using Prism.Mvvm;

namespace Tracker.Core.Models.Fees
{
    public class BooleanCharge : BindableBase, ICharge
    {
        private int _index;
        private string _name;
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

        public string Type { get; } = "boolean";

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
            get { return RoundTotal(_totalCost); }
            set { SetProperty(ref _totalCost, value); }
        }

        public BooleanCharge()
        {

        }

        public string GetCostString()
        {
            return Cost.ToString("C");
        }

        public void Reset()
        {
            IsIncurred = false;
        }

        public decimal GetAsDecimal()
        {
            if (IsIncurred)
                return 1;
            else
                return 0;
        }

        public decimal RoundTotal(decimal value)
        {
            return decimal.Round(value, 2, System.MidpointRounding.AwayFromZero);
        }
    }
}
