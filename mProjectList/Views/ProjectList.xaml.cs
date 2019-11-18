using Prism.Events;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mProjectList.Views
{
    /// <summary>
    /// Interaction logic for ProjectList
    /// </summary>
    public partial class ProjectList : UserControl
    {
        private IEventAggregator _ea;

        public ProjectList(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            InitializeComponent();
        }

        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Project selectedRS = (Project)ProjectListBox.SelectedItem;
            if(selectedRS != null)
                _ea.GetEvent<ProjectListSelectEvent>().Publish(selectedRS.Id);
        }

        private void ICFilePrefix_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            {
                if (ICFilePrefix.Text != null && ICFilePrefix.Text.Length >= 2)
                {
                    if (e.Key != Key.Back)
                    {
                        e.Handled = true;
                        System.Media.SystemSounds.Beep.Play();
                    }
                }
            }
        }

        private void ICFilePrefix_KeyUp(object sender, KeyEventArgs e)
        {
            if (ICFilePrefix.Text != null && ICFilePrefix.Text.Length >= 1)
            {
                ICFileYear.Focus();
                e.Handled = true;
            }
        }

        private void ICFileYear_KeyUp(object sender, KeyEventArgs e)
        {
            if (ICFilePrefix.Text != null && ICFileYear.Text.Length >= 2)
            {
                ICFileEnum.Focus();
                e.Handled = true;
            }
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (this.ShowAllCheck.IsChecked == false)
            {
                List<string> viewableColumns = new List<string>() { "ICTypePrefix", "ICYear", "ICEnumeration", "ICSuffix" , "ProjectName", "MainCounty", "PLSS" };
                if (!viewableColumns.Contains(e.PropertyName))
                    e.Cancel = true;
            }

        }

        private void ShowAllCheck_Changed(object sender, System.Windows.RoutedEventArgs e)
        {
            this.SheetGrid.AutoGenerateColumns = false;
            this.SheetGrid.AutoGenerateColumns = true;
        }
    }
}
