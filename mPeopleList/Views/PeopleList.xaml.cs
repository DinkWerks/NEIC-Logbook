using Prism.Events;
using System.Windows.Controls;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mPeopleList.Views
{
    /// <summary>
    /// Interaction logic for PeopleList
    /// </summary>
    public partial class PeopleList : UserControl
    {
        private IEventAggregator _ea;

        public PeopleList(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            InitializeComponent();
        }
        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Person selectedPerson = (Person)PersonListBox.SelectedItem;
            if(selectedPerson != null)
                _ea.GetEvent<PersonListSelectEvent>().Publish(selectedPerson.ID);
        }
    }
}
