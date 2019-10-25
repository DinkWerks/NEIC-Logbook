using mRecordSearchList.ViewModels;
using Prism.Common;
using Prism.Regions;
using System.ComponentModel;
using System.Windows.Controls;
using Tracker.Core.Models;

namespace mRecordSearchList.Views
{
    /// <summary>
    /// Interaction logic for AddressEntry
    /// </summary>
    public partial class AddressEntry : UserControl
    {
        public AddressEntry()
        {
            InitializeComponent();
            RegionContext.GetObservableContext(this).PropertyChanged += AddressPropertyChanged;
        }

        private void AddressPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var context = (ObservableObject<object>)sender;
            var address = (Address)context.Value;
            (DataContext as AddressEntryViewModel).Address = address;
        }
    }
}
