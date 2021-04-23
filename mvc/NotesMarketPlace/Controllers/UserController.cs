using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NotesMarketPlace.Models;
using NotesMarketPlace.EmailTemplates;
using System.Web.Security;
using NotesMarketPlace.Helpers;
using System.IO;
using System.IO.Compression;
using PagedList;
using PagedList.Mvc;

namespace NotesMarketPlace.Controllers
{
    public class UserController : Controller
    {
        //Get Sign-Up
        public ActionResult signup()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signup(Signup s)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                    {
                        tbl_Users u = new tbl_Users();
                        u.FirstName = s.FirstName;
                        u.LastName = s.LastName;
                        u.Email_id = s.Email_id;
                        u.Role_id = 3;
                        u.IsActive = true;
                        u.DateAdded = DateTime.Now;
                        u.DateEdited = DateTime.Now;
                        u.AddedBy = 1;
                        u.EditedBy = 1;
                        u.Password = EncryptPasswords.EncryptPasswordMD5(s.Password);
                        u.IsEmailVerified = false;

                        nm.tbl_Users.Add(u);
                        nm.SaveChanges();

                        if (u.Id > 0)
                        {
                            ModelState.Clear();
                            ViewBag.IsSuccess = "<p><span><i class='fas fa-check-circle'></i></span> Your account has been successfully created </p>";
                            TempData["name"] = s.FirstName;


                            //  Email Verification Link
                            var activationCode = s.Password;
                            var verifyUrl = "/User/VerifyAccount/" + activationCode;
                            var activationlink = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);


                            // Sending Email
                            verifyuser.SendVerifyLinkEmail(u, activationlink);
                            ViewBag.Title = "NotesMarketPlace";


                            return RedirectToAction("emailverification", "User");
                        }
                    }

                }
                return View();
            }
            catch(Exception e)
            {
                @TempData["message"] = e;
                return View();
            }
            
                
            
        }

        [HttpGet]
        public ActionResult VerifyAccount(string id)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                nm.Configuration.ValidateOnSaveEnabled = false; // This line I have added here to avoid 
                                                                // Confirm password does not match issue on save changes
                var p_id = EncryptPasswords.EncryptPasswordMD5(id);
                var em = nm.tbl_Users.Where(x => x.Password == p_id).FirstOrDefault();
                if (em != null)
                {
                    em.IsEmailVerified = true;
                    nm.SaveChanges();
                }
                else
                {
                    ViewBag.Message = "Invalid Request";
                }
            }
            TempData["Message"] = "Your Email Is Verified You Can Login Here";
            return RedirectToAction("Login", "User");

        }

        //Get Login
        public ActionResult login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult login(login obj)
        {
            if (ModelState.IsValid)
            {
                //Encrypt Password and save
                var newPassword = EncryptPasswords.EncryptPasswordMD5(obj.Password);
                NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();
                tbl_Users u = new tbl_Users();

                bool isValid = nm.tbl_Users.Any(x => x.Email_id == obj.Email_id && x.Password == newPassword);

                if (isValid)
                {
                    //tbl_Users userdetails = nm.tbl_Users.Where(x => x.Email_id == obj.Email_id && x.Password == newPassword).FirstOrDefault();
                    if (nm.tbl_Users.Any(x => x.Email_id == obj.Email_id && x.IsEmailVerified == true))
                    {
                        FormsAuthentication.SetAuthCookie(obj.Email_id, false);
                        Session["Emailid"] = obj.Email_id;

                        var user = nm.tbl_Users.Where(x => x.Email_id == obj.Email_id).FirstOrDefault();
                        Session["FullName"] = user.FirstName + " " + user.LastName;
                        Session["Userid"] = user.Id;
                        if (user.Role_id != 3)
                        {
                            return RedirectToAction("dashboard", "Admin");
                        }
                        else
                        {
                            int upCheck = nm.tbl_UserProfile.Where(x => x.User_id == user.Id).Count();
                            if (upCheck > 0)
                            {
                                return RedirectToAction("searchnotes", "User");
                            }
                            else
                            {
                                return RedirectToAction("userProfile", "User");
                            }
                        }

                    }
                    else
                    {
                        @TempData["message"] = "Your Email Address is not Verified, Please Verified it Once...!";
                    }
                }
                else
                {
                    ViewBag.log = "<p> The password that you've entered is incorrect </p>";
                    return View();
                }
            }
            return View();
        }

        //logout (destory session)
        [Authorize]
        public ActionResult Logout()
        {
            Session.Remove("Userid");
            Session.Remove("FullName");
            Session.Remove("Emaiid");
            FormsAuthentication.SignOut();
            return RedirectToAction("login");

        }

        public ActionResult forgot()
        {
            return View();
        }

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

                    u.Password = EncryptPasswords.EncryptPasswordMD5(passwordString);
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

        public ActionResult contactus()
        {
            return View();
        }

        [HttpPost]
        public ActionResult contactus(Contactus c)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                    {
                        tbl_ContactUs u = new tbl_ContactUs();
                        u.FullName = c.FullName;
                        u.Email_id = c.Email_id;
                        u.Subject = c.Subject;
                        u.Question = c.Question;
                        u.IsActive = true;
                        u.DateAdded = DateTime.Now;

                        string subject = u.Subject;
                        string name = u.FullName;
                        string comment = u.Question;

                        nm.tbl_ContactUs.Add(u);
                        nm.SaveChanges();

                        if (u.Id > 0)
                        {

                            ModelState.Clear();
                            contact.ContactUs(subject, name, comment);

                            return View();
                        }
                    }

                }
                return View();
            }
            catch (Exception a)
            {
                @TempData["message"] = a;
                return View();
            }
        }

        // GET: User
        [Authorize]
        public ActionResult dashboard(string submit, string searchnotes, int? page, int? page2)
        {
            using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
            {
                int userid = (int)Session["Userid"];

                ViewBag.mySoldNotes = DBobj.tbl_Download.Where(x => x.Seller_id == userid && x.IsSellerHasAllowedDownload == true).Count();
                var earning = DBobj.tbl_Download.Where(x => x.Seller_id == userid && x.IsSellerHasAllowedDownload == true).Select(x => x.PurchasedPrice).Sum();
                if (earning > 0)
                {
                    ViewBag.totalEarning = earning;
                }
                else
                {
                    ViewBag.totalEarning = 0;
                }
                ViewBag.myDownloadNotes = DBobj.tbl_Download.Where(x => x.Downloader_id == userid && x.IsSellerHasAllowedDownload == true).Count();
                ViewBag.myRejectedNotes = DBobj.tbl_Notes.Where(x => x.Seller_id == userid && x.Status == 3).Count();
                ViewBag.buyerRequests = DBobj.tbl_Download.Where(x => x.Seller_id == userid).Count();

                List<tbl_Notes> SellerNotes = null;
                if (submit == "search1")
                {
                    SellerNotes = DBobj.tbl_Notes.Where(x => x.Seller_id == userid && (x.Status == 4 || x.Status == 1) &&
                                                        (x.NoteTitle.StartsWith(searchnotes) || searchnotes == null)).ToList();
                }
                else
                {
                    SellerNotes = DBobj.tbl_Notes.Where(x => x.Seller_id == userid && (x.Status == 4 || x.Status == 1)).ToList();
                }

                var ProgressNotes = (from sell in SellerNotes
                                     join cate in DBobj.tbl_NoteCategory.ToList() on sell.Id equals cate.Id
                                     orderby sell.DateAdded descending
                                     join stu in DBobj.tbl_ReferenceData.ToList() on sell.Status equals stu.Id
                                     select new DisplayNotes
                                     {
                                         SellerNotes = sell,
                                         Category = cate,
                                         status = stu
                                     }).ToList();


                ViewBag.inProgressNotes = ProgressNotes.ToPagedList(page ?? 1,5);
                ViewBag.inProgressNotesCount = ProgressNotes.Count();

                List<tbl_Notes> PublishedNote = null;
                if (submit == "search2")
                {
                    PublishedNote = DBobj.tbl_Notes.Where(x => x.Seller_id == userid && x.Status == 2 &&
                                                           (x.NoteTitle.StartsWith(searchnotes) || searchnotes == null)).ToList();
                }
                else
                {
                    PublishedNote = DBobj.tbl_Notes.Where(x => x.Seller_id == userid && x.Status == 2).ToList();
                }





                var PublishedNoted = (from sell in PublishedNote
                                      join cate in DBobj.tbl_NoteCategory.ToList() on sell.Id equals cate.Id
                                      orderby sell.PublishedDate descending
                                      select new DisplayNotes
                                      {
                                          SellerNotes = sell,
                                          Category = cate,

                                      }).ToList();

                ViewBag.publishedNote = PublishedNoted.ToPagedList(page2 ?? 1, 5);
                ViewBag.publishedNoteCount = PublishedNoted.Count();

                if (Session["Userid"] != null)
                {
                    int usid = (int)Session["Userid"];
                    var dp = DBobj.tbl_UserProfile.Where(x => x.Id == usid).Select(x => x.ProfilePicture).FirstOrDefault();
                    if (dp != null)
                    {
                        Session["profile"] = dp;
                    }
                    else
                    {
                        Session["profile"] = "/Content/User/images/testimonial/customer-1.png";
                    }

                }

                return View();
            }

        }

        [Authorize]
        [HttpGet]
        public ActionResult addnote()
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                ViewBag.notecategoies = new SelectList(nm.tbl_NoteCategory.ToList(), "Id", "CategoryName");
                ViewBag.notetypes = new SelectList(nm.tbl_NoteType.ToList(), "Id", "TypeName");
                ViewBag.countries = new SelectList(nm.tbl_Country.ToList(), "Id", "Name");
            }
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult addnote(NotesModel notedetails, string submit)
        {
           

                if (notedetails.IsPaid == true && notedetails.NotesPreview == null)
                {
                    using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                    {
                        ViewBag.previewmessage = "Note Preview Is Required For Paid Note...";
                        ViewBag.notecategoies = new SelectList(nm.tbl_NoteCategory.ToList(), "Id", "CategoryName");
                        ViewBag.notetypes = new SelectList(nm.tbl_NoteType.ToList(), "Id", "TypeName");
                        ViewBag.countries = new SelectList(nm.tbl_Country.ToList(), "Id", "Name");
                        return View();
                    }
                }


                using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                {
                    tbl_Notes nd = new tbl_Notes();
                    tbl_NoteAttachments sna = new tbl_NoteAttachments();
                    nd.Seller_id = (int)Session["Userid"];
                    if (submit == "Save")
                    {
                        nd.Status = 1;
                    }
                    if (submit == "Publish")
                    {
                        nd.Status = 8;
                        nd.DateEdited = DateTime.Now;
                    }
                    nd.NoteTitle = notedetails.NoteTitle;
                    nd.NoteCategory_id = notedetails.NoteCategory_id;
                    nd.NoteType_id = notedetails.NoteType_id;
                    nd.NumberOfPages = notedetails.NumberOfPages;
                    nd.Description = notedetails.Description;
                    nd.University = notedetails.University;
                    nd.Country_id = notedetails.Country_id;
                    nd.Course = notedetails.Course;
                    nd.CourseCode = notedetails.CourseCode;
                    nd.Professor = notedetails.Professor;
                    nd.IsPaid = notedetails.IsPaid;
                    nd.SellingPrice = notedetails.SellingPrice;
                    nd.IsActive = true;
                    nd.DateAdded = DateTime.Now;
                    nd.DateEdited = DateTime.Now;
                    nd.AddedBy = (int)Session["Userid"];
                    nd.EditedBy = (int)Session["Userid"];

                    nm.tbl_Notes.Add(nd);
                    nm.SaveChanges();

                    string path = Path.Combine(Server.MapPath("~/Member/" + Session["Userid"].ToString()), nd.Id.ToString());

                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    if (notedetails.DisplayPicture != null && notedetails.DisplayPicture.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(notedetails.DisplayPicture.FileName);
                        string extension = Path.GetExtension(notedetails.DisplayPicture.FileName);
                        fileName = "DP_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(path, fileName);
                        notedetails.DisplayPicture.SaveAs(finalpath);

                        nd.DisplayPicture = Path.Combine(("~/Member/" + Session["Userid"].ToString() + "/" + nd.Id.ToString() + "/"), fileName);
                        nm.SaveChanges();
                    }
                    if (notedetails.NotesPreview != null && notedetails.NotesPreview.ContentLength > 0)
                    {
                        string fileName = Path.GetFileNameWithoutExtension(notedetails.NotesPreview.FileName);
                        string extension = Path.GetExtension(notedetails.NotesPreview.FileName);
                        fileName = "PREVIEW_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                        string finalpath = Path.Combine(path, fileName);
                        notedetails.NotesPreview.SaveAs(finalpath);

                        nd.NotesPreview = Path.Combine(("~/Member/" + Session["Userid"].ToString() + "/" + nd.Id.ToString() + "/"), fileName);
                        nm.SaveChanges();
                    }

                    string attachmentpath = Path.Combine(Server.MapPath("~/Member/" + Session["Userid"].ToString() + "/" + nd.Id.ToString()), "attachment");

                    if (!Directory.Exists(attachmentpath))
                    {
                        Directory.CreateDirectory(attachmentpath);
                    }
                    if (notedetails.NoteAttachment != null && notedetails.NoteAttachment[0].ContentLength > 0)
                    {
                        var count = 1;
                        var FilePath = "";
                        var FileName = "";
                        foreach (var file in notedetails.NoteAttachment)
                        {
                            string fileName = Path.GetFileNameWithoutExtension(file.FileName);
                            string extension = Path.GetExtension(file.FileName);
                            fileName = "Attachment_" + count + "_" + DateTime.Now.ToString("ddMMyyyy") + extension;
                            string finalpath = Path.Combine(attachmentpath, fileName);
                            file.SaveAs(finalpath);
                            FileName += fileName + ";";
                            FilePath += Path.Combine(("/Member/" + Session["Userid"].ToString() + "/" + nd.Id.ToString() + "/attachment/"), fileName) + ";";
                            count++;
                        }
                        sna.FileName = FileName;
                        sna.FilePath = FilePath;

                        sna.Note_id = nd.Id;
                        sna.IsActive = true;
                        sna.DateAdded = DateTime.Now;
                        sna.AddedBy = (int)Session["Userid"];
                    nm.tbl_NoteAttachments.Add(sna);
                        nm.SaveChanges();

                    }
                    nm.tbl_NoteAttachments.Add(sna);
                    nm.SaveChanges();

                    //Sending mail to admin
                    if (submit == "Publish")
                    {
                        Noteverify.VerifyByAdmin(Session["FullName"].ToString(), nd.NoteTitle);
                    }

                }
                ModelState.Clear();
                return RedirectToAction("dashboard", "User");
            

           // using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            //{
               // ViewBag.notecategoies = new SelectList(nm.tbl_NoteCategory.ToList(), "Id", "CategoryName");
               // ViewBag.notetypes = new SelectList(nm.tbl_NoteType.ToList(), "Id", "TypeName");
                //ViewBag.countries = new SelectList(nm.tbl_Country.ToList(), "Id", "Name");
               // return View();
            //}

        }

        //Get 
        public ActionResult searchnotes(int? page, string search, string NoteType_id, string NoteCategory_id, string University, string Country_id, string Course, decimal? rating)
        {
            NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();
            

                var notecategory = nm.tbl_NoteCategory.Distinct().ToList();
                var notetype = nm.tbl_NoteType.Distinct().ToList();
                var country = nm.tbl_Country.Distinct().ToList();
                var university_course = nm.tbl_Notes.Distinct().ToList();
                ViewBag.notecategoies = new SelectList(notecategory.Distinct(), "Id", "CategoryName");
                ViewBag.notetypes = new SelectList(notetype.Distinct(), "Id", "TypeName");
                ViewBag.countries = new SelectList(country.Distinct(), "Id", "Name");
                ViewBag.universities = new SelectList(university_course.Distinct(), "University", "University");
                ViewBag.courses = new SelectList(university_course.Distinct(), "Course", "Course");






                var notes = nm.tbl_Notes.Where(x => x.NoteTitle.StartsWith(search) || search == null).ToList();
                var cr = nm.tbl_Country.ToList();

                var seachedNotes = (from n in notes
                                    join c in cr on n.Country_id equals c.Id into table1
                                    from c in table1.ToList()
                                    where (n.Status == 2 && (n.NoteType_id.ToString() == NoteType_id || String.IsNullOrEmpty(NoteType_id))
                                          && (n.NoteCategory_id.ToString() == NoteCategory_id || String.IsNullOrEmpty(NoteCategory_id))
                                          && (n.University.ToString() == University || String.IsNullOrEmpty(University))
                                          && (n.Country_id.ToString() == Country_id || String.IsNullOrEmpty(Country_id))
                                          && (n.Course.ToString() == Course || String.IsNullOrEmpty(Course)))
                                    select new DisplayNotes
                                    {
                                        SellerNotes = n,
                                        country = c
                                    }).ToList();

                ViewBag.filterNotes = seachedNotes.ToPagedList(page ?? 1, 6);

                ViewBag.dn = seachedNotes.Count();

                if (Session["Userid"] != null)
                {
                    int usid = (int)Session["Userid"];
                    var dp = nm.tbl_UserProfile.Where(x => x.Id == usid).Select(x => x.ProfilePicture).FirstOrDefault();
                    if (dp != null)
                    {
                        Session["profile"] = dp;
                    }
                    else
                    {
                        Session["profile"] = "/Content/User/images/testimonial/customer-1.png";
                    }

                }



                return View();
            

        }

        //GET: Notedetails
        [Authorize]
        [Route("notedetail /{id}")]
        public ActionResult notedetail(int? id)
        {

            using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
            {

                var ni = DBobj.tbl_Notes.Where(x => x.Id == id).FirstOrDefault();
                tbl_NoteCategory noteCategory = DBobj.tbl_NoteCategory.Find(ni.NoteCategory_id);
                ViewBag.Category = noteCategory.CategoryName;
                tbl_Country country = DBobj.tbl_Country.Find(ni.Country_id);
                ViewBag.Country = country.Name;

                var reviewdetail = (from nr in DBobj.tbl_NoteReviews
                                    join n in DBobj.tbl_Notes on nr.Note_id equals n.Id
                                    join us in DBobj.tbl_Users on nr.ReviewedBy_id equals us.Id
                                    join up in DBobj.tbl_UserProfile on nr.ReviewedBy_id equals up.Id
                                    orderby nr.DateAdded descending
                                    select new DisplayNotes
                                    {
                                        SellerNotes = n,
                                        notereview = nr,
                                        user = us,
                                        uprofiledata = up
                                    }).Take(3).ToList();

                ViewBag.reviewdetailbag = reviewdetail;
                ViewBag.reviewcount = reviewdetail.Count();
                ViewBag.ratingCount = DBobj.tbl_NoteReviews.Where(x => x.Note_id == id).Select(x => x.Ratings).Count();
                if (ViewBag.ratingcount > 0)
                {
                    ViewBag.ratingSum = DBobj.tbl_NoteReviews.Where(x => x.Note_id == id).Select(x => x.Ratings).Sum();
                }
                else
                {
                    ViewBag.ratingSum = "No Review Found !";
                }



                ViewBag.spamtotalcount = DBobj.tbl_ReportedNotes.Where(x => x.Note_id == id).Count();

                return View(ni);
            }
        }

        [Authorize]
        public ActionResult buyersrequests(int? page)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];

                var by = (from n in nm.tbl_Notes
                          join dn in nm.tbl_Download on n.Id equals dn.Note_id
                          where dn.Seller_id == id
                          orderby dn.AttachmentDownloadedDate descending
                          join cat in nm.tbl_NoteCategory on n.NoteCategory_id equals cat.Id
                          join u in nm.tbl_Users on dn.Downloader_id equals u.Id
                          join up in nm.tbl_UserProfile on dn.Downloader_id equals up.User_id
                          join cc in nm.tbl_Country on up.Country equals cc.Id
                          select new DisplayNotes
                          {
                              downloadNote = dn,
                              user = u,
                              SellerNotes = n,
                              Category = cat,
                              uprofiledata = up,
                              country = cc

                          }).ToList().ToPagedList(page ?? 1, 5);
                ViewBag.BuyerRequests = by;
                ViewBag.BuyerRequestsCount = by.Count();

            }

            return View();
        }


        [Route("downloadflow /{ id }")]
        public ActionResult downloadflow(int? id)
        {
            if (Session["Userid"] != null)
            {

                using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                {
                    int sellerId = 0;



                    //Free Note
                    int isFree = nm.tbl_Notes.Where(x => x.Id == id && x.IsPaid == false).Count();
                    if (isFree > 0)
                    {
                        tbl_Download dn = new tbl_Download();
                        var notetitle = nm.tbl_Notes.Where(x => x.Id == id);
                        var sellerAttachement = nm.tbl_NoteAttachments.Where(x => x.Note_id == id).FirstOrDefault();

                        dn.Downloader_id = (int)Session["Userid"];
                        dn.IsSellerHasAllowedDownload = false;
                        dn.IsPaid = true;
                        dn.IsAttachmentDownloaded = false;
                        dn.DateAdded = DateTime.Now;
                        dn.AddedBy = (int)Session["Userid"];
                        
                        foreach (var iv in notetitle)
                        {
                            dn.Note_id = iv.Id;
                            dn.Seller_id = iv.Seller_id;
                            dn.PurchasedPrice = iv.SellingPrice;
                            dn.NoteTitle = iv.NoteTitle;
                            dn.NoteCategory = (iv.NoteCategory_id).ToString();
                            sellerId = iv.Seller_id;
                        }

                        nm.tbl_Download.Add(dn);
                        nm.SaveChanges();

                        //return files

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
                                    if (FileName == "User")
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





                    //Paid Note 

                    var i = nm.tbl_Notes.Where(x => x.Id == id && x.IsPaid == true).Count();
                    if (i > 0)
                    {
                        tbl_Download dn = new tbl_Download();
                        var notetitle = nm.tbl_Notes.Where(x => x.Id == id);

                        dn.Downloader_id = (int)Session["Userid"];
                        dn.IsSellerHasAllowedDownload = false;
                        dn.IsPaid = true;
                        dn.IsAttachmentDownloaded = false;
                        dn.DateAdded = DateTime.Now;
                        dn.AddedBy = (int)Session["Userid"];
                        
                        foreach (var iv in notetitle)
                        {
                            dn.Note_id = iv.Id;
                            dn.Seller_id = iv.Seller_id;
                            dn.PurchasedPrice = iv.SellingPrice;
                            dn.NoteTitle = iv.NoteTitle;
                            dn.NoteCategory = (iv.NoteCategory_id).ToString();
                            sellerId = iv.Seller_id;
                        }

                        nm.tbl_Download.Add(dn);
                        nm.SaveChanges();

                        var sellerInfo = nm.tbl_Users.Where(x => x.Id == sellerId).ToList();
                        foreach (var s in sellerInfo)
                        {
                            ViewBag.sellerName = s.FirstName + " " + s.LastName;
                            ViewBag.sellerEmailId = s.Email_id;
                        }



                        string buyerName = (string)Session["FullName"];



                        //Send mail to owner
                        Notifyseller.SellerPublishNote(ViewBag.sellerName, ViewBag.sellerEmailId, buyerName);
                    }
                }
                return RedirectToAction("notedetail", "User", new { id = id });
            }
            else
            {
                return RedirectToAction("login", "User");
            }

        }

        public ActionResult myrejectednotes(int? page, string rejectsearch)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];
                var rejected = (from n in nm.tbl_Notes
                                join cat in nm.tbl_NoteCategory on n.NoteCategory_id equals cat.Id
                                where n.Status == 3 && n.Seller_id == id
                                select new DisplayNotes
                                {
                                    SellerNotes = n,
                                    Category = cat
                                }).ToList();
                ViewBag.rejectedNote = rejected.ToPagedList(page ?? 1, 5);
            }
            return View();
        }

        //get
        [Authorize]
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


        public ActionResult index()
        {
            return View();
        }

        public ActionResult emailverification()
        {
            return View();
        }

        //get
        [Authorize]
        public ActionResult userprofile()
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {

                int id = (int)Session["Userid"];
                var user = nm.tbl_Users.Where(x => x.Id == id).FirstOrDefault();
                tbl_UserProfile userprofiles = nm.tbl_UserProfile.Where(x => x.User_id == id).FirstOrDefault();
                MyProfileModel upd = new MyProfileModel();

                var countryname = nm.tbl_Country.ToList();
                ViewBag.countries = new SelectList(countryname, "Id", "Name");
                var countrycode = nm.tbl_Country.ToList();
                ViewBag.countryCode = new SelectList(countrycode, "Id", "CountryCode");

                if (user != null)
                {
                    ViewBag.firstName = user.FirstName;
                    ViewBag.lastName = user.LastName;
                    ViewBag.email = user.Email_id;
                    if (userprofiles != null)
                    {
                        upd.DOB = userprofiles.DOB;
                        upd.AddressLine1 = userprofiles.AddressLine1;
                        upd.AddressLine2 = userprofiles.AddressLine2;
                        upd.City = userprofiles.City;
                        upd.State = userprofiles.State;
                        upd.ZipCode = userprofiles.ZipCode;
                        upd.University = userprofiles.University;
                        upd.College = userprofiles.College;
                        upd.PhoneNumber = userprofiles.PhoneNumber;


                    }
                    if (userprofiles != null)
                    {
                        Session["profile"] = userprofiles.ProfilePicture;
                    }
                    else
                    {
                        Session["profile"] = "/Content/User/images/testimonial/customer-1.png";
                    }
                    
                    return View(upd);
                }
                if (nm.tbl_UserProfile.Where(x => x.Id == id).Count() > 0)
                {
                    Session["profile"] = userprofiles.ProfilePicture;
                }
                else
                {
                    Session["profile"] = "/Content/User/images/testimonial/customer-1.png";
                }
                
                return View();
            }


        }

        [HttpPost]
        public ActionResult userprofile(MyProfileModel upd)
        {
            

                    using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                    {
                        int id = (int)Session["Userid"];
                        var countryname = nm.tbl_Country.ToList();
                        ViewBag.countries = new SelectList(countryname, "Id", "Name");
                        var countrycode = nm.tbl_Country.ToList();
                        ViewBag.countryCode = new SelectList(countrycode, "Id", "CountryCode");
                        tbl_Users u = nm.tbl_Users.Where(x => x.Id == id).FirstOrDefault();
                        u.FirstName = upd.FirstName;
                        u.LastName = upd.LastName;


                        int p = nm.tbl_UserProfile.Where(x => x.User_id == id).Count();
                        if (p > 0)
                        {

                            tbl_UserProfile profile = nm.tbl_UserProfile.Where(x => x.User_id == id).FirstOrDefault();

                            profile.User_id = (int)Session["Userid"];
                            profile.State = upd.State;
                            profile.Country = upd.Country;
                            profile.PhoneCountryCode = upd.PhoneCountryCode;
                            profile.AddressLine1 = upd.AddressLine1;
                            profile.AddressLine2 = upd.AddressLine2;
                            profile.DOB = upd.DOB;
                            profile.Gender = upd.Gender;
                            
                            profile.University = upd.University;
                            profile.ZipCode = upd.ZipCode;
                            profile.College = upd.College;
                            profile.City = upd.City;
                            profile.SecondaryEmailAddress = upd.SecondaryEmailAddress;
                            profile.PhoneNumber = upd.PhoneNumber;
                            profile.DateAdded = DateTime.Now;
                            profile.AddedBy = (int)Session["Userid"];
                            profile.DateEdited = DateTime.Now;
                            profile.EditedBy = (int)Session["Userid"];


                        nm.SaveChanges();
                            string path = Path.Combine(Server.MapPath("/Member/" + Session["Userid"].ToString()));

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

                                profile.ProfilePicture = Path.Combine(("/Member/" + Session["Userid"].ToString()), fileName);
                                nm.SaveChanges();
                            }

                        }
                        else
                        {
                            tbl_UserProfile profile = new tbl_UserProfile();

                            profile.User_id = (int)Session["Userid"];
                            profile.State = upd.State;
                            profile.Country = upd.Country;
                            profile.PhoneCountryCode = upd.PhoneCountryCode;
;
                            profile.AddressLine1 = upd.AddressLine1;
                            profile.AddressLine2 = upd.AddressLine2;
                            profile.DOB = upd.DOB;
                            profile.Gender = upd.Gender;
                            profile.University = upd.University;
                            profile.ZipCode = upd.ZipCode;
                            profile.College = upd.College;
                            profile.City = upd.City;
                            profile.SecondaryEmailAddress = upd.SecondaryEmailAddress;
                            profile.PhoneNumber = upd.PhoneNumber;
                            profile.DateAdded = DateTime.Now;
                            profile.AddedBy = (int)Session["Userid"];
                            profile.DateEdited = DateTime.Now;
                            profile.EditedBy = (int)Session["Userid"];
                            nm.tbl_UserProfile.Add(profile);
                            nm.SaveChanges();
                            string path = Path.Combine(Server.MapPath("/Member/" + Session["Userid"].ToString()));

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

                                profile.ProfilePicture = Path.Combine(("/Member/" + Session["Userid"].ToString()), fileName);
                                nm.SaveChanges();
                            }


                        }

                    }
            


            return RedirectToAction("dashboard", "User");
        }

        public ActionResult faq()
        {
            return View();
        }

        [Authorize]
        public ActionResult mydownloads(int? page, string downloadSearch)
        {
            try
            {
                using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                {
                    nm.Configuration.LazyLoadingEnabled = false;
                    int id = (int)Session["Userid"];


                    var downloadNotes = (from dn in nm.tbl_Download
                                         join n in nm.tbl_Notes.Where(x => x.NoteTitle.StartsWith(downloadSearch) || downloadSearch == null) on dn.Note_id equals n.Id
                                         where dn.Downloader_id == id && dn.IsSellerHasAllowedDownload == true && dn.IsActive == true
                                         join nc in nm.tbl_NoteCategory on n.NoteCategory_id equals nc.Id
                                         join u in nm.tbl_Users on dn.Seller_id equals u.Id

                                         select new DisplayNotes
                                         {
                                             user = u,
                                             SellerNotes = n,
                                             downloadNote = dn,
                                             Category = nc
                                         }).ToList();
                    ViewBag.DownloadNotes = downloadNotes.ToPagedList(page ?? 1, 5);
                    ViewBag.DownloadNotesCount = downloadNotes.Count();

                }
            }
            catch (Exception e)
            {
                return View(e);
            }

            return View();

        }

        public ActionResult mysoldnotes(int? page, string soldsearch)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];
                var soldnotes = (from n in nm.tbl_Notes
                                 join dn in nm.tbl_Download on n.Id equals dn.Note_id
                                 where dn.IsSellerHasAllowedDownload == true && dn.Seller_id == id
                                 join cat in nm.tbl_NoteCategory on n.NoteCategory_id equals cat.Id
                                 join u in nm.tbl_Users on dn.Downloader_id equals u.Id
                                 select new DisplayNotes
                                 {
                                     SellerNotes = n,
                                     downloadNote = dn,
                                     Category = cat,
                                     user = u
                                 }).ToList();

                ViewBag.mysoldnotes = soldnotes.ToPagedList(page ?? 1, 5); ;
            }
            return View();
        }

        [Authorize]
        [Route("spamReport/id")]
        public ActionResult spamReport(string remark, int noteID1)
        {


            if (remark != null)
            {
                using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
                {
                    int user = (int)Session["Userid"];
                    var n = DBobj.tbl_Notes.Where(x => x.Id == noteID1).FirstOrDefault();
                    var dn = DBobj.tbl_Download.Where(x => x.Note_id == noteID1 && x.Downloader_id == user).FirstOrDefault();
                    var s = DBobj.tbl_Users.Where(x => x.Id == n.Seller_id).FirstOrDefault();
                    int count = DBobj.tbl_ReportedNotes.Where(x => x.Note_id == noteID1 && x.ReportedBy_id == user).Count();
                    if (count > 0)
                    {
                        ViewBag.spammsg = "You have already reported for this note";
                    }
                    else
                    {
                        tbl_ReportedNotes sr = new tbl_ReportedNotes();
                        sr.Note_id = noteID1;
                        sr.ReportedBy_id = user;
                        sr.Remarks = remark;
                        sr.IsDownloaded_id = dn.Id;
                        sr.AddedBy = user;
                        sr.EditedBy = user;
                        sr.DateEdited = DateTime.Now;
                        sr.DateAdded = DateTime.Now;

                        DBobj.tbl_ReportedNotes.Add(sr);
                        DBobj.SaveChanges();

                        ReportedNoteMail.spamReport(s.FirstName, Session["FullName"].ToString(), n.NoteTitle);
                        return RedirectToAction("mydownloads", "User");
                    }
                    return RedirectToAction("myDownloads", "User");
                }
            }
            return RedirectToAction("mydownloads", "User");
        }

        [Authorize]
        [Route("adminDownloadNote/id")]
        //GET: AdminDownloadNote
        public ActionResult userDownloadNote(int id)
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
                            if (FileName == "userDownloadNote")
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
        [Route("acceptDownloadRequest /{id}")]
        public ActionResult acceptDownloadRequest(int? id)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {

                var accepted = nm.tbl_Download.Where(x => x.Note_id == id && x.IsSellerHasAllowedDownload == false).FirstOrDefault();
                accepted.IsSellerHasAllowedDownload = true;
                accepted.EditedBy = (int)Session["Userid"];
                accepted.DateEdited = DateTime.Now;
                var path = nm.tbl_NoteAttachments.Where(x => x.Note_id == id && x.FilePath != null).FirstOrDefault();
                accepted.AttachmentPath = path.FilePath;
                nm.SaveChanges();

                int buyerId = accepted.Downloader_id;
                var buyerInfo = nm.tbl_Users.Where(x => x.Id == buyerId);
                foreach (var i in buyerInfo)
                {
                    ViewBag.buyerName = i.FirstName + " " + i.LastName;
                    ViewBag.emailId = i.Email_id;
                }

                ViewBag.sellerName = Session["FullName"];
                InformBuyer.mailToBuyer(ViewBag.buyerName, ViewBag.emailId, ViewBag.sellerName);
            }
            return RedirectToAction("buyersrequests", "User");
        }

        [Authorize]
        [Route("noteReview/id")]
        //GET: NoteReview
        public ActionResult noteReview(tbl_NoteReviews model, int noteID)
        {
            if (ModelState.IsValid)
            {
                using (NoteMarketPlaceEntities DBobj = new NoteMarketPlaceEntities())
                {
                    int id = (int)Session["Userid"];

                    tbl_NoteReviews nr = new tbl_NoteReviews();
                    var downloaddata = DBobj.tbl_Download.Where(x => x.Note_id == noteID && x.Downloader_id == id).FirstOrDefault();
                    nr.IsDownloaded_id = downloaddata.Id;
                    nr.ReviewedBy_id = id;
                    nr.Ratings = model.Ratings;
                    nr.Reviews = model.Reviews;
                    nr.Note_id = noteID;
                    nr.IsActive = true;
                    nr.DateAdded = DateTime.Now;
                    nr.AddedBy = id;
                    nr.EditedBy = id;
                    nr.DateEdited = DateTime.Now;

                    DBobj.tbl_NoteReviews.Add(nr);
                    DBobj.SaveChanges();

                }
            }
            return RedirectToAction("mydownloads", "User");
        }
    }
}