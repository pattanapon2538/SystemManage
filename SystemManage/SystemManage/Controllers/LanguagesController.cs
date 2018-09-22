using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;

namespace SystemManage.Controllers
{
    public class LanguagesController : Controller
    {
        Entities db = new Entities();
        // GET: Languages
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create(Languages lang)
        {
            if(lang.languages != null)
            {
                Language lan = new Language();
                lan.languages = lang.languages;
                lan.UpdateDate = DateTime.Now;
                lan.CreateBy = "PLUS";
                lan.Lg_ID = Guid.NewGuid();
                db.Languages.Add(lan);
                db.SaveChanges();
            }
            return View();
        }
        public ActionResult Detail(string id)
        {
            Language l = db.Languages.Where(w => w.languages == "asd").FirstOrDefault();
            Languages la = new Languages();
            la.languages = l.languages;
            la.UpdateBy = l.Updateby; 
            la.UpdateDate = l.UpdateDate;
            return View();
        }

        public ActionResult Delete(string id)
        {
            var result = db.Languages.Where(w => w.languages == id);
            db.Languages.RemoveRange(result);
            db.SaveChanges();
            return View();
        }
    }
}