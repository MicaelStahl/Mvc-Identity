using Lexicon.CSharp.InfoGenerator;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.DataBase
{
    public static class CountryDbInitializer
    {
        public static void Initialize(CountryDbContext db)
        {
            InfoGenerator IG = new InfoGenerator();

            db.Database.EnsureCreated();

            if (!db.Roles.Any())
            {

            }
        }
    }
}
