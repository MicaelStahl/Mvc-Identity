using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.ViewModels
{
    public class AppUser : IdentityUser
    {
        public bool Admin { get; set; }
    }
}
