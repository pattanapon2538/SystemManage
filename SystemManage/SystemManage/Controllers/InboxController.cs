using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Models;
using System.Net;
using System.Net.Mail;
using SystemManage.Database;
using SystemManage.Controllers;
using System.IO;

namespace SystemManage.Controllers
{
    public class InboxController : Controller
    {
        Entities db = new Entities();
        // GET: Inbox
        public ActionResult Inbox()
        {
            List<InboxModel> InboxList = new List<InboxModel>();
            List<LogMessageModel> LogList = new List<LogMessageModel>();
            var userId = Convert.ToInt32(Session["userID"]);

            var u = db.Messages.Where(m => m.CreateBy == userId || m.SendTo == userId).OrderByDescending(m => m.CreateDate).ToList();
            foreach (var c in u)
            {
                var i = db.Mails.Where(m => m.Mail_ID == c.Mail_ID).FirstOrDefault();
                var l = db.LogMessages.Where(m => m.Message_ID == c.Message_ID).FirstOrDefault();
                var k = db.Users.Where(m => m.User_ID == l.Recive).FirstOrDefault();
                InboxList.Add(new InboxModel
                {
                    MailID = c.Message_ID.ToString(),
                    MailName = c.Name,
                    Status = i.Status,
                    SendTo = k.User_Name
                });
            }
            ViewBag.DataList = InboxList;
            return View();
        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(InboxModel model)
        {
            Mail m = new Mail();
            Message ms = new Message();
            LogMessage lm = new LogMessage();
            UploadFileController up = new UploadFileController();
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
            string path = Process(model);
            ms.AttachFile = path;
            //ms.AttachShow1 = model.AttachFile1.FileName;
            db.Messages.Add(ms);
            db.SaveChanges();
            lm.Log_Name = model.MailName;
            lm.Log_Message = model.MailDetail;
            lm.Recive = Convert.ToInt32(model.SendTo);
            lm.CreateDate = DateTime.Now;
            lm.CreateBy = ms.CreateBy;
            lm.Message_ID = ms.Message_ID;
            db.LogMessages.Add(lm);
            db.SaveChanges();
            var u = db.Users.Where(d => d.User_ID == ms.SendTo).FirstOrDefault();
            string receiver = u.User_Email;
            string subject = model.MailName;
            string message = model.MailDetail;
            var u2 = db.Users.Where(g => g.User_ID == m.CreateBy).FirstOrDefault();
            string sender = u2.User_Email;
            SendEmail(receiver, subject, message, sender);
            return RedirectToAction("Inbox", "Inbox");


        }
        public ActionResult SendEmail(string receiver, string subject, string message, string sender)
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
        public ActionResult DeleteMessage(int MailID)
        {
            var d = db.Messages.Where(m => m.Message_ID == MailID).FirstOrDefault();
            if (d.CreateBy == Convert.ToInt32(Session["userID"]))
            {
                if (d.SendTo != 0 && d.CreateBy != 0)
                {
                    d.CreateBy = 0;
                    db.SaveChanges();
                }
                else if (d.SendTo == 0)
                {
                    var m = db.Mails.Where(t => t.Mail_ID == d.Mail_ID).FirstOrDefault();
                    db.Messages.Remove(d);
                    db.SaveChanges();
                    db.Mails.Remove(m);
                    db.SaveChanges();
                }
            }
            else if (d.SendTo == Convert.ToInt32(Session["userID"]))
            {
                if (d.SendTo != 0 && d.CreateBy != 0)
                {
                    d.SendTo = 0;
                    db.SaveChanges();
                }
                else if (d.CreateBy == 0)
                {
                    var m = db.Mails.Where(t => t.Mail_ID == d.Mail_ID).FirstOrDefault();
                    db.Messages.Remove(d);
                    db.SaveChanges();
                    db.Mails.Remove(m);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("Inbox");
        }
        public ActionResult ReadMail(int MailID)
        {
            InboxModel model = new InboxModel();
            var i = db.Messages.Where(m => m.Message_ID == MailID).FirstOrDefault();
            var me = db.Mails.Where(m => m.Mail_ID == i.Mail_ID).FirstOrDefault();
            me.Status = 0;
            db.SaveChanges();
            model.MailID = i.Message_ID.ToString();
            model.MailName = i.Name;
            model.MailDetail = i.Detail;
            model.Image = i.AttachFile;
            var u = db.Users.Where(m => m.User_ID == i.SendTo).FirstOrDefault();
            model.SendTo = u.User_Name;
            model.CreateBy = i.CreateBy.ToString();
            return View(model);
        }
        private bool isValidContentType(string contentType)
        {
            return contentType.Equals("image/png") || contentType.Equals("image/jpg") || contentType.Equals("application/pdf") || contentType.Equals("image/jpeg");
        }

        private bool isValidContentLength(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1; // 1MB
        }

        public string Process(InboxModel photo)
        {
            foreach (var i in photo.AttachFile1)
            {
                if (i.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(i.FileName);
                    //var test = Server.MapPath();
                    var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    string item = "/Upload/" + fileName;
                    i.SaveAs(path);
                    return item;
                }
            }
            ViewBag.Message = "Image(s) uploaded successfully";
            return "";
        }
    }
}