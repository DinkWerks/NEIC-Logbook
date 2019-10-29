using Prism.Events;
using Tracker.Core.Models;

namespace Tracker.Core.Events
{
    public class ProjectEntryChangedEvent : PubSubEvent<FeeX>
    {
        public ProjectEntryChangedEvent()
        {

        }
    }
}
