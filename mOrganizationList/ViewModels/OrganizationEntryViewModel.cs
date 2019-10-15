using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.CompositeCommands;
using Tracker.Core.Models;
using Tracker.Core.Services;

namespace mOrganizationList.ViewModels
{
    public class OrganizationEntryViewModel : BindableBase
    {
        private IEFService _ef;
        private IEventAggregator _ea;
        private Organization _organization;

        public Organization Organization
        {
            get { return _organization; }
            set { SetProperty(ref _organization, value); }
        }

        //
        public OrganizationEntryViewModel(IEFService efService, IApplicationCommands applicationCommands, IEventAggregator eventAggregator)
        {
            _ef = efService;
            _ea = eventAggregator;

        }
    }
}
