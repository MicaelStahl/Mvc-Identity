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
    public class CityRepository : ICityRepository
    {
        private readonly CountryDbContext _db;

        public CityRepository(CountryDbContext countryDbContext)
        {
            _db = countryDbContext;
        }

        public List<City> AllCities()
        {
            var cities = _db.Cities
                .Include(x => x.People)
                .ThenInclude(x => x.City)
                .Include(x => x.Country)
                .Where(x => x.Id == x.Id)
                .ToList();

            return cities;
        }

        public City CreateCity(City city)
        {
            if (string.IsNullOrWhiteSpace(city.Name) ||
                string.IsNullOrWhiteSpace(city.Population))
            {
                return null;
            }
            City newCity = new City() { Name = city.Name, Population = city.Population };

            if (newCity != null)
            {
                _db.Cities.Add(newCity);
                _db.SaveChanges();

                return newCity;
            }
            return city;
        }

        public City EditCity(City city)
        {
            if (string.IsNullOrWhiteSpace(city.Name) ||
                string.IsNullOrWhiteSpace(city.Population))
            {
                return null;
            }

            var newCity = _db.Cities.SingleOrDefault(x => x.Id == city.Id);

            if (newCity != null)
            {
                newCity.Name = city.Name;
                newCity.Population = city.Population;

                _db.SaveChanges();

                return newCity;
            }
            return city;
        }

        public bool DeleteCity(int? id)
        {
            if (id == null || id == 0)
            {
                return false;
            }

            var city = _db.Cities.SingleOrDefault(x => x.Id == id);

            if (city != null)
            {
                _db.Cities.Remove(city);

                _db.SaveChanges();

                return true;
            }
            return false;
        }

        public City FindCity(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var city = _db.Cities.SingleOrDefault(x => x.Id == id);

            if (city != null)
            {
                return city;
            }
            return null;
        }

        public City FindCityWithEverything(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var city = _db.Cities
                .Include(x => x.People)
                .ThenInclude(x=>x.City)
                .Include(x => x.Country)
                .ThenInclude(x=>x.Cities)
                .SingleOrDefault(x => x.Id == id);

            if (city != null)
            {
                return city;
            }

            return null;
        }

        public AddPeopleToCityVM FindCityAndAllHomeless(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var cityVM = new AddPeopleToCityVM();

            cityVM.City = _db.Cities.SingleOrDefault(x => x.Id == id);

            var allPeople = _db.People
                .Include(x=>x.City)
                .Where(x => x.Id == x.Id).ToList();

            foreach (var item in allPeople)
            {
                if (item.City == null)
                {
                    cityVM.Homeless.Add(item);
                }
            }

            if (cityVM.City != null || cityVM.Residents.Count != 0)
            {
                return cityVM;
            }

            return null;
        }

        public bool AddPeopleToCity(int? cityId, List<int> studentId)
        {
            if (cityId == null || cityId == 0)
            {
                return false;
            }
            if (studentId.Count == 0)
            {
                return false;
            }

            var city = _db.Cities.SingleOrDefault(x => x.Id == cityId);

            if (city != null)
            {
                foreach (var item in studentId)
                {
                    var student = _db.People.SingleOrDefault(x => x.Id == item);

                    if (student != null)
                    {
                        student.City = city;
                        city.People.Add(student);
                    }
                }
                _db.SaveChanges();

                return true;
            }

            return false;
        }
    }
}
