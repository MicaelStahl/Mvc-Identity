using Mvc_Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.Interfaces
{
    public interface ICityRepository
    {
        List<City> AllCities();

        bool AddPeopleToCity(int? cityId, List<int> studentId);

        City CreateCity(City city);

        City EditCity(City city);

        bool DeleteCity(int? id);

        City FindCity(int? id);

        City FindCityWithEverything(int? id);

        AddPeopleToCityVM FindCityAndAllHomeless(int? id);


    }
}
