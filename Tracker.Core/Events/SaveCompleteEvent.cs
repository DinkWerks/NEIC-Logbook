using Prism.Events;
using Tracker.Core.Events.Payloads;

namespace Tracker.Core.Events
{
    public class StatusUpdateEvent : PubSubEvent<StatusPayload>
    {
    }
}
