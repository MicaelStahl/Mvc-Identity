using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Mvc_Identity.DataBase;
using Mvc_Identity.ViewModels;

namespace Mvc_Identity.Controllers
{
    public class JSONController : Controller
    {
        private readonly CountryDbContext _db;

        public JSONController(CountryDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetCountries()
        {
            var countries = _db.Countries
                .Where(x => x.Id == x.Id).ToList();

            return Json(countries);
        }
        [HttpPost]
        public JsonResult GetCities(string countryName)
        {
            if (!string.IsNullOrWhiteSpace(countryName))
            {
                var result = (from Cities in _db.Cities
                              where Cities.Country.Name == countryName
                              select Cities).ToList();

                return Json(result);
            }
            return Json(countryName);
        }
    }
}