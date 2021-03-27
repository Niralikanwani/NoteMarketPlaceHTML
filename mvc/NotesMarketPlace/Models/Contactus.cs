using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class Contactus
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        [EmailAddress(ErrorMessage = "Please enter valid email")]
        public string Email_id { get; set; }

        public string Subject { get; set; }

        public string Question { get; set; }



        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateEdited { get; set; }
        public Nullable<int> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}