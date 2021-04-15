using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace NotesMarketPlace.EmailTemplates
{
    public class unpublishSellerNote
    {
        public static void unpublishNote(string sellerName, string sellerEmailId, string remark)
        {
            var fromEmail = new MailAddress("supportEmail", "Notes Marketplace"); //need system email
            var toEmail = new MailAddress(sellerEmailId);
            var fromEmailPassword = "password"; // Replace with actual password
            string subject = "Sorry! We need to remove your notes from our portal.";

            string msg = "Hello " + sellerName + ",<br>";
            msg += "We want to inform you that, your note <Note Title> has been removed from the portal. Please find our remarks as below - ";
            msg += remark;



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