using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using NoteMarketPlace.EmailTemplates;
using NoteMarketPlace.Helpers;
using NoteMarketPlace.Models;

namespace NoteMarketPlace.Controllers
{
    public class AdminController : Controller
    {


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
                    return RedirectToAction("Login", "Admin");
                }
                TempData["Message"] = "Invalid EmailAddress";
                return View();


            }

        }

        

        [HttpPost]
        public ActionResult changepassword()
        {
            return View();
        }
        public ActionResult addadministrator()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addadministrator(string s)
        {
            return View();
        }
        public ActionResult addcategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addcategory(Addcategory obj)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                    {
                        tbl_NoteCategory u = new tbl_NoteCategory();
                        u.CategoryName = obj.CategoryName;
                        u.Description = obj.Description;
                        u.IsActive = true;
                        u.DateAdded = DateTime.Now;


                        nm.tbl_NoteCategory.Add(u);
                        nm.SaveChanges();

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
        public ActionResult addcountry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addcountry(Addcountry obj)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                    {
                        tbl_Country u = new tbl_Country();
                        u.Name = obj.Name;
                        u.CountryCode = obj.CountryCode;
                        u.IsActive = true;
                        u.DateAdded = DateTime.Now;


                        nm.tbl_Country.Add(u);
                        nm.SaveChanges();

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
        public ActionResult addtype()
        {
            return View();
        }
        [HttpPost]
        public ActionResult addtype(Addtype obj)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    using (NoteMarketPlaceEntities nm = new NoteMarketPlaceEntities())
                    {
                        tbl_NoteType u = new tbl_NoteType();
                        u.TypeName = obj.TypeName;
                        u.Description = obj.Description;
                        u.IsActive = true;
                        u.DateAdded = DateTime.Now;


                        nm.tbl_NoteType.Add(u);
                        nm.SaveChanges();

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
        public ActionResult dashboard()
        {
            return View();
        }
        [HttpPost]
        public ActionResult dashboard(string s)
        {
            return View();
        }
        [HttpPost]
        public ActionResult downloadednotes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult manageadministrator()
        {
            return View();
        }
        [HttpPost]
        public ActionResult managecategory()
        {
            return View();
        }
        [HttpPost]
        public ActionResult managecountry()
        {
            return View();
        }
        [HttpPost]
        public ActionResult managesystemconfiguration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult managetype()
        {
            return View();
        }
        [HttpPost]
        public ActionResult memberdetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult members()
        {
            return View();
        }
        [HttpPost]
        public ActionResult myprofile()
        {
            return View();
        }
        [HttpPost]
        public ActionResult notedetails()
        {
            return View();
        }
        [HttpPost]
        public ActionResult notesunderreview()
        {
            return View();
        }
        [HttpPost]
        public ActionResult publishednotes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult rejectednotes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult spamreports()
        {
            return View();
        }
    }
}