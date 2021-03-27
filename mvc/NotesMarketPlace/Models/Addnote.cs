using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace NotesMarketPlace.Models
{
    public class Addnote
    {
        public int Id { get; set; }
        public int Seller_id { get; set; }
        public int Status { get; set; }
        public Nullable<int> StatusActionedBy { get; set; }
        public string AdminRemarks { get; set; }
        public Nullable<System.DateTime> PublishedDate { get; set; }
        [Required(ErrorMessage = "Please Enter Note Title")]
        public string NoteTitle { get; set; }

        [Required(ErrorMessage = "Please Select Note Category")]
        public int NoteCategory_id { get; set; }

        public HttpPostedFileBase DisplayPicture { get; set; }


        public List<HttpPostedFileBase> NoteAttachment { get; set; }

        [Required(ErrorMessage = "Please select Note Type")]
        public int NoteType_id { get; set; }

        public Nullable<int> NumberOfPages { get; set; }

        [Required(ErrorMessage = "Please Enter Note Description")]
        public string Description { get; set; }
        public string University { get; set; }

        [Required(ErrorMessage = "Please Select Country")]
        public Nullable<int> Country_id { get; set; }
        public string Course { get; set; }
        public string CourseCode { get; set; }
        public string Professor { get; set; }

        [Required(ErrorMessage = "Please Select Selling Type")]
        public Boolean IsPaid { get; set; }

        [Required(ErrorMessage = "Please Enter Selling Price")]
        public decimal SellingPrice { get; set; }


        public HttpPostedFileBase NotesPreview { get; set; }


    }
}