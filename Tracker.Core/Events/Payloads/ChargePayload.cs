namespace Tracker.Core.Events.Payloads
{
    public class ChargePayload
    {
        public string Type { get; set; }
        public string DBField { get; set; }
        public decimal Count { get; set; }

        public ChargePayload(string type, string dbField, decimal count)
        {
            Type = type;
            DBField = dbField;
            Count = count;
        }
    }
}
