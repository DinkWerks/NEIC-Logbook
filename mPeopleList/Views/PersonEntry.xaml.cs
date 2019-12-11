using Prism.Events;
using System.Windows.Controls;
using System.Windows.Input;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mPeopleList.Views
{
    /// <summary>
    /// Interaction logic for PersonEntry
    /// </summary>
    public partial class PersonEntry : UserControl
    {
        private IEventAggregator _ea;

        public PersonEntry(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Project selectedRS = (Project)RSListBox.SelectedItem;
            _ea.GetEvent<PersonProjectListSelectEvent>().Publish(selectedRS.Id);
        }
    }
}
