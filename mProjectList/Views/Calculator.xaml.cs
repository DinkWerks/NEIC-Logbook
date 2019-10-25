using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using Tracker.Core.Events;

namespace mProjectList.Views
{
    /// <summary>
    /// Interaction logic for Calculator
    /// </summary>
    public partial class Calculator : UserControl
    {
        private IEventAggregator _ea;

        public Calculator(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            InitializeComponent();
        }

        private void CostChangedEvent(object sender, RoutedEventArgs e)
        {
            _ea.GetEvent<CalculatorCostChangedEvent>().Publish();
        }
    }
}
