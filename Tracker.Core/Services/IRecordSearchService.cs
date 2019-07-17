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
        List<RecordSearch> GetAllPartialRecordSearches();
        List<RecordSearch> GetPartialRecordSearchesByCriteria(string criteria);
        int AddNewRecordSearch(object[] array);
        int GetNextEnumeration(string prefix, string year);
        bool ConfirmDistinct(string prefix, string year, int enumeration, string suffix);
        
    }
}
