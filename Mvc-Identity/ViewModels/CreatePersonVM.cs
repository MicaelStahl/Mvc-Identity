using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Mvc_Identity.ViewModels
{
    public class CreatePersonVM
    {
        [Required]
        [Display(Name = "Full name")]
        public string PersonName { get; set; }

        [Required]
        [Display(Name = "Age")]
        [Range(0, 120, ErrorMessage = "Please write a valid age.")]
        public int? PersonAge { get; set; }

        [Required]
        [Display(Name = "Your gender")]
        public string PersonGender { get; set; }

        [Required]
        [Display(Name = "Phonenumber")]
        public string PersonPhoneNumber { get; set; }

        [Required]
        public int? CityId { get; set; }
    }
}
