using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tracker.Core.Models;
using Tracker.Core.Services;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.ViewModels
{
    public class CountySelectDialogContentsViewModel : BindableBase
    {
        private IEnumerable<County> _selectableCounties = new List<County>();
        private ObservableCollection<County> _selectedCounties = new ObservableCollection<County>();
        private RecordSearch _recordSearch;

        public IEnumerable<County> SelectableCounties
        {
            get { return _selectableCounties; }
            set { SetProperty(ref _selectableCounties, value); }
        }

        public ObservableCollection<County> SelectedCounties
        {
            get { return _selectedCounties; }
            set { SetProperty(ref _selectedCounties, value); }
        }

        public RecordSearch RecordSearch
        {
            get { return _recordSearch; }
            set { SetProperty(ref _recordSearch, value); }
        }

        public CountySelectDialogContentsViewModel(IRecordSearchService recordSearchService)
        {
            SelectableCounties = from c in Counties.Values
                                 where c.ICCurator is "NEIC"
                                 select c;
            RecordSearch = recordSearchService.CurrentRecordSearch;
        }
    }
}
