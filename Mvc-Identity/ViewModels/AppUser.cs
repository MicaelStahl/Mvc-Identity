using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.ViewModels
{
    public class AppUser : IdentityUser
    {
        [Required]
        [Display(Name = "Firstname")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name has to be between 2 to 20 letters long.")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Secondname")]
        [StringLength(20, MinimumLength = 2, ErrorMessage = "Name has to be between 2 to 20 letters long.")]
        public string SecondName { get; set; }

        [Required]
        public bool Admin { get; set; }
    }
}
