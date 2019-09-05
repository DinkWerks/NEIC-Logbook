using Prism.Events;

namespace Tracker.Core.Events
{
    public class RSListModifiedEvent : PubSubEvent<Payloads.ListModificationPayload>
    {
    }
}
