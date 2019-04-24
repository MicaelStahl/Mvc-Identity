using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mvc_Identity.Interfaces;
using Mvc_Identity.ViewModels;

namespace Mvc_Identity.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CityController : Controller
    {

        private const string SessionKeyAddPeople = "_AddPeople";

        private readonly ICityRepository _city;

        public CityController(ICityRepository city)
        {
            _city = city;
        }

        public IActionResult Index()
        {
            return View(_city.AllCities());
        }

        [HttpGet]
        public IActionResult AddPeopleToCity(int? id)
        {
            if (id != null || id != 0)
            {
                var cityVM = new AddPeopleToCityVM();

                cityVM = _city.FindCityAndAllHomeless(id);

                if (cityVM.City != null)
                {
                    return View(cityVM);
                }
                return NotFound();
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult AddPeopleToCity(int? cityId, List<int> studentId)
        {
            if (cityId != null || cityId != 0)
            {
                if (studentId.Count != 0)
                {
                    var boolean = _city.AddPeopleToCity(cityId, studentId);

                    if (boolean)
                    {
                        return RedirectToAction(nameof(Details), "City", new { id = cityId });
                    }
                }
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Name, Population")]City city)
        {
            if (ModelState.IsValid)
            {
                var newCity = _city.CreateCity(city);

                if (newCity != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id != null || id != 0)
            {
                var city = _city.FindCityWithEverything(id);

                if (city != null)
                {
                    return View(city);
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id != null || id != 0)
            {
                var city = _city.FindCity(id);

                if (city != null)
                {
                    return View(city);
                }
                return NotFound();
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult Edit([Bind("Name, Population")] City city)
        {
            if (ModelState.IsValid)
            {
                var newCity = _city.EditCity(city);

                if (newCity != null)
                {
                    return RedirectToAction(nameof(Details), "City", new { id = city.Id });
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Delete(int? id)
        {
            if (id != null || id != 0)
            {
                var city = _city.FindCity(id);

                if (city != null)
                {
                    return View(city);
                }
                return NotFound();
            }
            return BadRequest();
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id != null || id != 0)
            {
                var boolean = _city.DeleteCity(id);

                if (boolean)
                {
                    return RedirectToAction(nameof(Index));
                }
                return NotFound();
            }
            return BadRequest();
        }
    }
}