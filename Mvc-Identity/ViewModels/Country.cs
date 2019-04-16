using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.ViewModels
{
    public class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Population { get; set; }

        public List<City> Cities { get; } = new List<City>();
    }
}
