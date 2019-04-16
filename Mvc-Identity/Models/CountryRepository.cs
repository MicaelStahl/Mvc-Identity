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
            return _db.Countries.ToList();
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
