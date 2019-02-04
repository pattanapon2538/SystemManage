using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SystemManage.Controllers
{
    public class UploadFileController : Controller
    {
        // GET: Upload
        public ActionResult Index()
        {
            return View();
        }

        private bool isValidContentType(string contentType) {
            return contentType.Equals("image/png") || contentType.Equals("image/jpg") || contentType.Equals("application/pdf") || contentType.Equals("image/jpeg");
        }

        private bool isValidContentLength(int contentLength) {
            return ((contentLength / 1024) / 1024) < 1; // 1MB
        }
        public ActionResult Process(HttpPostedFileBase photo)
        {
            if (!isValidContentType(photo.ContentType))
            {
                ViewBag.Error = "เฉพาะไฟล์ jpg png pdf";
                return View("Index");
            }
            else if (!isValidContentLength(photo.ContentLength))
            {
                ViewBag.Error = "ไฟล์มีขนาดใหญ่เกินไป (1MB)";
                return View("Index");
            }
            else
            {
                if (photo.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(photo.FileName);
                    var path = Path.Combine(Server.MapPath("~/Upload"), fileName);
                    photo.SaveAs(path);
                    ViewBag.fileName = photo.FileName;
                    if (photo.ContentType.Equals("application/pdf"))
                    {
                        return View("Success");
                    }
                    else
                        return View("Success");
                }
                return View("Success");
            }
        }
    }    
}
         

    
