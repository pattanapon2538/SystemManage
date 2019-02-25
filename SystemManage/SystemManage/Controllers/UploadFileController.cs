using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;
using SystemManage.Controllers;
using System.Diagnostics;

namespace SystemManage.Controllers
{
    public class UploadFileController : Controller
    {
        Entities db = new Entities();
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddDocument(DocumentModel model)
        {
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            Document d = new Document();
            var data = Process(model.AttachFile);
            string[] txt = data.Split(",".ToCharArray());
            string fileName = txt[0];
            string path = txt[1];
            d.DocumentName = model.DocumentName;
            d.DocumentDetail = model.DocumentDetail;
            d.AttachFile = path;
            d.AttachShow = fileName;
            d.CreateDate = DateTime.Now;
            d.CreateBy = Convert.ToInt32(Session["userID"]);
            d.Project_ID = projectID;
            db.Documents.Add(d);
            db.SaveChanges();
            return Json(d.DocumentID, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ShowDocument()
        {
            return View();
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
                    var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    string i = fileName + "," + path;
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
         

    
