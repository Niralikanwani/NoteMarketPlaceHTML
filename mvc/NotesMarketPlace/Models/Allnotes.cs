using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class Allnotes
    {
        public tbl_Notes SellerNotes { get; set; }
        public tbl_NoteCategory Category { get; set; }
        public tbl_ReferenceData status { get; set; }
        public tbl_Users user { get; set; }
        public tbl_Download downloadNote { get; set; }
        public tbl_NoteAttachments Attachmnet { get; set; }
        public tbl_UserProfile uprofiledata { get; set; }
        public tbl_Country country { get; set; }
       


        public tbl_NoteReviews notereview { get; set; }
    }
}