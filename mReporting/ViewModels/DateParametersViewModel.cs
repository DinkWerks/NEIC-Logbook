using Prism.Mvvm;
using System;

namespace mReporting.ViewModels
{
    public class DateParametersViewModel : BindableBase
    {
        private DateTime? _startDate;
        private DateTime? _endDate;

        public DateTime? Date
        {
            get { return _startDate; }
            set { SetProperty(ref _startDate, value); }
        }
        
        public DateTime? EndDate
        {
            get { return _endDate; }
            set { SetProperty(ref _endDate, value); }
        }

        public DateParametersViewModel()
        {

        }
    }
}
