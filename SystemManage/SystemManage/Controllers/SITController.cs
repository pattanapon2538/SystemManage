using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;
using System.Threading;

namespace SystemManage.Controllers
{
    
    public class SITController : Controller
    {
        Entities db = new Entities();
        // GET: SIT
        public ActionResult AddSIT()
        {
            SITModel SIT = new SITModel();
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            SIT.Task = db.Tasks.Where(m => m.ProjectID == ProjectID).ToList();
            SIT.Tester = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 3).ToList();
            return View(SIT);
        }
        [HttpPost]
        public ActionResult AddSIT(SITModel model)
        {
            SIT s = new SIT();
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            s.Project_ID = ProjectID;
            s.Name = model.Name;
            s.Detail = model.Detail;
            s.CreateDate = DateTime.Now;
            s.CreateBy = Convert.ToInt32(Session["userID"]);
            db.SITs.Add(s);
            db.SaveChanges();
            return RedirectToAction("ShowTask", "Task");
        }
        public ActionResult ShowSIT()
        {
            SITModel data = new SITModel();
            List<SITModel> model = new List<SITModel>();
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            data.CreateBy = Convert.ToInt32(Session["userID"]);
            var sit = db.SITs.Where(m => m.Project_ID == projectID).ToList();
            foreach (var n in sit) {
                model.Add(new SITModel {
                    SIT_ID = n.SIT_ID,
                    Name = n.Name,
                    Detail = n.Detail,
                    CreateDate = n.CreateDate
                });
            }
            ViewBag.DataList = model;
            return View(data);
        }
    }
}