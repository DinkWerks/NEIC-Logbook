using System.Collections.Generic;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IClientService
    {
        Client CurrentClient { get; set; }
        List<Client> CompleteClientList { get; set; }
        string ConnectionString { get; set; }
        void SetConnectionString();
        Client GetClientByID(int id, bool loadAsCurrentClient = true);
        List<Client> GetAllPartialClients();
        int AddNewClient(Client newClient);
        void UpdateClientInformation(object array);
        bool ConfirmDistinct(string clientName, string officeName);
    }
}
