using Prism.Events;
using System.Windows.Controls;
using System.Windows.Input;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mRecordSearchList.Views
{
    /// <summary>
    /// Interaction logic for RSList
    /// </summary>
    public partial class RSList : UserControl
    {
        private IEventAggregator _ea;
        public RSList(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RecordSearch selectedRS = (RecordSearch)RSListBox.SelectedItem;
            _ea.GetEvent<ProjectListSelectEvent>().Publish(selectedRS.ID);
        }

        private void RSIDPrefix_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            {
                if (RSIDPrefix.Text != null && RSIDPrefix.Text.Length >= 2)
                {
                    if (e.Key != Key.Back) // allow removing chars
                    {
                        e.Handled = true; // block any additional key press if there is more than allowed max
                        System.Media.SystemSounds.Beep.Play(); // optional: beep to let user know he is out of space :)
                    }
                }
            }
        }

        private void RSIDPrefix_KeyUp(object sender, KeyEventArgs e)
        {
            if (RSIDPrefix.Text != null && RSIDPrefix.Text.Length >= 1)
            {
                RSIDYear.Focus();
                e.Handled = true;
            }
        }

        private void RSIDYear_KeyUp(object sender, KeyEventArgs e)
        {
            if (RSIDPrefix.Text != null && RSIDYear.Text.Length >= 2)
            {
                RSIDEnum.Focus();
                e.Handled = true;
            }
        }
    }
}
