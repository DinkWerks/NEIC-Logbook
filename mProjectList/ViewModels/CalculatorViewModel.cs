using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tracker.Core.Events;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mProjectList.ViewModels
{
    public class CalculatorViewModel : BindableBase
    {
        private FeeX _fee;
        private bool _isLoaded = false;
        private ObservableCollection<FeeStructure> _versions = new ObservableCollection<FeeStructure>();

        public FeeX Fee
        {
            get { return _fee; }
            set { SetProperty(ref _fee, value); }
        }

        public ObservableCollection<FeeStructure> Versions
        {
            get { return _versions; }
            set { SetProperty(ref _versions, value); }
        }

        //Constructor
        public CalculatorViewModel(IEventAggregator eventAggregator)
        {
            Versions = new ObservableCollection<FeeStructure>(FeeStructures.Structures.Values);


            eventAggregator.GetEvent<ProjectEntryChangedEvent>().Subscribe(OnParentNavigatedTo);
            eventAggregator.GetEvent<CalculatorCostChangedEvent>().Subscribe(UpdateTotalCost);
        }

        //Methods
        public void UpdateTotalCost()
        {
            Fee.CalculateProjectCost();
            //Add update to Project Total in Project?
        }

        public void OnParentNavigatedTo(FeeX fee)
        {
            Fee = fee;
        }
    }
}
