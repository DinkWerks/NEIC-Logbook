using mReporting.ViewModels;
using Prism.Common;
using Prism.Regions;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;

namespace mReporting.Views
{
    /// <summary>
    /// Interaction logic for DateParameters
    /// </summary>
    public partial class DateParameters : UserControl
    {
        public DateParameters()
        {
            RegionContext.GetObservableContext(this).PropertyChanged += ParameterPropertyChanged;
            InitializeComponent();
        }

        private void ParameterPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var parameters = (ObservableCollection<object>)context.Value;
            var dc = (DataContext as DateParametersViewModel);
            dc.ParameterPayload = parameters;
            dc.ParameterPayload.Add(dc.StartDate);
            dc.ParameterPayload.Add(dc.EndDate);
            dc.EndDate = DateTime.Now;
        }
    }
}
