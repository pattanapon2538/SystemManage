using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
namespace SystemManage.Controllers
{
    public class LanguageController : Controller
    {
        // GET: Language
        public ActionResult AddOrEdit()
        {
            Language languageModel = new Language();
            return View(languageModel);
        }
    }
}