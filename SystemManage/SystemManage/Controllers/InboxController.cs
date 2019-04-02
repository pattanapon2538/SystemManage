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
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<InboxModel> InboxList = new List<InboxModel>();
            List<LogMessageModel> LogList = new List<LogMessageModel>();
            var userId = Convert.ToInt32(Session["userID"]);
            int item = 0;
            string Name_User = "";
            var u = db.Messages.Where(m => m.CreateBy == userId || m.SendTo == userId).OrderByDescending(m => m.CreateDate).ToList();
            int num = 1;
            foreach (var c in u)
            {
                var i = db.Mails.Where(m => m.Mail_ID == c.Mail_ID).FirstOrDefault();
                var l = db.LogMessages.Where(m => m.Message_ID == c.Message_ID).FirstOrDefault();
                var k = db.Users.Where(m => m.User_ID == l.Recive).FirstOrDefault();
                var Name = db.Users.Where(m => m.User_ID == c.SendTo).FirstOrDefault();
                if (num == 1)
                {
                    Name_User = Name.User_Name;
                }
                else
                {
                    Name_User = Name_User + "," + Name.User_Name;
                }
                if(u.Count == num)
                {
                    InboxList.Add(new InboxModel
                    {
                        MailID = c.Message_ID.ToString(),
                        MailName = c.Name,
                        Status = i.Status,
                        SendTo = Name_User
                    });
                }
                item = c.Mail_ID;
                num++;
            }
            ViewBag.DataList = InboxList;
            return View();
        }
        public ActionResult Index()
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var User = db.Users.Where(m => m.Permisstion != "A").OrderBy(m=>m.User_ID).ToList();
            List<UserModel> UserList = new List<UserModel>();
            foreach (var item in User)
            {
                UserList.Add(new UserModel {
                    Users_ID = item.User_ID,
                    User_Name = item.User_Name
                });
            }
            ViewBag.DataList = UserList;
            return View();
        }
        [HttpPost]
        public ActionResult Index(InboxModel model)
        {
            Mail m = new Mail();
            Message ms = new Message();
            LogMessage lm = new LogMessage();
            UploadFileController up = new UploadFileController();
            string[] txt = model.UserList.Split(",".ToCharArray());
            int c = txt.Count()-1;
            m.Name = model.MailName;
            m.Status = 1;
            m.CreateDate = DateTime.Now;
            m.CreateBy = Convert.ToInt32(Session["userID"]);
            db.Mails.Add(m);
            db.SaveChanges();
            for (int i = 0; i < c; i++)
            {
                ms.Mail_ID = m.Mail_ID;
                ms.Name = model.MailName;
                ms.Detail = model.MailDetail;
                ms.SendTo = Convert.ToInt32(txt[i]);
                ms.CreateDate = DateTime.Now;
                ms.CreateBy = Convert.ToInt32(Session["userID"]);
                if (model.AttachFile1 != null)
                {
                    var path = Process(model.AttachFile1);
                    string[] txt2 = path.Split(",".ToCharArray());
                    ms.AttachFile = txt2[0];
                    ms.AttachShow1 = txt2[1];
                }
                db.Messages.Add(ms);
                db.SaveChanges();
                lm.Log_Name = model.MailName;
                lm.Log_Message = model.MailDetail;
                lm.Recive = Convert.ToInt32(txt[i]);
                lm.CreateDate = DateTime.Now;
                lm.CreateBy = ms.CreateBy;
                lm.Message_ID = ms.Message_ID;
                db.LogMessages.Add(lm);
                db.SaveChanges();
                var u = db.Users.Where(d => d.User_ID == ms.SendTo).FirstOrDefault();
                var Name_Receiver = db.Users.Where(e => e.User_ID == ms.SendTo).FirstOrDefault();
                var Name_Sender = db.Users.Where(k => k.User_ID == ms.CreateBy).FirstOrDefault();
                string receiver = u.User_Email;
                string subject = model.MailName;
                string message = null;
                if (model.AttachFile1 != null)
                {
                    string Path_file = "http://localhost:8080"+ms.AttachFile;
                     message = "<html><body><p>ถึงคูณ " + Name_Receiver.User_Name + ",</p><p>&nbsp;&nbsp;&nbsp;&nbsp; " + model.MailDetail + "&nbsp;&nbsp;&nbsp;&nbsp; <a href= \"" + Path_file + "\">ไฟล์แนบ</a></p> <p> " + Name_Sender.User_Name + "</br></p></body></html>";
                }
                else
                {
                     message = "<html><body><p>ถึงคูณ " + Name_Receiver.User_Name + ",</p><p>&nbsp;&nbsp;&nbsp;&nbsp; " + model.MailDetail + "</p> <p> " + Name_Sender.User_Name + "</br></p></body></html>";
                }
                  var u2 = db.Users.Where(g => g.User_ID == m.CreateBy).FirstOrDefault();
                string sender = u2.User_Email;
                SendEmail(receiver, subject, message, sender);
            }
            return RedirectToAction("Inbox", "Inbox");
        }
        public ActionResult SendEmail(string receiver, string subject, string message, string sender)
        {
            try
            {
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
                        Body = body,
                    })
                    {
                    mess.IsBodyHtml = true;
                    smtp.Send(mess);
                    }
                    return View();
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
                return View();
            }
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
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            InboxModel model = new InboxModel();
            var i = db.Messages.Where(m => m.Message_ID == MailID).FirstOrDefault();
            var me = db.Mails.Where(m => m.Mail_ID == i.Mail_ID).FirstOrDefault();
            me.Status = 0;
            db.SaveChanges();
            model.MailID = i.Message_ID.ToString();
            model.MailName = i.Name;
            model.MailDetail = i.Detail;
            model.Image = i.AttachFile;
            model.AttachShow1 = i.AttachShow1;
            var u = db.Users.Where(m => m.User_ID == i.SendTo).FirstOrDefault();
            model.SendTo = u.User_Name;
            model.CreateBy = i.CreateBy;
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
        public string Process(HttpPostedFileBase photo)
        {
            if (!isValidContentType(photo.ContentType))
            {
                ViewBag.Error = "เฉพาะไฟล์ jpg png pdf";
                return ("Error");
            }
            else if (!isValidContentLength(photo.ContentLength))
            {
                ViewBag.Error = "ไฟล์มีขนาดใหญ่เกินไป (1MB)";
                return ("Error");
            }
            else
            {
                if (photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/Upload/") + fileName);
                    string i = "/Upload/" + fileName + "," + fileName;
                    photo.SaveAs(path);
                    ViewBag.fileName = photo.FileName;
                    if (photo.ContentType.Equals("application/pdf"))
                    {
                        return i;
                    }
                    else

                        return i;
                }
                return ("Error");
            }
        }
    }
}