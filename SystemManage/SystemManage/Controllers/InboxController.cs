using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Models;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SystemManage.Database;

namespace SystemManage.Controllers
{
    public class InboxController : Controller
    {
        Entities db = new Entities();
        // GET: Inbox
        public ActionResult Inbox()
        {
            return View();
        }
        
        public ActionResult SendEmail(string receiver, string subject, string message,string sender)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    UserModel model = new UserModel();
                    //var user = db.Users.Where(m => m.User_Email == sender).FirstOrDefault();
                    var senderEmail = new MailAddress(sender, sender);
                    var receiverEmail = new MailAddress(receiver, receiver);
                    var password = "cpe59346";
                    var sub = subject;
                    var body = message;
                    var smtp = new SmtpClient
                    {
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail.Address, password)
                    };
                    using (var mess = new MailMessage(senderEmail, receiverEmail)
                    {
                        Subject = subject,
                        Body = body
                    })
                    {
                        smtp.Send(mess);
                    }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
    }
}