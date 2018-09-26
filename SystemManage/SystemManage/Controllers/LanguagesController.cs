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
            Language_of_Type lg = new Language_of_Type();
            lg.languageID = model.languageID;
            lg.languagesName = model.languagesName;
            lg.CreateDate = model.CreateDate;
            lg.CreateBy = model.CreateBy;
            db.Language_of_Type.Add(lg);
            db.SaveChanges();
            return View();
        }
    }
}