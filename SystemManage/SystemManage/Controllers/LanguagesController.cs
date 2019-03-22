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
        public ActionResult AddLanguage(LanguageOfTypeModel lg)
        {
            if (lg.languageID != 0)
            {
                var results = db.Language_of_Type.Where(s => s.languageID == lg.languageID).FirstOrDefault();
                results.Name = lg.Name;
                results.UpdateDate = DateTime.Now;
                results.UpdateBy = Convert.ToInt32(Session["userID"]);
                db.SaveChanges();
                ModelState.Clear();
            }
            else
            {
                Language_of_Type model = new Language_of_Type();
                model.Name = lg.Name;
                model.CreateDate = DateTime.Now;
                model.CreateBy = Convert.ToInt32(Session["userID"]);
                db.Language_of_Type.Add(model);
                db.SaveChanges();
                ModelState.Clear();
            }
            //List<LanguageOfTypeModel> languagelist = new List<LanguageOfTypeModel>();
            //var result = db.Language_of_Type.OrderByDescending(s => s.languageID).ToList();
            //foreach (var i in result)
            //{
            //    languagelist.Add(new LanguageOfTypeModel
            //    {
            //        languageID = i.languageID,
            //        Name = i.Name,
            //        CreateDate = i.CreateDate,
            //        UpdateDate = i.UpdateDate,
            //        CreateBy = Convert.ToInt32(i.CreateBy),
            //        Updateby = Convert.ToInt32(i.UpdateBy),
            //    });
            //}
            //ViewBag.DataList = languagelist;
            //return PartialView("ShowLanguage");
           return RedirectToAction("ShowLanguage", "Languages");
        }

        public ActionResult ShowLanguage()
        {
            List<LanguageOfTypeModel> model = new List<LanguageOfTypeModel>();
            LanguageOfTypeModel a = new LanguageOfTypeModel();
            var item = db.Language_of_Type.OrderByDescending(s => s.languageID).ToList();
            foreach (var i in item)
            {
                model.Add(new LanguageOfTypeModel
                {
                    languageID = i.languageID,
                    Name = i.Name,
                    CreateDate = i.CreateDate,
                    UpdateDate = i.UpdateDate,
                    CreateBy = Convert.ToInt32(i.CreateBy),
                    Updateby = Convert.ToInt32(i.UpdateBy),
                });
            }
            ViewBag.DataList = model;
            if (Convert.ToInt32(Session["Alert_L"]) == 1)
            {
                a.Alert = 1;
                Session["Alert_L"] = 0;
                return View(a);
            }
            else
            {
                a.Alert = 0;
                return View(a);
            }
        }
        [HttpPost]
        public ActionResult DetailLanguages(int languageID)
        {
            LanguageOfTypeModel model = new LanguageOfTypeModel();
            Language_of_Type lg = db.Language_of_Type.Where(w => w.languageID == languageID).FirstOrDefault();
            model.languageID = lg.languageID;
            model.Name = lg.Name;
            return Json(new { languageID = lg.languageID, Name = lg.Name , Save = model.Save},JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult DeleteLanguage(int languageID)
        {
            try
            {
                LanguageOfTypeModel model = new LanguageOfTypeModel();
                Language_of_Type lg = db.Language_of_Type.Where(w => w.languageID == languageID).FirstOrDefault();
                db.Language_of_Type.Remove(lg);
                db.SaveChanges();
                var lg2 = db.Language_of_Type.ToList();
                return RedirectToAction("ShowLanguage");
            }
            catch (Exception)
            {
                Session["Alert_L"] = 1;
                return RedirectToAction("ShowLanguage", "Languages");
            }

        }
        //public ActionResult EditLanguages(LanguageOfTypeModel model) Edit แบบ ไม่ใช้ jquery
        //{
        //    Language_of_Type lg = db.Language_of_Type.Where(w => w.languageID == model.languageID).FirstOrDefault();
        //    lg.Name = model.languagesName;
        //    lg.UpdateDate = DateTime.Now;
        //    lg.UpdateBy = 11;
        //    db.SaveChanges();
        //    return RedirectToAction("ShowLanguage");
        //}
    }
}