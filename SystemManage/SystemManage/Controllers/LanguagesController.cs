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
        public ActionResult AddLanguages()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddLanguages(LanguageOfTypeModel model)
        {
            var user = "Plus";//Session["userName"].ToString();
            Language_of_Type lg = new Language_of_Type();
            lg.languageID = model.languageID;
            lg.languagesName = model.languagesName;
            lg.CreateDate = DateTime.Now;
            lg.CreateBy = user;
            db.Language_of_Type.Add(lg);
            db.SaveChanges();
            return RedirectToAction("ShowLanguage");
        }
        public ActionResult ShowLanguage()
        {
            List<LanguageOfTypeModel> model = new List<LanguageOfTypeModel>();
            var item = db.Language_of_Type.ToList();
            foreach (var i in item)
            {
                model.Add(new LanguageOfTypeModel
                {
                    languageID = i.languageID,
                    languagesName = i.languagesName,
                    CreateDate = i.CreateDate,
                    UpdateDate = i.UpdateDate,
                    CreateBy = "Admin",
                });
            }
            ViewBag.DataList = model;
            return View();
        }
        public ActionResult DetailLanguages(string languageID)
        {
            LanguageOfTypeModel model = new LanguageOfTypeModel();
            Language_of_Type lg = db.Language_of_Type.Where(w => w.languageID == languageID).FirstOrDefault();
            model.languageID = lg.languageID;
            model.languagesName = lg.languagesName;
            return View(model);
        }
        public ActionResult EditLanguages(LanguageOfTypeModel model)
        {
            Language_of_Type lg = db.Language_of_Type.Where(w => w.languageID == model.languageID).FirstOrDefault();
            lg.languagesName = model.languagesName;
            lg.UpdateDate = DateTime.Now;
            lg.Updateby = "Admin";
            db.SaveChanges();
            return RedirectToAction("ShowLanguage");
        }
        public ActionResult DeleteLanguage(string languageID)
        {
            LanguageOfTypeModel model = new LanguageOfTypeModel();
            Language_of_Type lg = db.Language_of_Type.Where(w => w.languageID == languageID).FirstOrDefault();
            db.Language_of_Type.Remove(lg);
            db.SaveChanges();
            var lg2 = db.Language_of_Type.ToList();
            return RedirectToAction("ShowLanguage");

        }
        
    }
}