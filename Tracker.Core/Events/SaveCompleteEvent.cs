using Prism.Events;
using Tracker.Core.Events.CustomPayloads;

namespace Tracker.Core.Events
{
    public class SaveCompleteEvent : PubSubEvent<StatusPayload>
    {
    }
}
