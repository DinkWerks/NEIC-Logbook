using Prism.Events;
using Tracker.Core.Events.CustomPayloads;

namespace Tracker.Core.Events
{
    public class StatusUpdateEvent : PubSubEvent<StatusPayload>
    {
    }
}
