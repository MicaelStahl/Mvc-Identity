﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.ViewModels
{
    public class ListsVM
    {
        public List<City> Cities { get; set; } = new List<City>();

        public List<Person> People { get; set; } = new List<Person>();

        public List<Country> Countries { get; set; } = new List<Country>();
    }

    public class AddPeopleToCityVM
    {
        public List<Person> Residents { get; set; } = new List<Person>();

        public List<Person> Homeless { get; set; } = new List<Person>();

        public List<int> StudentId { get; set; } = new List<int>();

        public City City { get; set; }

        public bool Selected { get; set; }
    }

    public class AddCitiesToCountryVM
    {
        public List<City> Cities { get; set; } = new List<City>();

        public List<City> CountryLess { get; set; } = new List<City>();

        public List<int> CityId { get; set; } = new List<int>();

        public Country Country { get; set; }
    }

    public class CreatePersonVM
    {
        public Person Person { get; set; }

        public City City { get; set; }
        public List<City> Cities { get; set; } = new List<City>();

        public int CountryId { get; set; }
        public Country Country { get; set; }
        public List<Country> Countries { get; set; } = new List<Country>();

        public List<City> CitiesInCountry { get; set; } = new List<City>();
    }
}
