using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Tracker.Core.DTO;
using Tracker.Core.Models;

namespace Tracker.Core.Services
{
    public interface IPersonService
    {
        Person GetPerson(int id);
        Person GetPersonFull(int id);
        List<Person> GetPeople(bool tracking = true);
        void AddPerson(Person newPerson);
        void UpdatePerson(Person person);
        void DeletePerson(Person person);
    }

    public class PersonService : IPersonService
    {
        private readonly IEFService _context;

        public PersonService(IEFService eFService)
        {
            _context = eFService;
        }

        public Person GetPerson(int id)
        {
            return _context.People.Find(id);
        }

        public Person GetPersonFull(int id)
        {
            return _context.People
                .Include(p => p.Affiliation)
                .Include(p => p.RecentProjects)
                .Where(p => p.ID == id)
                .FirstOrDefault();
        }

        public List<Person> GetPeople(bool tracking = true)
        {
            if (tracking)
                return _context.People
                    .Include(p => p.Affiliation)
                    .OrderBy(s => s.LastName)
                    .ToList();
            else
                return _context.People
                    .Include(p => p.Affiliation)
                    .OrderBy(s => s.LastName)
                    .AsNoTracking()
                    .ToList();
        }

        public List<PersonListDTO> GetPeopleDTOs()
        {
            return _context.People.Select(p => new PersonListDTO
            {
                Id = p.ID,
                FirstName = p.FirstName,
                LastName = p.LastName,
                Affiliation = p.Affiliation.OrganizationName
            }).OrderPeopleDTOsByLastName().ToList();
        }

        public void AddPerson(Person newPerson)
        {
            _context.People.Add(newPerson);
            _context.SaveChanges();
        }

        public void UpdatePerson(Person person)
        {
            _context.People.Update(person);
            _context.SaveChanges();
        }

        public void DeletePerson(Person person)
        {
            _context.People.Remove(person);
            _context.SaveChanges();
        }
    }

    public static class PersonServiceExtensions
    {
        public static IQueryable<Person> OrderPeopleByLastName(this IQueryable<Person> people)
        {
            return people.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
        }

        public static IQueryable<PersonListDTO> OrderPeopleDTOsByLastName(this IQueryable<PersonListDTO> people)
        {
            return people.OrderBy(p => p.LastName).ThenBy(p => p.FirstName);
        }
    }
}
