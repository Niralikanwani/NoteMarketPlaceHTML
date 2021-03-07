using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class login
    {
        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        public string Email_id { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        [StringLength(24, ErrorMessage = "The {0} Must be at least {2} characters long.", MinimumLength = 6)]
        [RegularExpression(@"^((?=.*[a-z])(?=.*[A-Z])(?=.*\d)).+$", ErrorMessage = "Enter 6-24 digit,1 upper letter,1 lower latter,1 special character")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
        public object FirstName { get; internal set; }
        public object LastName { get; internal set; }
    }
}