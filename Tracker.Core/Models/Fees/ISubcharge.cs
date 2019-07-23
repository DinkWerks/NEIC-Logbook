namespace Tracker.Core.Models.Fees
{
    public interface ISubcharge
    {
        int Minimum { get; }
        int Maximum { get; }
        decimal Cost { get; }
        bool CheckValue(decimal value);
        decimal GetCost(decimal count);
    }
}
