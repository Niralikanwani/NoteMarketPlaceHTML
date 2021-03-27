using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class Addtype
    {
        public int Id { get; set; }

        public string TypeName { get; set; }

        public string Description { get; set; }



        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateEdited { get; set; }
        public Nullable<int> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}