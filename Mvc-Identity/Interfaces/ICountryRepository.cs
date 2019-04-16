﻿using Mvc_Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.Interfaces
{
    public interface ICountryRepository
    {
        List<Country> AllCountries();

        Country CreateCountry(Country country);

        Country EditCountry(Country country);

        bool DeleteCountry(int? id);

        Country FindCountry(int? id);

        //List<Country> SortList(int? id, char choice);

        //bool FilterList(string filter);
    }
}
