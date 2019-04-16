using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.ViewModels
{
    public class City
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Population { get; set; }

        public List<Person> People { get; } = new List<Person>();

        public Country Country { get; set; }
    }
}
