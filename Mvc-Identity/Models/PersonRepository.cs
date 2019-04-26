using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mvc_Identity.DataBase;
using Mvc_Identity.Interfaces;
using Mvc_Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.Models
{
    public class PersonRepository : IPersonRepository
    {
        private readonly CountryDbContext _db;

        public PersonRepository(CountryDbContext countryDbContext)
        {
            _db = countryDbContext;
        }

        public List<Person> AllPeople()
        {
            var people = _db.People
                .Include(x => x.City)
                .ThenInclude(x => x.Country)
                .Where(x => x.Id == x.Id)
                .ToList();

            return people;
        }

        public Person CreatePerson(CreatePersonVM cp)
        {
            if (string.IsNullOrWhiteSpace(cp.PersonName) ||
                string.IsNullOrWhiteSpace(cp.PersonGender) ||
                string.IsNullOrWhiteSpace(cp.PersonPhoneNumber)
                )
            {
                return null;
            }
            if (cp.CityId == null || cp.CityId == 0 || cp.PersonAge == null || cp.PersonAge < 0)
            {
                return null;
            }

            var city = _db.Cities
                .Include(x=>x.Country)
                .SingleOrDefault(x => x.Id == cp.CityId);

            if (city != null)
            {
                Person person = new Person
                {
                    Name = cp.PersonName,
                    Age = (int)cp.PersonAge,
                    Gender = cp.PersonGender,
                    PhoneNumber = cp.PersonPhoneNumber,
                    City = city,
                };

                if (person != null)
                {
                    _db.People.Add(person);

                    _db.SaveChanges();

                    return person;
                }
            }
            return null;
        }

        public Person EditPerson(Person person)
        {
            if (string.IsNullOrWhiteSpace(person.Name) ||
                string.IsNullOrWhiteSpace(person.Gender) ||
                string.IsNullOrWhiteSpace(person.PhoneNumber))
            {
                return null;
            }

            var original = _db.People.SingleOrDefault(x => x.Id == person.Id);

            if (original != null)
            {
                original.Name = person.Name;
                original.Gender = person.Gender;
                original.Age = person.Age;
                original.PhoneNumber = person.PhoneNumber;

                _db.SaveChanges();

                return original;
            }
            return person;
        }

        public bool DeletePerson(int? id)
        {
            if (id != null || id != 0)
            {
                var person = _db.People.SingleOrDefault(x => x.Id == id);

                if (person != null)
                {
                    _db.People.Remove(person);

                    _db.SaveChanges();

                    return true;
                }
            }
            return false;
        }

        public Person FindPerson(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var person = _db.People.SingleOrDefault(x => x.Id == id);

            if (person != null)
            {
                return person;
            }
            return null;
        }

        public Person FindPersonWithEverything(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var person = _db.People
                .Include(x => x.City)
                .ThenInclude(x => x.Country)
                .SingleOrDefault(x => x.Id == id);

            if (person != null)
            {
                return person;
            }

            return null;
        }

        //public CreatePersonVM FindPersonAllCitiesAllCountries(CreatePersonVM cp)
        //{
        //    cp.Cities = _db.Cities.Where(x => x.Id == x.Id).ToList();
        //    cp.Countries = _db.Countries
        //        .Include(x => x.Cities)
        //        .Where(x => x.Id == x.Id).ToList();

        //    if (cp.CountryId != 0)
        //    {
        //        cp.CitiesInCountry = _db.Cities
        //            .Include(x => x.Country)
        //            .Where(x => x.Country.Id == cp.CountryId)
        //            .ToList();
        //    }

        //    if (cp.Cities.Count == 0 || cp.Countries.Count == 0)
        //    {
        //        return null;
        //    }
        //    return cp;
        //}
    }
}
