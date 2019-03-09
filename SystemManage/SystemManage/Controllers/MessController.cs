using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;


namespace SystemManage.Controllers
{
    public class MessController : Controller
    {
        Entities db = new Entities();
        // GET: Mess
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
            InboxController s = new InboxController();
            UploadFileController up = new UploadFileController();
            var test = Server.MapPath("~/Upload/");
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
            string path = Send(model);
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
            s.SendEmail(receiver, subject, message, sender);
            return RedirectToAction("Inbox", "Inbox");
        }

        public string Send(InboxModel photo)
        {
            foreach (var i in photo.AttachFile1)
            {
                if (i.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(i.FileName);
                    var test = Server.MapPath("~/Upload/");
                    var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    i.SaveAs(path);
                }
            }
            ViewBag.Message = "Image(s) uploaded successfully";
            return "";
        }
    }
}