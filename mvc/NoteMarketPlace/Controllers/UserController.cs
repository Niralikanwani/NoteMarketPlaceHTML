using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NoteMarketPlace.Models;
using NoteMarketPlace.EmailTemplates;
using System.Web.Security;
using NoteMarketPlace.Helpers;
using System.IO;

namespace NoteMarketPlace.Controllers
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
                            ViewBag.Title = "NoteMarketPlace";


                            return RedirectToAction("emailverification", "User");
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
                        Session["Email_id"] = obj.Email_id;
                        var user = nm.tbl_Users.Where(x => x.Email_id == obj.Email_id).FirstOrDefault();
                        Session["FullName"] = user.FirstName + " " + user.LastName;
                        if(user.Role_id == 3)
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
        public ActionResult dashboard()
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {

                int userid = (int)Session["Userid"];
                ViewBag.mySoldNotes = nm.tbl_Download.Where(x => x.Seller_id == userid).Count();
                ViewBag.totalEarning = nm.tbl_Download.Where(x => x.Seller_id == userid).Select(x => x.PurchasedPrice).Sum();
                ViewBag.myDownloadNotes = nm.tbl_Download.Where(x => x.Downloader_id == userid && x.IsSellerHasAllowedDownload == true).Count();
                ViewBag.myRejectedNotes = nm.tbl_Notes.Where(x => x.Seller_id == userid && x.Status == 3).Count();
                ViewBag.buyerRequests = nm.tbl_Download.Where(x => x.Seller_id == userid && x.IsSellerHasAllowedDownload == false).Count();
                var ProgressNotes = nm.tbl_Notes.Where(x => x.Seller_id == userid && x.Status == 4 || x.Status == 1).ToList();
                ViewBag.inProgressNotes = ProgressNotes;

                var PublishedNoted = nm.tbl_Notes.Where(x => x.Seller_id == userid && x.Status == 2).ToList();
                ViewBag.pubishedNote = PublishedNoted;

            }
            return View();
        }

       
        [HttpGet]
        public ActionResult addnote()
        {
            using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
            {
                var notecategory = nm.tbl_NoteCategory.ToList();
                var notetype = nm.tbl_NoteType.ToList();
                var country = nm.tbl_Country.ToList();
                ViewBag.notecategoies = new SelectList(notecategory, "NoteCategory_id", "CategoryName");
                ViewBag.notetypes = new SelectList(notetype, "NoteType_id", "TypeName");
                ViewBag.countries = new SelectList(country, "Country_id", "Name");
            }
            return View();
        }

        [HttpPost]
        public ActionResult addnote(Addnote notedetails)
        {

            if (ModelState.IsValid)
            {
                if (notedetails.IsPaid == true && notedetails.NotesPreview == null)
                {
                    NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities();
                    ViewBag.previewmessage = "Note Preview Is Required For Paid Note...";
                    var notecategory = nm.tbl_NoteCategory.ToList();
                    var notetype = nm.tbl_NoteType.ToList();
                    var country = nm.tbl_Country.ToList();
                    ViewBag.notecategoies = new SelectList(notecategory, "NoteCategory_id", "CategoryName");
                    ViewBag.notetypes = new SelectList(notetype, "NoteType_id", "TypeName");
                    ViewBag.countries = new SelectList(country, "Country_id", "Name");
                    return View();

                }

                using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                {
                    tbl_Notes nd = new tbl_Notes();
                    tbl_NoteAttachments sna = new tbl_NoteAttachments();
                    nd.Seller_id = (int)Session["Userid"];
                    nd.Status = 4;
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
                            FilePath += Path.Combine(("~/Member/" + Session["Userid"].ToString() + "/" + nd.Id.ToString() + "/"), fileName) + ";";
                            count++;
                        }
                        sna.FileName = FileName;
                        sna.FilePath = FilePath;

                        sna.Id = nd.Id;
                        sna.IsActive = true;
                        sna.DateAdded = DateTime.Now;
                        sna.AddedBy = (int)Session["Userid"];
                        nm.SaveChanges();



                    }
                    nm.tbl_NoteAttachments.Add(sna);
                    nm.SaveChanges();


                }

                ModelState.Clear();
                return RedirectToAction("addnote", "User");
            }


            return View();
        }


        public ActionResult changepassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult changepassword(string s)
        {
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

        [HttpPost]
        public ActionResult emailverification(string s)
        {
            return View();
        }

        public ActionResult userprofile()
        {
            return View();
        }

        [HttpPost]
        public ActionResult userprofile(string s)
        {
            return View();
        }

        public ActionResult searchnotes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult searchnotes(string s)
        {
            return View();
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

        public ActionResult buyersrequests()
        {
            return View();
        }

        [HttpPost]
        public ActionResult buyersrequests(string s)
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

        public ActionResult myrejectednotes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult myrejectednotes(string s)
        {
            return View();
        }

        public ActionResult mysoldnotes()
        {
            return View();
        }

        [HttpPost]
        public ActionResult mysoldnotes(string s)
        {
            return View();
        }

        public ActionResult notedetail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult notedetail(string s)
        {
            return View();
        }
    }
}