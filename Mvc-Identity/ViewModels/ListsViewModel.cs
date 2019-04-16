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
        public List<Person> People { get; set; } = new List<Person>();

        public City City { get; set; }
    }

    public class AddCitiesToCountryVM
    {
        public List<City> Cities { get; set; } = new List<City>();

        public Country Country { get; set; }
    }
}
