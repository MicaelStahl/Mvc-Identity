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
        [HttpGet]
        public JsonResult GetCities(string country)
        {
            var cities = new List<City>();

            if (!string.IsNullOrWhiteSpace(country))
            {
                //using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(country)))
                //{
                //    StreamWriter sw = new StreamWriter(ms);

                //    DataContractJsonSerializer deserializer = new DataContractJsonSerializer(typeof(JsonDeSerializer));
                //    JsonDeSerializer jsonDeSerializer = (JsonDeSerializer)deserializer.ReadObject(ms);
                //    sw.WriteLine("country: " + jsonDeSerializer.Name);

                    cities = _db.Cities
                        .Include(x => x.Country)
                        .Include(x=>x.People)
                        .ThenInclude(x=>x.City)
                        .Where(x => x.Country.Name.ToLower() == country.ToLower())
                        .ToList();
                //}
            }

            return Json(cities);
        }
    }
}