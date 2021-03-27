using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using NotesMarketPlace.Models;

namespace NotesMarketPlace.EmailTemplates
{
    public class forgotpassword
    {
        public static void SendOtpToEmail(tbl_Users u, string otp)
        {
            var fromEmail = new MailAddress("15nhk2000@gmail.com", "Notes Marketplace"); //system email
            var toEmail = new MailAddress(u.Email_id);
            var fromEmailPassword = "$nhk1111"; //actual password
            string subject = "New Temporary Password has been created for you";
            string msg = "Hello " + u.FirstName + " " + u.LastName + "<br/>";
            msg += "We have generated a new password for you <br/>";
            msg += "Password: " + otp;
            msg += "<br/><br/>Regards,<br/>";
            msg += "Notes Marketplace";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = msg,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }
    }
}