using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using Tracker.Core.Events;

namespace mFeeCalculator.Views
{
    /// <summary>
    /// Interaction logic for Calculator
    /// </summary>
    public partial class Calculator : UserControl
    {
        IEventAggregator _ea;
        public Calculator(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            InitializeComponent();
        }

        private void CostChangedEvent(object sender, RoutedEventArgs e)
        {
            //_ea.GetEvent<CalculatorCostChangedEvent>().Publish();
        }
    }
}
