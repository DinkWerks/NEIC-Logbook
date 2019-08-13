using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IAddressService
    {
        string ConnectionString { get; set; }
        void SetConnectionString();
        Address GetAddressByID(int id);
        int AddNewAddress(Address a);
        int UpdateAddress(Address a);
        void RemoveAddress(int id);
    }
}
