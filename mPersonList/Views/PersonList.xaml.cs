using Prism.Events;
using System.Windows.Controls;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mPersonList.Views
{
    /// <summary>
    /// Interaction logic for PersonList
    /// </summary>
    public partial class PersonList : UserControl
    {
        public IEventAggregator _ea;

        public PersonList(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Person selectedPerson = (Person)PersonListBox.SelectedItem;
            _ea.GetEvent<PersonListSelectEvent>().Publish(selectedPerson.ID);
        }
    }
}
