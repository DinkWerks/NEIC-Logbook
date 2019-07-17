using Prism.Events;
using System.Windows.Controls;
using Tracker.Core.Events;
using Tracker.Core.StaticTypes;

namespace mRecordSearchList.Views
{
    /// <summary>
    /// Interaction logic for CountySelectDialogContents
    /// </summary>
    public partial class CountySelectDialogContents : UserControl
    {
        private IEventAggregator _ea;
        public CountySelectDialogContents(IEventAggregator eventAggregator)
        {
            _ea = eventAggregator;
            InitializeComponent();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (County item in e.AddedItems)
            {
                _ea.GetEvent<AdditionalCountySelectionEvent>().Publish(
                    new Tracker.Core.Events.CustomPayloads.AdditionalCountySelectionPayload() { CountyPayload = item, IsAdded = true }
                    );
            }

            foreach (County item in e.RemovedItems)
            {
                _ea.GetEvent<AdditionalCountySelectionEvent>().Publish(
                    new Tracker.Core.Events.CustomPayloads.AdditionalCountySelectionPayload() { CountyPayload = item, IsAdded = false }
                    );
            }
        }
    }
}
