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
            //var user = "Plus";//Session["userName"].ToString();
            Language_of_Type lg = new Language_of_Type();
            lg.languageID = model.languageID;
            lg.Name = model.languagesName;
            lg.CreateDate = DateTime.Now;
            lg.CreateBy = 11;
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
                    languagesName = i.Name,
                    CreateDate = i.CreateDate,
                    UpdateDate = i.UpdateDate,
                    CreateBy = 11,
                });
            }
            ViewBag.DataList = model;
            return View();
        }
        public ActionResult DetailLanguages(int languageID)
        {
            LanguageOfTypeModel model = new LanguageOfTypeModel();
            Language_of_Type lg = db.Language_of_Type.Where(w => w.languageID == languageID).FirstOrDefault();
            model.languageID = lg.languageID;
            model.languagesName = lg.Name;
            return View(model);
        }
        public ActionResult EditLanguages(LanguageOfTypeModel model)
        {
            Language_of_Type lg = db.Language_of_Type.Where(w => w.languageID == model.languageID).FirstOrDefault();
            lg.Name = model.languagesName;
            lg.UpdateDate = DateTime.Now;
            lg.UpdateBy = 11;
            db.SaveChanges();
            return RedirectToAction("ShowLanguage");
        }
        public ActionResult DeleteLanguage(int languageID)
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