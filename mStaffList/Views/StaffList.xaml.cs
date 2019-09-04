using Prism.Events;
using System.Windows.Controls;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mStaffList.Views
{
    /// <summary>
    /// Interaction logic for StaffList
    /// </summary>
    public partial class StaffList : UserControl
    {
        private IEventAggregator _ea;

        public StaffList(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _ea = eventAggregator;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RecordSearch selectedRS = (RecordSearch)RSListBox.SelectedItem;
            _ea.GetEvent<RecordSearchListSelectEvent>().Publish(selectedRS.ID);
        }
    }
}
