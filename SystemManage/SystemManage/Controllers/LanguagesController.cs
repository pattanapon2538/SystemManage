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
        [HttpPost]
        public ActionResult AddLanguage(LanguageOfTypeModel language)
        {
            if (ModelState.IsValid)
            {
                Language_of_Type model = new Language_of_Type();
                model.Name = language.languagesName;
                model.CreateDate = DateTime.Now;
                model.CreateBy = 11;
                db.Language_of_Type.Add(model);
                db.SaveChanges();
                ModelState.Clear();
            }
            List<LanguageOfTypeModel> languagelist = new List<LanguageOfTypeModel>();
            var result = db.Language_of_Type.OrderByDescending(s => s.languageID).ToList();
            foreach (var i in result)
            {
                languagelist.Add(new LanguageOfTypeModel
                {
                    languageID = i.languageID,
                    languagesName = i.Name,
                    CreateDate = i.CreateDate,
                    UpdateDate = i.UpdateDate,
                    CreateBy = Convert.ToInt16(i.CreateBy),
                });
            }
            ViewBag.DataList = languagelist;
            return PartialView("ShowLanguage");
        }
            
        public ActionResult ShowLanguage()
        {
            List<LanguageOfTypeModel> model = new List<LanguageOfTypeModel>();
            var item = db.Language_of_Type.OrderByDescending(s=>s.languageID).ToList();
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