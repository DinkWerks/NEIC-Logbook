namespace Tracker.Core.Models.Fees
{
    public interface ISubcharge
    {
        int Minimum { get; }
        int Maximum { get; }
        decimal Cost { get; }
        bool CheckValue(int value);
        decimal GetCost(int count);
    }
}
