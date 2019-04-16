using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mvc_Identity.Interfaces;
using Mvc_Identity.ViewModels;

namespace Mvc_Identity.Controllers
{
    public class CountryController : Controller
    {
        private readonly ICountryRepository _country;

        public CountryController(ICountryRepository country)
        {
            _country = country;
        }

        [AutoValidateAntiforgeryToken]
        public IActionResult Index()
        {
            return View(_country.AllCountries());
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create([Bind("Name, Population")]Country country)
        {
            if (ModelState.IsValid)
            {
                var newCountry = _country.CreateCountry(country);

                if (newCountry != null)
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return BadRequest();
        }

        [AutoValidateAntiforgeryToken]
        public IActionResult Details(int? id)
        {
            if (id != null || id != 0)
            {
                var country = _country.FindCountry(id);

                if (country != null)
                {
                    return View(country);
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit(int? id)
        {
            if (id != null || id != 0)
            {
                var country = _country.FindCountry(id);

                if (country != null)
                {
                    return View(country);
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Edit([Bind("Name, Population")]Country country)
        {
            if (ModelState.IsValid)
            {
                var newCountry = _country.EditCountry(country);

                if (newCountry != null)
                {
                    return RedirectToAction(nameof(Details), "Country", new { id = newCountry.Id });
                }
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete(int? id)
        {
            if (id != null || id != 0)
            {
                var country = _country.FindCountry(id);

                if (country != null)
                {
                    return View(country);
                }
                return NotFound();
            }
            return BadRequest();
        }
        [HttpPost, ActionName("Delete")]
        [AutoValidateAntiforgeryToken]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id != null || id != 0)
            {
                var boolean = _country.DeleteCountry(id);

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