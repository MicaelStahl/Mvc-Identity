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
        [StringLength(60, MinimumLength =2, ErrorMessage ="Your name has to be between 2 to 60 characters long.")]
        public string PersonName { get; set; }

        [Required]
        [Display(Name = "Age")]
        [Range(0, 120, ErrorMessage = "Please write a age between 0 to 120.")]
        public int? PersonAge { get; set; }

        [Required]
        [Display(Name = "Your gender")]
        public string PersonGender { get; set; }

        [Required]
        [Display(Name = "Phonenumber")]
        [StringLength(12, MinimumLength =5,ErrorMessage ="Please write a phonenumber between 5 to 12 numbers long.")]
        public string PersonPhoneNumber { get; set; }

        public int? CityId { get; set; }
    }
}
