using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tracker.ViewModels
{
    public class HomeScreenViewModel : BindableBase
    {
        private IRegionManager _rm;

        public DelegateCommand<string> NavigateCommand { get; private set; }

        public HomeScreenViewModel(IRegionManager regionManager)
        {
            _rm = regionManager;
            NavigateCommand = new DelegateCommand<string>(Navigate);
        }

        private void Navigate(string navigationTarget)
        {
            _rm.RequestNavigate("ContentRegion", navigationTarget);
        }
    }
}
