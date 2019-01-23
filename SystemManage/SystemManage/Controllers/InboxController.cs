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
            List<InboxModel> InboxList = new List<InboxModel>();
            var userId = Convert.ToInt32(Session["userID"]);
            var u = db.Messages.Where(m => m.CreateBy == userId).OrderByDescending(m => m.CreateDate).ToList();
            foreach (var c in u)
            {
                var i = db.Mails.Where(m => m.Mail_ID == c.Mail_ID).FirstOrDefault();
                var k = db.Users.Where(m => m.User_ID == c.SendTo).FirstOrDefault();
                InboxList.Add(new InboxModel {
                    MailID = c.Message_ID.ToString(),
                    MailName = c.Name,
                    Status = i.Status,
                    SendTo = k.User_Name
                });
                ViewBag.DataList = InboxList;
            }
            return View();
        }
        public ActionResult AddInbox()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddInbox(InboxModel model)
        {
            Mail m = new Mail();
            Message ms = new Message();
            InboxController s = new InboxController();
            m.Name = model.MailName;
            m.Status = 1;
            m.CreateDate = DateTime.Now;
            m.CreateBy = Convert.ToInt32(Session["userID"]);
            db.Mails.Add(m);
            db.SaveChanges();
            ms.Mail_ID = m.Mail_ID;
            ms.Name = model.MailName;
            ms.Detail = model.MailDetail;
            ms.SendTo = Convert.ToInt32(model.SendTo);
            ms.CreateDate = DateTime.Now;
            ms.CreateBy = Convert.ToInt32(Session["userID"]);
            db.Messages.Add(ms);
            db.SaveChanges();
            var u = db.Users.Where(d => d.User_ID == ms.SendTo).FirstOrDefault();
            string receiver = u.User_Email;
            string subject = model.MailName;
            string message = model.MailDetail;
            var u2 = db.Users.Where(g => g.User_ID == m.CreateBy).FirstOrDefault();
            string sender = u2.User_Email;
            s.SendEmail(receiver, subject, message, sender);
            return RedirectToAction("Inbox", "Inbox");
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