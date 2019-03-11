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
        public ActionResult Index(DocumentModel model)
        {
            if (ModelState.IsValid)
            {
                int projectID = Convert.ToInt32(Session["ProjectID"]);
                Document d = new Document();
                var data = Process(model.AttachFile);
                //string[] txt = data.Split(",".ToCharArray());
                //string fileName = txt[0];
                string path = data;
                d.DocumentName = model.DocumentName;
                d.DocumentDetail = model.DocumentDetail;
                d.AttachFile = path;
                d.AttachShow = model.AttachFile.FileName;
                d.CreateDate = DateTime.Now;
                d.CreateBy = Convert.ToInt32(Session["userID"]);
                d.Project_ID = projectID;
                db.Documents.Add(d);
                db.SaveChanges();
                return RedirectToAction("ShowDocument");
            }
            return View();
        }
        public ActionResult ShowDocument()
        {
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            List<DocumentModel> DocumentList = new List<DocumentModel>();
            DocumentModel model = new DocumentModel();
            var d = db.Documents.Where(m => m.Project_ID == ProjectID).ToList();
            var pm = db.Projects.Where(m => m.ProjectID == ProjectID).FirstOrDefault();
            model.CreateBy = pm.CreateBy;
            foreach (var item in d)
            {
                string[] Type = item.AttachShow.Split(".".ToCharArray());
                DocumentList.Add(new DocumentModel {
                    DocumentID = item.DocumentID,
                    DocumentName = item.DocumentName,
                    AttachShow = Type[0],
                    pathFormView = item.AttachFile,
                    Type = Type[1],
                    CreateDate = item.CreateDate
                });
            }
            ViewBag.DataList = DocumentList;
            return View(model);
        }
        public ActionResult EditDocument(DocumentModel model)
        {
            var d = db.Documents.Where(m => m.DocumentID == model.DocumentID).FirstOrDefault();
            d.DocumentName = model.DocumentName;
            d.DocumentDetail = model.DocumentDetail;
            var data = Process(model.AttachFile);
            string[] txt = data.Split(",".ToCharArray());
            string fileName = txt[0];
            string path = txt[1];
            d.AttachShow = fileName;
            d.AttachFile = path;
            db.SaveChanges();
            return RedirectToAction("ShowDocument");
        }
        public ActionResult DetailDocument(int DocumentID)
        {
            var d = db.Documents.Where(m => m.DocumentID == DocumentID).FirstOrDefault();
            DocumentModel model = new DocumentModel();
            model.DocumentID = d.DocumentID;
            model.DocumentName = d.DocumentName;
            model.DocumentDetail = d.DocumentDetail;
            model.AttachShow = d.AttachShow;
            model.ShowPath = d.AttachFile;
            model.CreateBy = d.CreateBy;
            return View(model);
        }
        public ActionResult DeleteDocument(int DocumentID)
        {
            var d = db.Documents.Where(m => m.DocumentID == DocumentID).FirstOrDefault();
            System.IO.File.Delete("C:/Users/SWNGMN/source/repos/SystemManage/SystemManage/SystemManage" + d.AttachFile);
            db.Documents.Remove(d);
            db.SaveChanges();
            return RedirectToAction("ShowDocument");
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
                    string i = "/Upload/" + fileName;
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
         

    
