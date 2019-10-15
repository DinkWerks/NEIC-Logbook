using Prism.Events;
using System.Windows.Controls;

namespace mOrganizationList.Views
{
    /// <summary>
    /// Interaction logic for OrganizationList
    /// </summary>
    public partial class OrganizationList : UserControl
    {
        private IEventAggregator _ea;

        public OrganizationList(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _ea = eventAggregator;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
           
        }
    }
}
