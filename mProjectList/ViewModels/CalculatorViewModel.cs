using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mProjectList.ViewModels
{
    public class CalculatorViewModel : BindableBase
    {
        private FeeX _fee;
        private bool _isLoaded = false;

        public FeeX Fee
        {
            get { return _fee; }
            set { SetProperty(ref _fee, value); }
        }

        //Constructor
        public CalculatorViewModel(IEventAggregator eventAggregator)
        {
            eventAggregator.GetEvent<CalculatorCostChangedEvent>().Subscribe(UpdateTotalCost);
        }

        //Methods
        public void UpdateTotalCost()
        {
            Fee.CalculateProjectCost();
            //Add update to Project Total?
        }
    }
}
