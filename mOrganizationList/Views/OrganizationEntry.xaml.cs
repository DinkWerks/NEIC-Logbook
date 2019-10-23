﻿using Prism.Events;
using System.Windows.Controls;
using Tracker.Core.Events;
using Tracker.Core.Models;

namespace mOrganizationList.Views
{
    /// <summary>
    /// Interaction logic for OrganizationEntry
    /// </summary>
    public partial class OrganizationEntry : UserControl
    {
        private IEventAggregator _ea;

        public OrganizationEntry(IEventAggregator eventAggregator)
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