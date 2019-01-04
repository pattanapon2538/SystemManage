using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SystemManage.Controllers
{
    public class DefectController : Controller
    {
        // GET: Defect
        public ActionResult AddDefect()
        {
            return View();
        }
    }
}