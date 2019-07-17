using System.Collections.Generic;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IPersonService
    {
        Person CurrentPerson { get; set; }
        List<Person> CompletePeopleList { get; set; }
        string ConnectionString { get; set; }
        void SetConnectionString();
        Person GetPersonByID(int id, bool loadAsCurrentClient = true);
        List<Person> GetPartialPeopleByCriteria(string criteria);
        List<Person> GetAllPartialPeople();
        int AddNewPerson(Person p);
        int UpdatePersonInformation(Person p);
        bool ConfirmDistinct(string firstName, string lastName);
    }
}
