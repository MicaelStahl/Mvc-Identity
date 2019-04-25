using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mvc_Identity.Interfaces;
using Mvc_Identity.ViewModels;

namespace Mvc_Identity.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        private readonly IPersonRepository _person;

        public PersonController(IPersonRepository person)
        {
            _person = person;
        }

        public IActionResult Index()
        {
            return View(_person.AllPeople());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([Bind("Name, Age, Gender, PhoneNumber")]Person person)
        {
            if (ModelState.IsValid)
            {
                var newPerson = _person.CreatePerson(person);

                if (newPerson != null)
                {
                    return RedirectToAction(nameof(Details), "Person", new { id = newPerson.Id });
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult CreatePerson(int? countryId)
        {
            CreatePersonVM cp = new CreatePersonVM();

            if (countryId != null || countryId != 0)
            {
            }
            else
            {
                cp.CountryId = (int)countryId;
            }
            cp = _person.FindPersonAllCitiesAllCountries(cp);

            return View(cp);
        }
        [HttpPost]
        public IActionResult CreatePerson(CreatePersonVM cp)
        {
            if (ModelState.IsValid)
            {
                var person = _person.CreatePerson(cp.Person);

                if (person != null)
                {
                    if (cp.City != null || cp.Country != null)
                    {
                        person.City = cp.City;
                        person.City.Country = cp.Country;
                    }
                    return RedirectToAction(nameof(Index), "Person", new { id = person.Id });
                }
            }
            return BadRequest();
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {
            if (id != null || id != 0)
            {
                var student = _person.FindPersonWithEverything(id);

                if (student != null)
                {
                    return View(student);
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
                var person = _person.FindPerson(id);

                if (person != null)
                {
                    return View(person);
                }
                return NotFound();
            }
            return BadRequest();
        }
        [HttpPost]
        public IActionResult Edit([Bind("Name, Age, Gender, PhoneNumber")]Person person)
        {
            if (ModelState.IsValid)
            {
                var newPerson = _person.EditPerson(person);

                if (newPerson != null)
                {
                    return RedirectToAction(nameof(Details), "Person", new { id = person.Id });
                }
                return NotFound();
            }
            return BadRequest();
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        public IActionResult Delete(int? id)
        {
            if (id != null || id != 0)
            {
                var person = _person.FindPerson(id);

                if (person != null)
                {
                    return View(person);
                }
                return NotFound();
            }
            return BadRequest();
        }
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteConfirmed(int? id)
        {
            if (id != null || id != 0)
            {
                var boolean = _person.DeletePerson(id);

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