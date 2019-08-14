using System.Collections.Generic;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IRecordSearchService
    {
        RecordSearch CurrentRecordSearch { get; set; }
        string ConnectionString { get; set; }
        void SetConnectionString();
        RecordSearch GetRecordSearchByID(int id, bool loadAsCurrentSearch = true);
        List<RecordSearch> GetRecordSearchesByCriteria(string criteria);
        List<RecordSearch> GetAllPartialRecordSearches();
        List<RecordSearch> GetPartialRecordSearchesByCriteria(string criteria);
        int AddNewRecordSearch(object[] array);
        void UpdateRecordSearch(RecordSearch rs);
        void UpdateICFileNumber(int id, object[] array);
        void RemoveRecordSearch(int id, int mailingAddressID, int billingAddressID, int feeID);
        int GetNextEnumeration(string prefix, string year);
        bool ConfirmDistinct(string prefix, string year, int enumeration, string suffix);

    }
}
