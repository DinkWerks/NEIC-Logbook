using Prism.Events;
using System.Windows.Controls;
using System.Windows.Input;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mPersonList.Views
{
    /// <summary>
    /// Interaction logic for PersonEntry
    /// </summary>
    public partial class PersonEntry : UserControl
    {
        private IEventAggregator _ea;

        public PersonEntry(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _ea = eventAggregator;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RecordSearch selectedRS = (RecordSearch)RSListBox.SelectedItem;
            _ea.GetEvent<RecordSearchListSelectEvent>().Publish(selectedRS.ID);
        }
    }
}
