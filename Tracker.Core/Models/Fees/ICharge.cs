namespace Tracker.Core.Models.Fees
{
    public interface ICharge
    {
        int Index { get; set; }
        string Name { get; set; }
        string DBField { get; set; }
        string Type { get; }
        string Description { get; set; }
        decimal Cost { get; set; }
        decimal TotalCost { get; }
        decimal RoundTotal(decimal value);
        string GetCostString();
        void Reset();
        decimal GetAsDecimal();
    }
}
