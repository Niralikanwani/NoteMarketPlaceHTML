using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NoteMarketPlace.Models
{
    public class Addcategory
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public string Description { get; set; }



        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<int> AddedBy { get; set; }
        public Nullable<System.DateTime> DateEdited { get; set; }
        public Nullable<int> EditedBy { get; set; }
        public bool IsActive { get; set; }
    }
}