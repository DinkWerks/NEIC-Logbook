using Prism.Events;
using System.Collections.ObjectModel;
using Tracker.Core.StaticTypes;

namespace Tracker.Core.Events
{
    public class AdditionalCountySelectionOpenedEvent : PubSubEvent<ObservableCollection<County>>
    {
    }
}
