using Tracker.Core.Models.Fees;

namespace Tracker.Core.Services
{
    public interface IFeeService
    {
        string ConnectionString { get; set; }
        void SetConnectionString();
        Fee GetFeeData(Fee returnValue);
        int AddNewFee();
        int UpdateFee(Fee f);
    }
}
