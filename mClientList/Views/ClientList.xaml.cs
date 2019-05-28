using Prism.Events;
using System.Windows.Controls;

namespace mClientList.Views
{
    /// <summary>
    /// Interaction logic for ClientList
    /// </summary>
    public partial class ClientList : UserControl
    {
        private IEventAggregator _ea;
        public ClientList(IEventAggregator eventAggregator)
        {
            InitializeComponent();
            _ea = eventAggregator;
        }

        private void ListBoxItem_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            _
        }
    }
}
