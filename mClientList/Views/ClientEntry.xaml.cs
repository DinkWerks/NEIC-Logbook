using Prism.Events;
using System.Windows.Controls;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mClientList.Views
{
    /// <summary>
    /// Interaction logic for ClientEntry
    /// </summary>
    public partial class ClientEntry : UserControl
    {
        private IEventAggregator _ea;

        public ClientEntry(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _ea = eventAggregator;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Person selectedPerson = (Person)PersonListBox.SelectedItem;
            _ea.GetEvent<PersonListSelectEvent>().Publish(selectedPerson.ID);
        }
    }
}
