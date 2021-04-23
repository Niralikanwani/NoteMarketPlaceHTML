using NotesMarketPlace.EmailTemplates;
using NotesMarketPlace.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO.Compression;

namespace NotesMarketPlace.Controllers
{
    public class AdminController : Controller
    {

        [Authorize]
        public ActionResult forgot()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult forgot(tbl_Users user)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                string allowedChars = "";
                string passwordString = "";
                string temp = "";

                bool isValid = nm.tbl_Users.Any(x => x.Email_id == user.Email_id);
                if (isValid)
                {
                    tbl_Users u = nm.tbl_Users.Where(x => x.Email_id == user.Email_id).FirstOrDefault();

                    allowedChars = "a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,";

                    allowedChars += "A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z,";

                    allowedChars += "1,2,3,4,5,6,7,8,9,0,!,@,#,$,%,&,?";

                    char[] sep = { ',' };

                    string[] arr = allowedChars.Split(sep);





                    Random rand = new Random();

                    for (int i = 0; i < 6; i++)

                    {

                        temp = arr[rand.Next(0, arr.Length)];

                        passwordString += temp;

                    }

                    //  Save Password 

                    u.Password = passwordString;
                    nm.SaveChanges();

                    //Sending new password on mail
                    forgotpassword.SendOtpToEmail(u, passwordString);

                    TempData["Message"] = "Otp Sent To Your Registered EmailAddress use it for login";
                    return RedirectToAction("Login", "User");
                }
                TempData["Message"] = "Invalid EmailAddress";
                return View();


            }
        }

        [Authorize]
        //get
        public ActionResult changepassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult changepassword(Changepassword cp)
        {

            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];
                tbl_Users u = nm.tbl_Users.Where(x => x.Id == id).FirstOrDefault();
                if (u.Password == cp.Password)
                {
                    u.Password = cp.NewPassword;
                    nm.SaveChanges();
                    ViewBag.PassMessage = "<p><span><i class='fas fa-check-circle'></i></span> Your Password has been Changed successfully</p>";


                }

            }
            return View();
        }

        [Authorize]
        public ActionResult addadministrator()
        {
            using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
            {
                var countrycode = DBobj.tbl_Country.ToList();
                ViewBag.countryCode = new SelectList(countrycode, "CountryCode", "CountryCode");
                return View();
            }

        }

        [Authorize]
        // POST: Admin
        [HttpPost]
        public ActionResult addadministrator(AddAdministratorModel model)
        {
            if (ModelState.IsValid)
            {

                using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())

                {
                    tbl_Users u = new tbl_Users();
                    u.FirstName = model.FirstName;
                    u.LastName = model.LastName;
                    u.Email_id = model.Email_id;
                    u.Role_id = 2;
                    u.IsActive = true;
                    u.DateAdded = DateTime.Now;
                    u.Password = "admin";
                    u.IsEmailVerified = true;

                    DBobj.tbl_Users.Add(u);
                    DBobj.SaveChanges();

                    if (u.Id > 0)
                    {
                        tbl_UserProfile up = new tbl_UserProfile();
                        up.User_id = u.Id;
                        up.PhoneCountryCode = model.CountryCode;
                        up.PhoneNumber = model.PhoneNumber;
                        up.AddressLine1 = "null";
                        up.City = "null";
                        up.State = "null";
                        up.ZipCode = "null";
                        up.Country = 1;
                        up.DateAdded = DateTime.Now;

                        DBobj.tbl_UserProfile.Add(up);
                        DBobj.SaveChanges();
                        ModelState.Clear();
                        var countrycode = DBobj.tbl_Country.ToList();
                        ViewBag.countryCode = new SelectList(countrycode, "CountryCode", "CountryCode");
                        ViewBag.IsSuccess = "<p><span><i class='fas fa-check-circle'></i></span> Admin added successfully.</p>";
                    }
                }

            }

            return View();
        }

        [Authorize]
        [Route("addcategory/id")]
        //get
        public ActionResult addcategory(int? id)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();
            tbl_NoteCategory catdata = nm.tbl_NoteCategory.Where(x => x.Id == id).FirstOrDefault();
            return View(catdata);
        }

        [Authorize]
        [HttpPost]
        public ActionResult addcategory(tbl_NoteCategory catergory)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {

                tbl_NoteCategory ng = new tbl_NoteCategory();

                ng.CategoryName = catergory.CategoryName;
                ng.Description = catergory.Description;
                ng.IsActive = true;
                ng.DateAdded = DateTime.Now;
                ng.AddedBy = (int)Session["Userid"];
                ng.EditedBy = (int)Session["Userid"];
                ng.DateEdited = DateTime.Now;
                nm.tbl_NoteCategory.Add(ng);
                nm.SaveChanges();
                ModelState.Clear();

            }
            return View();
        }

        [Authorize]
        [Route("addcountry/id")]
        //get
        public ActionResult addcountry(int? id)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();
            tbl_Country cdata = nm.tbl_Country.Where(x => x.Id == id).FirstOrDefault();

            return View(cdata);

        }

        [Authorize]
        [HttpPost]
        public ActionResult addcountry(tbl_Country c)
        {
            if (ModelState.IsValid)
            {
                using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                {
                    tbl_Country cr = new tbl_Country();
                    cr.CountryCode = c.CountryCode;
                    cr.Name = c.Name;
                    cr.IsActive = true;
                    cr.AddedBy = (int)Session["Userid"];
                    cr.EditedBy = (int)Session["Userid"];
                    cr.DateEdited = DateTime.Now;
                    cr.DateAdded = DateTime.Now;
                    nm.tbl_Country.Add(cr);
                    nm.SaveChanges();
                }
            }

            return View();
        }

        [Authorize]
        [Route("addType/id")]
        public ActionResult addtype(int? id)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();
            tbl_NoteType typedata = nm.tbl_NoteType.Where(x => x.Id == id).FirstOrDefault();
            return View(typedata);


        }

        [Authorize]
        [HttpPost]
        public ActionResult addtype(tbl_NoteType type)
        {
            if (ModelState.IsValid)
            {
                using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                {
                    tbl_NoteType note = new tbl_NoteType();
                    note.TypeName = type.TypeName;
                    note.Description = type.Description;
                    note.isActive = true;
                    note.AddedBy = (int)Session["Userid"];
                    note.EditedBy = (int)Session["Userid"];
                    note.DateEdited = DateTime.Now;
                    note.DateAdded = DateTime.Now;
                    nm.tbl_NoteType.Add(note);
                    nm.SaveChanges();
                    ModelState.Clear();
                }

            }
            return View();
        }

        [Authorize]
        // GET: ManageType
        public ActionResult managetype(string typesearch, int? page)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();


            var typeuser = (from n in nm.tbl_NoteType.Where(x => x.TypeName.StartsWith(typesearch) || typesearch == null).ToList()
                            join u in nm.tbl_Users.ToList() on n.AddedBy equals u.Id
                            select new TypeCountryCategoryUser
                            {
                                types = n,
                                user = u
                            });
            ViewBag.tulist = typeuser.ToPagedList(page ?? 1, 5);
            ViewBag.tulistCount = typeuser.Count();
            return View();


        }

        [Authorize]
        // POST: ManageType
        [HttpPost]
        public ActionResult managetype(tbl_NoteType model)
        {
            return View();
        }

        [Authorize]
        // GET: ManageCategory
        public ActionResult managecategory(string catsearch, int? page)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();

            var catusr = (from c in nm.tbl_NoteCategory.Where(x => x.CategoryName.StartsWith(catsearch) || catsearch == null).ToList()
                          join u in nm.tbl_Users.ToList() on c.AddedBy equals u.Id
                          select new TypeCountryCategoryUser
                          {
                              categorydata = c,
                              user = u
                          });
            ViewBag.culist = catusr.ToPagedList(page ?? 1, 5);
            ViewBag.culistCount = catusr.Count();

            return View();

        }

        [Authorize]
        // POST: ManageCategory
        [HttpPost]
        public ActionResult managecategory(tbl_NoteCategory model)
        {
            return View();
        }

        [Authorize]
        // GET: ManageCountry
        public ActionResult managecountry(string countysearch, int? page)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                var countrylist = nm.tbl_Country.Where(x => x.Name.StartsWith(countysearch) || countysearch == null).ToList();
                var usr = nm.tbl_Users.ToList();
                var cousr = (from co in countrylist
                             join u in usr on co.AddedBy equals u.Id into table1
                             from u in table1.ToList()
                             select new TypeCountryCategoryUser
                             {
                                 countrydata = co,
                                 user = u
                             });
                ViewBag.colist = cousr.ToPagedList(page ?? 1, 5);
                ViewBag.colistCount = cousr.Count();
                return View();
            }
        }

        [Authorize]
        // POST: ManageCountry
        [HttpPost]
        public ActionResult managecountry(tbl_Country model)
        {
            return View();
        }

        [Authorize]
        public ActionResult dashboard(int? page, string dashsearch, int? Month)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();

            var day = DateTime.Now.AddDays(-7);
            ViewBag.NoteUnderReview = nm.tbl_Notes.Where(x => x.Status == 4 && x.IsActive == true).Count();
            ViewBag.LastDownLoadedNote = nm.tbl_Download.Where(x => x.IsSellerHasAllowedDownload == true && x.DateAdded > day).Count();
            ViewBag.LastRegisteredUsers = nm.tbl_Users.Where(x => x.Role_id == 3 && x.DateAdded > day).Count();

            var categories = nm.tbl_NoteCategory.ToList();
            var notes = nm.tbl_Notes.Where(x => x.Status == 2 &&
        (x.NoteTitle.StartsWith(dashsearch) || dashsearch == null) && (x.PublishedDate.Value.Month == Month || String.IsNullOrEmpty(Month.ToString()))).ToList();

            var users = nm.tbl_Users.ToList();
            var dnotes = nm.tbl_Download.Where(x => x.IsSellerHasAllowedDownload == true).ToList();
            var attachmentNote = nm.tbl_NoteAttachments.ToList();
            var pn = (from n in notes
                      join ct in categories on n.NoteCategory_id equals ct.Id into table1
                      from ct in table1.ToList()
                      join usr in users on n.Seller_id equals usr.Id into table2
                      from usr in table2.ToList()
                      join anote in attachmentNote on n.Id equals anote.Note_id into table3
                      from anote in table3.ToList()
                      select new DisplayNotes
                      {
                          Category = ct,
                          user = usr,
                          Attachmnet = anote,
                          SellerNotes = n
                      });

            ViewBag.publishednotes = pn.ToPagedList(page ?? 1, 5);
            ViewBag.publishednotesCount = pn.Count();





            return View();
        }

        [Authorize]
        [Route("unpublishNote/id")]
        //GET: DeleteAdmin
        public ActionResult unpublishNote(int id, string adminRemarks)
        {
            using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
            {
                tbl_Notes note = DBobj.tbl_Notes.FirstOrDefault(x => x.Id == id);
                tbl_Users u = DBobj.tbl_Users.Where(x => x.Id == note.Seller_id).FirstOrDefault();
                if (note != null)
                {
                    note.Status = 1005;
                    note.AdminRemarks = adminRemarks;
                    note.PublishedDate = DateTime.Now;
                    note.StatusActionedBy = (int)Session["UserID"];
                    note.EditedBy = (int)Session["UserID"];
                    note.DateEdited = DateTime.Now;
                    DBobj.SaveChanges();
                    unpublishSellerNote.unpublishNote(u.FirstName, u.Email_id, adminRemarks);
                }
                return RedirectToAction("dashboard", "Admin");
            }
        }

        [Authorize]
        public ActionResult members(int? page, string membersearch)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();


            var members = nm.tbl_Users.Where(x => x.Role_id == 3 && ((x.FirstName + " " + x.LastName).StartsWith(membersearch) || membersearch == null)).ToList();

            ViewBag.members = members.ToPagedList(page ?? 1, 5);
            ViewBag.membersCount = members.Count();

            return View();

        }

        [Authorize]
        [Route("memberdetails/id")]
        public ActionResult memberdetails(int? id)
        {


            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                var user = nm.tbl_Users.Where(x => x.Id == id).ToList();
                var up = nm.tbl_UserProfile.Where(x => x.User_id == id).ToList();
                var notes = nm.tbl_Notes.ToList();
                var category = nm.tbl_NoteCategory.ToList();
                var status = nm.tbl_ReferenceData.ToList();
                var userProfileDate = (from usr in user
                                       join updata in up on usr.Id equals updata.User_id
                                       join n in notes on usr.Id equals n.Seller_id
                                       join cat in category on n.NoteCategory_id equals cat.Id
                                       join cn in nm.tbl_Country on n.Country_id equals cn.Id
                                       join ns in nm.tbl_ReferenceData on n.Status equals ns.Id

                                       select new DisplayNotes
                                       {
                                           user = usr,
                                           uprofiledata = updata,
                                           SellerNotes = n,
                                           Category = cat,
                                           country = cn,
                                           status = ns

                                       }).ToList();
                ViewBag.profile = userProfileDate;

            }
            return View();
        }

        [Authorize]
        public ActionResult myprofile()
        {

            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];
                var user = nm.tbl_Users.Where(x => x.Id == id).FirstOrDefault();
                tbl_UserProfile userprofiles = nm.tbl_UserProfile.Where(x => x.User_id == id).FirstOrDefault();
                ViewBag.firstName = user.FirstName;
                ViewBag.lastName = user.LastName;
                ViewBag.email = user.Email_id;

                var countrycode = nm.tbl_Country.ToList();
                ViewBag.countryCode = new SelectList(countrycode, "CountryCode", "CountryCode");


                return View();


            }
        }


        [Authorize]
        [HttpPost]

        public ActionResult myprofile(MyProfileModel upd)
        {

            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];

                var countrycode = nm.tbl_Country.ToList();
                ViewBag.countryCode = new SelectList(countrycode, "CountryCode", "CountryCode");



                tbl_Users u = nm.tbl_Users.Where(x => x.Id == id).FirstOrDefault();
                u.FirstName = upd.FirstName;
                u.LastName = upd.LastName;


                int p = nm.tbl_UserProfile.Where(x => x.User_id == id).Count();
                if (p > 0)
                {

                    tbl_UserProfile profile = nm.tbl_UserProfile.Where(x => x.User_id == id).FirstOrDefault();

                    profile.User_id = (int)Session["Userid"];
                    profile.State = "NULL";
                    profile.PhoneCountryCode = upd.PhoneCountryCode;
                    profile.Country = 1;
                    profile.AddressLine1 = "NULL";
                    profile.DateEdited = DateTime.Now;
                    profile.University = "NULL";
                    profile.ZipCode = "NULL";
                    profile.College = "NULL";
                    profile.City = "NULL";
                    profile.SecondaryEmailAddress = upd.SecondaryEmailAddress;
                    profile.PhoneNumber = upd.PhoneNumber;

                    nm.SaveChanges();
                    string path = Path.Combine(Server.MapPath("~/Member/" + Session["Userid"].ToString()));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (upd.ProfilePicture != null && upd.ProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(upd.ProfilePicture.FileName);
                        string extension = Path.GetExtension(upd.ProfilePicture.FileName);
                        fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(path, fileName);
                        upd.ProfilePicture.SaveAs(finalpath);

                        profile.ProfilePicture = Path.Combine(("~/Member/" + Session["Userid"].ToString()), fileName);
                        nm.SaveChanges();
                    }

                }
                else
                {
                    tbl_UserProfile profile = new tbl_UserProfile();

                    profile.User_id = (int)Session["Userid"];
                    profile.State = null;
                    profile.PhoneCountryCode = upd.PhoneCountryCode;
                    profile.Country = 1;
                    profile.AddressLine1 = "NULL";
                    profile.DateEdited = DateTime.Now;
                    profile.ZipCode = "NULL";
                    profile.City = "NULL";
                    profile.SecondaryEmailAddress = upd.SecondaryEmailAddress;
                    profile.PhoneNumber = upd.PhoneNumber;
                    nm.tbl_UserProfile.Add(profile);
                    //nm.SaveChanges();
                    string path = Path.Combine(Server.MapPath("~/Member/" + Session["Userid"].ToString()));

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (upd.ProfilePicture != null && upd.ProfilePicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(upd.ProfilePicture.FileName);
                        string extension = Path.GetExtension(upd.ProfilePicture.FileName);
                        fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(path, fileName);
                        upd.ProfilePicture.SaveAs(finalpath);

                        profile.ProfilePicture = Path.Combine(("~/Member/" + Session["Userid"].ToString()), fileName);
                        nm.SaveChanges();
                    }


                }


            }




            return View();
        }

        [Authorize]
        //GET: NotesUnderReview
        public ActionResult notesunderreview(string FirstName, string nursearch, int? page)
        {
            using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
            {
                var adminnotesunderreview = (from n in DBobj.tbl_Notes.Where(x => (x.Status == 8 || x.Status == 7) && (x.NoteTitle.StartsWith(nursearch) || nursearch == null)).ToList()
                                             join cat in DBobj.tbl_NoteCategory.ToList() on n.NoteCategory_id equals cat.Id
                                             join usr in DBobj.tbl_Users.ToList() on n.Seller_id equals usr.Id
                                             where (usr.FirstName == FirstName || String.IsNullOrEmpty(FirstName))
                                             join stu in DBobj.tbl_ReferenceData.ToList() on n.Status equals stu.Id
                                             select new DisplayNotes
                                             {
                                                 user = usr,
                                                 SellerNotes = n,
                                                 Category = cat,
                                                 status = stu
                                             }).ToList();

                ViewBag.notesUnderReview = adminnotesunderreview.ToPagedList(page ?? 1, 5);
                ViewBag.notesUnderReviewCount = adminnotesunderreview.Count();

                ViewBag.Sellers = new SelectList(DBobj.tbl_Users.Where(x => x.Role_id == 3).ToList(), "FirstName", "FirstName");

                return View();
            }

        }

        [Authorize]

        [Route("approveNotes/id")]
        //GET: ApproveNote
        public ActionResult approveNotes(int id)
        {
            using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
            {
                tbl_Notes note = DBobj.tbl_Notes.FirstOrDefault(x => x.Id == id);
                if (note != null)
                {
                    note.Status = 2;
                    note.PublishedDate = DateTime.Now;
                    note.StatusActionedBy = (int)Session["UserID"];
                    note.EditedBy = (int)Session["UserID"];
                    note.DateEdited = DateTime.Now;
                    DBobj.SaveChanges();
                }
                return RedirectToAction("notesUnderReview", "Admin");
            }
        }

        [Authorize]
        [Route("rejectNotes/id")]

        //GET: RejectNotes
        public ActionResult rejectNotes(int id, string adminRemarks)
        {
            using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
            {
                tbl_Notes note = DBobj.tbl_Notes.FirstOrDefault(x => x.Id == id);
                if (note != null)
                {
                    note.Status = 3;
                    note.AdminRemarks = adminRemarks;
                    note.PublishedDate = DateTime.Now;
                    note.StatusActionedBy = (int)Session["UserID"];
                    note.EditedBy = (int)Session["UserID"];
                    note.DateEdited = DateTime.Now;
                    DBobj.SaveChanges();
                }
                return RedirectToAction("notesUnderReview", "Admin");
            }
        }

        [Authorize]
        [Route("inReviewNotes/id")]
        //GET: InReviewNotes
        public ActionResult inReviewNotes(int id)
        {
            using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
            {
                tbl_Notes note = DBobj.tbl_Notes.FirstOrDefault(x => x.Id == id);
                if (note != null)
                {
                    note.Status = 5;
                    note.PublishedDate = DateTime.Now;
                    note.StatusActionedBy = (int)Session["UserID"];
                    note.EditedBy = (int)Session["UserID"];
                    note.DateEdited = DateTime.Now;
                    DBobj.SaveChanges();
                }
                return RedirectToAction("notesUnderReview", "Admin");
            }
        }

        [Authorize]
        public ActionResult downloadednotes(string Note, string Seller, string Buyer, string dnsearch, int? page)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                var admindownloadnotes = (from dn in nm.tbl_Download
                                          join n in nm.tbl_Notes.Where(x => x.NoteTitle.StartsWith(dnsearch) || dnsearch == null) on dn.Note_id equals n.Id
                                          where (dn.IsSellerHasAllowedDownload == true && dn.AttachmentPath != null && dn.AttachmentDownloadedDate != null
                                          && (n.NoteTitle == Note || String.IsNullOrEmpty(Note)))
                                          join nc in nm.tbl_NoteCategory on n.NoteCategory_id equals nc.Id
                                          join u in nm.tbl_Users on dn.Seller_id equals u.Id
                                          where (u.FirstName == Seller || String.IsNullOrEmpty(Seller))
                                          join s in nm.tbl_Users on dn.Downloader_id equals s.Id
                                          where (s.FirstName == Buyer || String.IsNullOrEmpty(Buyer))
                                          select new DisplayNotes
                                          {
                                              user = u,
                                              SellerNotes = n,
                                              Category = nc,
                                              buyer = s,
                                              downloadNote = dn

                                          }).ToList();

                ViewBag.downloadNote = admindownloadnotes.ToPagedList(page ?? 1, 5);
                ViewBag.downloadNoteCount = admindownloadnotes.Count();

                ViewBag.dnSellers = new SelectList(nm.tbl_Users.Where(x => x.Role_id == 3).ToList(), "FirstName", "FirstName");
                ViewBag.dnBuyers = new SelectList(nm.tbl_Users.Where(x => x.Role_id == 3).ToList(), "FirstName", "FirstName");
                ViewBag.dnNote = new SelectList(nm.tbl_Notes.ToList(), "NoteTitle", "NoteTitle");
                return View();
            }

        }

        [Authorize]
        //GET: PublishedNotes
        public ActionResult publishednotes(string pnsearch, string FirstName, int? page)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();

            List<DisplayNotes> adminpublishednotes = (from n in nm.tbl_Notes.Where(x => x.Status == 2 && (x.NoteTitle.StartsWith(pnsearch) || pnsearch == null)).ToList()
                                                          join cat in nm.tbl_NoteCategory.ToList() on n.NoteCategory_id equals cat.Id
                                                          join usr in nm.tbl_Users.ToList() on n.Seller_id equals usr.Id
                                                          where usr.FirstName == FirstName || String.IsNullOrEmpty(FirstName)
                                                          join u in nm.tbl_Users.ToList() on n.StatusActionedBy equals u.Id
                                                          select new DisplayNotes
                                                          {
                                                              user = usr,
                                                              SellerNotes = n,
                                                              Category = cat,
                                                              buyer = u
                                                          }).ToList();

            ViewBag.publishedNotes = adminpublishednotes.ToPagedList(page ?? 1, 5);
            ViewBag.publishedNotesCount = adminpublishednotes.Count();

            ViewBag.pnSellers = new SelectList(nm.tbl_Users.Where(x => x.Role_id == 3).ToList(), "FirstName", "FirstName");

            return View();

        }

        [Authorize]
        public ActionResult rejectednotes(string rnsearch, string Seller, int? page)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();

            var adminRejectedNote = (from n in nm.tbl_Notes.Where(x => x.Status == 3 && (x.NoteTitle.StartsWith(rnsearch) || rnsearch == null)).ToList()
                                     join cat in nm.tbl_NoteCategory on n.NoteCategory_id equals cat.Id
                                     join u in nm.tbl_Users on n.Seller_id equals u.Id
                                     where (u.FirstName == Seller || String.IsNullOrEmpty(Seller))
                                     join a in nm.tbl_Users on n.StatusActionedBy equals a.Id
                                     select new DisplayNotes
                                     {
                                         user = u,
                                         SellerNotes = n,
                                         Category = cat,
                                         buyer = a,
                                     }).ToList();

            ViewBag.rejectedNote = adminRejectedNote.ToPagedList(page ?? 1, 5);
            ViewBag.rejectedNoteCount = adminRejectedNote.Count();

            ViewBag.dnSellers = new SelectList(nm.tbl_Users.Where(x => x.Role_id == 3).ToList(), "FirstName", "FirstName");
            return View();
        }

        [Authorize]
        public ActionResult manageadministrator(string adminsearch, int? page)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();

            var admins = (from u in nm.tbl_Users
                          where u.Role_id == 2 && (u.FirstName.StartsWith(adminsearch) || adminsearch == null)
                          join up in nm.tbl_UserProfile on u.Id equals up.User_id
                          select new DisplayNotes
                          {
                              uprofiledata = up,
                              user = u
                          }).ToList();
            ViewBag.administrator = admins.ToPagedList(page ?? 1, 5);
            ViewBag.administratorCount = admins.Count();

            return View();


        }

        [Authorize]
        public ActionResult spamreports(string spamsearch, int? page)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();
            var spamReports = (from sr in nm.tbl_ReportedNotes
                               join u in nm.tbl_Users on sr.ReportedBy_id equals u.Id
                               join n in nm.tbl_Notes on sr.Note_id equals n.Id
                               where (n.NoteTitle.StartsWith(spamsearch) || spamsearch == null)
                               join c in nm.tbl_NoteCategory on n.NoteCategory_id equals c.Id
                               select new DisplayNotes
                               {
                                   Category = c,
                                   user = u,
                                   spam = sr,
                                   SellerNotes = n
                               }).ToList();
            ViewBag.spams = spamReports.ToPagedList(page ?? 1, 5);
            ViewBag.spamsCount = spamReports.Count();

            return View();
        }

        [Authorize]
        public ActionResult managesystemconfiguration()
        {
            return View();
        }

        [Authorize]
        public ActionResult notedetails()
        {
            return View();
        }

        [Authorize]
        [Route("adminDownloadNote/id")]
        //GET: AdminDownloadNote
        public ActionResult adminDownloadNote(int id)
        {
            using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
            {
                tbl_NoteAttachments sellerAttachement = DBobj.tbl_NoteAttachments.Where(x => x.Note_id == id).FirstOrDefault();

                //Return files

                var filesPath = sellerAttachement.FilePath.Split(';');
                var filesName = sellerAttachement.FileName.Split(';');
                using (var ms = new MemoryStream())
                {
                    using (var z = new ZipArchive(ms, ZipArchiveMode.Create, true))
                    {
                        foreach (var FilePath in filesPath)
                        {
                            string FullPath = Path.Combine(Server.MapPath(FilePath));
                            string FileName = Path.GetFileName(FullPath);
                            if (FileName == "adminDownloadNote")
                            {
                                continue;
                            }
                            else
                            {
                                z.CreateEntryFromFile(FullPath, FileName);
                            }
                        }
                    }
                    return File(ms.ToArray(), "application/zip", "Attachement.zip");
                }
            }
        }

        [Authorize]
        [Route("deleteType/id")]
        //GET: DeleteType
        public ActionResult deleteType(int id)
        {
            NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities();
            var typedetail = DBobj.tbl_NoteType.Where(x => x.Id == id).FirstOrDefault();
            typedetail.isActive = false;
            DBobj.SaveChanges();
            return RedirectToAction("managetype", "Admin");
        }

        [Authorize]
        [Route("deleteCategory/id")]
        //GET: DeleteCategory
        public ActionResult deleteCategory(int id)
        {
            NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities();
            var categorydetail = DBobj.tbl_NoteCategory.Where(x => x.Id == id).FirstOrDefault();
            categorydetail.IsActive = false;
            DBobj.SaveChanges();
            return RedirectToAction("managecategory", "Admin");
        }

        [Authorize]
        [Route("deleteCountry/id")]
        //GET: DeleteCountry
        public ActionResult deleteCountry(int id)
        {
            NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities();
            var countrydetail = DBobj.tbl_Country.Where(x => x.Id == id).FirstOrDefault();
            countrydetail.IsActive = false;
            DBobj.SaveChanges();
            return RedirectToAction("managecountry", "Admin");
        }

        [Authorize]
        [Route("deleteAdmin/id")]
        //GET: DeleteAdmin
        public ActionResult deleteAdmin(int id)
        {
            NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities();
            var admindetail = DBobj.tbl_Users.Where(x => x.Id == id).FirstOrDefault();
            admindetail.IsActive = false;
            DBobj.SaveChanges();
            return RedirectToAction("manageadministrator", "Admin");
        }

        //GET: DeactivateUser
        [Authorize]
        public ActionResult deactivateUser(int uid)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                tbl_Users udetail = nm.tbl_Users.Where(x => x.Id == uid).FirstOrDefault();
                IQueryable<tbl_Notes> ndetail = nm.tbl_Notes.Where(x => x.Seller_id == uid);
                foreach (var i in ndetail)
                {
                    i.IsActive = false;
                    tbl_NoteAttachments nddetails = nm.tbl_NoteAttachments.Where(x => x.Note_id == i.Id).FirstOrDefault();
                    nddetails.IsActive = false;
                }
                udetail.IsActive = false;

                nm.SaveChanges();
                return View("members", "Admin");
            }
        }

    }
}