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

        City CreateCity(City city);

        City EditCity(City city);

        City AddPeopleToCity(int? cityId, List<int> studentId);

        bool DeleteCity(int? id);

        City FindCity(int? id);

        AddPeopleToCityVM FindCityAndAllHomeless(int? id);


    }
}
