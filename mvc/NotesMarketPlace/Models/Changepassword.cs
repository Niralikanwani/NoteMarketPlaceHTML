using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class Changepassword
    {
        [Required(ErrorMessage = "Please enter Old password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please enter your New password")]
        [StringLength(24, ErrorMessage = "The {0} Must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Enter 6 to 24 digit,one upper latter,one lower latter,one special character")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Please Re-enter your password")]
        [Compare("NewPassword", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }

    }
}