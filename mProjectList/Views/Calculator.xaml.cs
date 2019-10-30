using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using Tracker.Core.Events;
using Tracker.Core.Events.Payloads;
using Tracker.Core.Models.Fees;

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
            ICharge c = (ICharge)((FrameworkElement)sender).Tag;
            if (c != null)
            {
                var payload = new ChargePayload(c.Type, c.DBField, c.GetAsDecimal());
                _ea.GetEvent<CalculatorCostChangedEvent>().Publish(payload);
            }
        }

        private void ModifierChangedEvent(object sender, RoutedEventArgs e)
        {
            _ea.GetEvent<CalculatorModifierChangedEvent>().Publish();
        }
    }
}
