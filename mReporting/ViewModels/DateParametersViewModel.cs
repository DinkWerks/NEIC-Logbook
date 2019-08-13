using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;

namespace mReporting.ViewModels
{
    public class DateParametersViewModel : BindableBase
    {
        private DateTime? _startDate;
        private DateTime? _endDate;
        private ObservableCollection<object> _parameterPayload = new ObservableCollection<object>();

        public DateTime? StartDate
        {
            get { return _startDate; }
            set {
                SetProperty(ref _startDate, value);
                ParameterPayload[0] = value;
            }
        }
        
        public DateTime? EndDate
        {
            get { return _endDate; }
            set {
                SetProperty(ref _endDate, value);
                ParameterPayload[1] = value;
            }
        }

        public ObservableCollection<object> ParameterPayload
        {
            get { return _parameterPayload; }
            set { SetProperty(ref _parameterPayload, value); }
        }

        public DelegateCommand Cmd { get; private set; }
        public DateParametersViewModel()
        {
            Cmd = new DelegateCommand(Test);
        }

        private void Test()
        {
            EndDate = DateTime.Now.AddDays(3);
        }
    }
}
