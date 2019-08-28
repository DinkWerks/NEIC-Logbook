using Tracker.Core.StaticTypes;

namespace Tracker.Core.Events.Payloads
{
    public class AdditionalCountySelectionPayload
    {
        public County CountyPayload { get; set; } 
        public bool IsAdded { get; set; }
    }
}
