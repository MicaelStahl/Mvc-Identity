using Lexicon.CSharp.InfoGenerator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Mvc_Identity.DataBase;
using Mvc_Identity.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity
{
    internal class CountryDbInitializer
    {
        internal static void Initialize(CountryDbContext db, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            //InfoGenerator IG = new InfoGenerator();

            db.Database.EnsureCreated();

            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                IdentityRole role = new IdentityRole("Administrator");

                roleManager.CreateAsync(role).Wait();
            }

            if (!roleManager.RoleExistsAsync("NormalUser").Result)
            {
                IdentityRole role = new IdentityRole("NormalUser");

                roleManager.CreateAsync(role).Wait();
            }

            //----------------- New Section ----------------------

            if (userManager.FindByNameAsync("Micael").Result == null)
            {
                AppUser user = new AppUser();
                user.Email = "Micael@Administrator.com";
                user.Admin = true;
                user.UserName = "Micael";
                user.FirstName = "Micael";
                user.SecondName = "Ståhl";
                user.PhoneNumber = "0725539574";

                var result = userManager.CreateAsync(user, "Password!23").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "NormalUser").Wait();
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
                }
            }

            if (userManager.FindByNameAsync("Rikke").Result == null)
            {
                AppUser user = new AppUser();
                user.Email = "Rikke@NormalUser.com";
                user.UserName = "Rikke";
                user.FirstName = "Rikke";
                user.SecondName = "Frederiksen";
                user.PhoneNumber = "123456789";

                var result = userManager.CreateAsync(user, "Password!23").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, "NormalUser").Wait();
                }
            }

            //----------------- New Section ----------------------

            if (!db.Countries.Any())
            {
                var countries = new Country[]
                {
                    new Country{Name="Sweden", Population="9,2Million" },
                    new Country{Name="Denmark", Population="5,5Million" },
                    new Country{Name="USA", Population="532Million" },
                };

                db.Countries.AddRange(countries);

                db.SaveChanges();

                if (!db.Cities.Any())
                {
                    var cities = new City[]
                    {
                        new City{Name="Vetlanda", Population="13,123", Country=countries[0]},
                        new City{Name="Viborg", Population="32,361", Country=countries[1]},
                        new City{Name="Växjö", Population="68,213", Country=countries[0]},
                        new City{Name="Washington DC", Population="1,013,123", Country=countries[2]},
                    };
                    db.Cities.AddRange(cities);

                    db.SaveChanges();

                    if (!db.People.Any())
                    {
                        var people = new Person[]
                        {
                            new Person{Name="Micael Ståhl", Age=22, Gender="Male", PhoneNumber="123456789", City = cities[0]},
                            new Person{Name="Rikke Frederiksen", Age=24, Gender="Female", PhoneNumber="123456789", City = cities[1]},
                            new Person{Name="Emma Ståhl", Age=19, Gender="Female", PhoneNumber="987654321", City = cities[0]},
                            new Person{Name="Adam Adamsson", Age=42, Gender="Llama", PhoneNumber="291939212", City = cities[2]},
                            new Person{Name="Abraham Lincoln", Age=53, Gender="Male", PhoneNumber="32142132", City = cities[3]}
                        };

                        db.People.AddRange(people);

                        db.SaveChanges();
                    }
                }
            }
        }
    }
}
