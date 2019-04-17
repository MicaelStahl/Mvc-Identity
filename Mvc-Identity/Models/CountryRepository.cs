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
    public class CountryRepository : ICountryRepository
    {
        private readonly CountryDbContext _db;

        public CountryRepository(CountryDbContext countryDbContext)
        {
            _db = countryDbContext;
        }

        public List<Country> AllCountries()
        {
            var countries = _db.Countries
                .Include(x => x.Cities)
                .ThenInclude(x => x.People)
                .ThenInclude(x => x.City)
                .Where(x => x.Id == x.Id)
                .ToList();

            return countries;
        }

        public bool AddCitiesToCountry(int? id, List<int> cityId)
        {
            if (id == null || id == 0)
            {
                return false;
            }
            if (cityId.Count == 0)
            {
                return false;
            }

            var country = _db.Countries.SingleOrDefault(x => x.Id == id);

            if (country != null)
            {
                foreach (var item in cityId)
                {
                    var city = _db.Cities.SingleOrDefault(x => x.Id == item);

                    if (city != null)
                    {
                        city.Country = country;
                        country.Cities.Add(city);
                    }
                }
                _db.SaveChanges();

                return true;
            }

            return false;
        }

        public Country CreateCountry(Country country)
        {
            if (string.IsNullOrWhiteSpace(country.Name) ||
                string.IsNullOrWhiteSpace(country.Population))
            {
                return null;
            }
            Country newCountry = new Country() { Name = country.Name, Population = country.Population };

            if (newCountry != null)
            {
                _db.Countries.Add(newCountry);
                _db.SaveChanges();

                return newCountry;
            }
            return country;
        }

        public Country EditCountry(Country country)
        {
            if (string.IsNullOrWhiteSpace(country.Name) ||
                string.IsNullOrWhiteSpace(country.Population))
            {
                return null;
            }

            var newCountry = _db.Countries.SingleOrDefault(x => x.Id == country.Id);

            if (newCountry != null)
            {
                newCountry.Name = country.Name;
                newCountry.Population = country.Population;

                _db.SaveChanges();

                return newCountry;
            }
            return country;
        }

        public Country FindCountry(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }
            var country = _db.Countries.SingleOrDefault(x => x.Id == id);

            if (country == null)
            {
                return null;
            }
            return country;
        }

        public Country FindCountryWithEverything(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var city = _db.Countries
                .Include(x => x.Cities)
                .ThenInclude(x => x.People)
                .ThenInclude(x=>x.City)
                .SingleOrDefault(x => x.Id == id);

            if (city != null)
            {
                return city;
            }

            return null;
        }

        public AddCitiesToCountryVM FindCountryAndAllRogueCities(int? id)
        {
            if (id == null || id == 0)
            {
                return null;
            }

            var countryVM = new AddCitiesToCountryVM();

            countryVM.Country = _db.Countries.SingleOrDefault(x => x.Id == id);

            var allCities = _db.Cities.Where(x => x.Id == x.Id).ToList();

            if (countryVM.Country != null)
            {
                if (allCities.Count != 0)
                {
                    foreach (var item in allCities)
                    {
                        if (item.Country == null)
                        {
                            countryVM.Cities.Add(item);
                        }
                    }
                    return countryVM;
                }
            }

            return null;
        }

        public bool DeleteCountry(int? id)
        {
            if (id == null || id == 0)
            {
                return false;
            }

            var country = _db.Countries.SingleOrDefault(x => x.Id == id);

            if (country != null)
            {
                _db.Countries.Remove(country);
                _db.SaveChanges();

                return true;
            }
            return false;
        }
    }
}
