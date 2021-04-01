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

namespace NotesMarketPlace.Controllers
{
    public class UserController : Controller
    {
        
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
                       
                        var user = nm.tbl_Users.Where(x => x.Email_id == obj.Email_id).FirstOrDefault();
                        Session["FullName"] = user.FirstName + " " + user.LastName;
                        Session["Userid"] = user.Id;
                        if (user.Role_id == 3)
                        {
                            return RedirectToAction("dashboard", "User");
                        }
                        else
                        {
                            return RedirectToAction("dashboard", "Admin");
                        }
                        
                    }
                    @TempData["Error"] = "Your Email Address is not Verified, Please verify it Once...!";
                    return View();
                }
                @TempData["Error"] = "Invalid username or password!";
                return View();
            }
            return View();
        }

        public ActionResult Logout()
        {
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
        public ActionResult dashboard(string submit, string searchnotes)
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
                                     select new Allnotes
                                     {
                                         SellerNotes = sell,
                                         Category = cate,
                                         status = stu
                                     }).ToList();


                ViewBag.inProgressNotes = ProgressNotes;
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
                                      select new Allnotes
                                      {
                                          SellerNotes = sell,
                                          Category = cate,

                                      }).ToList();

                ViewBag.publishedNote = PublishedNoted;
                ViewBag.publishedNoteCount = PublishedNoted.Count();



                return View();
            }

        }
        
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


        [HttpPost]
        public ActionResult addnote(Addnote notedetails, string submit)
        {
            if (ModelState.IsValid)
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
                        nd.Status = 4;
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
            }
            
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                ViewBag.notecategoies = new SelectList(nm.tbl_NoteCategory.ToList(), "Id", "CategoryName");
                ViewBag.notetypes = new SelectList(nm.tbl_NoteType.ToList(), "Id", "TypeName");
                ViewBag.countries = new SelectList(nm.tbl_Country.ToList(), "Id", "Name");
                return View();
            }

        }



        //Get 
        public ActionResult searchnotes(string search, string NoteType_id, string NoteCategory_id, string University, string Country_id, string Course)
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {

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
                                    join c in cr on n.Country_id equals c.Id 
                                    
                                    where ((n.NoteType_id.ToString() == NoteType_id || String.IsNullOrEmpty(NoteType_id))
          && (n.NoteCategory_id.ToString() == NoteCategory_id || String.IsNullOrEmpty(NoteCategory_id))
          && (n.University.ToString() == University || String.IsNullOrEmpty(University))
          && (n.Country_id.ToString() == Country_id || String.IsNullOrEmpty(Country_id))
          && (n.Course.ToString() == Course || String.IsNullOrEmpty(Course)))
                                    select new Allnotes
                                    {
                                        SellerNotes = n,
                                        country = c
                                    }).ToList();

                ViewBag.filterNotes = seachedNotes;

                ViewBag.nd = seachedNotes.Count();



                return View();
            }

        }


        //GET: Notedetails
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
                                    select new Allnotes
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



        public ActionResult buyersrequests()
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
                          select new Allnotes
                          {
                              downloadNote = dn,
                              user = u,
                              SellerNotes = n,
                              Category = cat,
                              uprofiledata = up,
                              country = cc

                          }).ToList();
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
                    int isFree = nm.tbl_Notes.Where(x => x.Id == id && !x.IsPaid).Count();
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

                    var i = nm.tbl_Notes.Where(x => x.Id == id && x.IsPaid).Count();
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


        public ActionResult myrejectednotes()
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];
                var rejected = (from n in nm.tbl_Notes
                                join cat in nm.tbl_NoteCategory on n.NoteCategory_id equals cat.Id
                                where n.Status == 3 && n.Seller_id == id
                                select new Allnotes
                                {
                                    SellerNotes = n,
                                    Category = cat
                                }).ToList();
                ViewBag.rejectedNote = rejected;
            }
            return View();
        }
        //get
        public ActionResult changepassword()
        {
            return View();
        }
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

        [HttpPost]
        public ActionResult index(string s)
        {
            return View();
        }

        public ActionResult emailverification()
        {
            return View();
        }

        //get
        public ActionResult userprofile()
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];
                var user = nm.tbl_Users.Where(x => x.Id == id).FirstOrDefault();
                tbl_UserProfile userprofiles = nm.tbl_UserProfile.Where(x => x.User_id == id).FirstOrDefault();
                Userprofile upd = new Userprofile();
                ViewBag.firstName = user.FirstName;
                ViewBag.lastName = user.LastName;
                ViewBag.email = user.Email_id;

                var countryname = nm.tbl_Country.ToList();
                ViewBag.countries = new SelectList(countryname, "Country_id", "Name");
                var countrycode = nm.tbl_Country.ToList();
                ViewBag.countryCode = new SelectList(countrycode, "Country_id", "CountryCode");

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


                    return View(upd);
                }

                return View();
            }


        }

        [HttpPost]
        public ActionResult userprofile(Userprofile upd)
        {

            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];
                var countryname = nm.tbl_Country.ToList();
                ViewBag.countries = new SelectList(countryname, "CountryID", "CountryName");
                var countrycode = nm.tbl_Country.ToList();
                ViewBag.countryCode = new SelectList(countrycode, "CountryID", "CountryCode");
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
                    profile.PhoneCountryCode = upd.CountryCode;
                    profile.AddressLine1 = upd.AddressLine1;
                    profile.AddressLine2 = upd.AddressLine2;
                    profile.DOB = upd.DOB;
                    profile.Gender = upd.Gender;
                    profile.DateEdited = DateTime.Now;
                    profile.University = upd.University;
                    profile.ZipCode = upd.ZipCode;
                    profile.College = upd.College;
                    profile.City = upd.City;
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
                    profile.State = upd.State;
                    profile.Country = upd.Country;
                    profile.PhoneCountryCode = upd.CountryCode;
                    profile.AddressLine1 = upd.AddressLine1;
                    profile.AddressLine2 = upd.AddressLine2;
                    profile.DOB = upd.DOB;
                    profile.Gender = upd.Gender;
                    profile.DateEdited = DateTime.Now;
                    profile.University = upd.University;
                    profile.ZipCode = upd.ZipCode;
                    profile.College = upd.College;
                    profile.City = upd.City;
                    profile.SecondaryEmailAddress = upd.SecondaryEmailAddress;
                    profile.PhoneNumber = upd.PhoneNumber;
                    nm.tbl_UserProfile.Add(profile);
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

            }


            return RedirectToAction("dashboard", "User");
        }



        public ActionResult faq()
        {
            return View();
        }

        [HttpPost]
        public ActionResult faq(string s)
        {
            return View();
        }


        public ActionResult mydownloads()
        {
            return View();
        }

        [HttpPost]
        public ActionResult mydownloads(string s)
        {
            return View();
        }



        public ActionResult mysoldnotes()
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                int id = (int)Session["Userid"];
                var soldnotes = (from n in nm.tbl_Notes
                                 join dn in nm.tbl_Download on n.Id equals dn.Note_id
                                 where dn.IsSellerHasAllowedDownload == true && dn.Seller_id == id
                                 join cat in nm.tbl_NoteCategory on n.NoteCategory_id equals cat.Id
                                 join u in nm.tbl_Users on dn.Downloader_id equals u.Id
                                 select new Allnotes
                                 {
                                     SellerNotes = n,
                                     downloadNote = dn,
                                     Category = cat,
                                     user = u
                                 }).ToList();

                ViewBag.mysoldnotes = soldnotes;
            }
            return View();
        }


    }
}