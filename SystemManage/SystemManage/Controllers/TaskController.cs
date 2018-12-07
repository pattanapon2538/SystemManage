using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;

namespace SystemManage.Controllers
{
    public class TaskController : Controller
    {
        Entities db = new Entities();
        // GET: Task
        public ActionResult AddTask()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddTask(int ProjectID)
        {
            
        }
    }
}