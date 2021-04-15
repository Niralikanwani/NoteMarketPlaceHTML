using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NotesMarketPlace.Models
{
    public class DisplayNotes
    {
        public tbl_Notes SellerNotes { get; set; }
        public tbl_NoteCategory Category { get; set; }
        public tbl_ReferenceData status { get; set; }
        public tbl_Users user { get; set; }
        public tbl_Download downloadNote { get; set; }
        public tbl_NoteAttachments Attachmnet { get; set; }
        public tbl_UserProfile uprofiledata { get; set; }
        public tbl_Country country { get; set; }
        public tbl_Users buyer { get; set; }
        public tbl_ReportedNotes spam { get; set; }
        public tbl_NoteReviews notereview { get; set; }
    }

    public class NoteCategory
    {
        public tbl_Notes SellerNotes { get; set; }
        public tbl_NoteCategory Category { get; set; }
    }

    public class SellerBuyerNotes
    {
        public string Seller { get; set; }
        public string Buyer { get; set; }
        public string Note { get; set; }

    }

    public class TypeCountryCategoryUser
    {
        public tbl_NoteType types { get; set; }
        public tbl_NoteCategory categorydata { get; set; }
        public tbl_Country countrydata { get; set; }
        public tbl_Users user { get; set; }
    }

    public class dn
    {
        public tbl_Notes note { get; set; }
        public tbl_NoteCategory Category { get; set; }
        public tbl_Country countryname { get; set; }
        public tbl_NoteReviews nr { get; set; }
    }
}