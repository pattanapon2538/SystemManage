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
            SIT.Tester = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 3).OrderBy(m => m.UserID).ToList();
            SIT.Dev = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 2).OrderBy(m => m.UserID).ToList();
            return View(SIT);
        }
        [HttpPost]
        public ActionResult AddSIT(SITModel model)
        {
            SIT s = new SIT();
            Database.SITStep Step = new Database.SITStep();
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            s.Project_ID = ProjectID;
            s.Name = model.Name;
            s.Detail = model.Detail;
            s.Tester_ID = model.Tester_ID;
            s.Dev_ID = model.Dev_ID;
            s.CreateDate = DateTime.Now;
            s.CreateBy = Convert.ToInt32(Session["userID"]);
            db.SITs.Add(s);
            db.SaveChanges();
            var c = model.TaskList.Count();
            for (int item = 0; item < c; item++)
            {
                Step.Task_ID = model.TaskList[item];
                Step.Step = item;
                Step.SIT_ID = s.SIT_ID;
                db.SITSteps.Add(Step);
                db.SaveChanges();
            }
                return RedirectToAction("ShowSIT", "SIT");
        }
        public ActionResult ShowSIT()
        {
            SITModel data = new SITModel();
            List<SITModel> model = new List<SITModel>();
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            data.CreateBy = Convert.ToInt32(Session["userID"]);
            var sit = db.SITs.Where(m => m.Project_ID == projectID).ToList();
            foreach (var n in sit)
            {
                model.Add(new SITModel
                {
                    SIT_ID = n.SIT_ID,
                    Name = n.Name,
                    Detail = n.Detail,
                    CreateDate = n.CreateDate
                });
            }
            ViewBag.DataList = model;
            return View(data);
        }
        public ActionResult DetailSIT(int SIT_ID)
        {
            var SIT = db.SITs.Where(m => m.SIT_ID == SIT_ID).FirstOrDefault();
            SITModel model = new SITModel();
            return View();
        }
        public ActionResult DeleteSIT(int SIT_ID)
        {
            var Step = db.SITSteps.Where(m => m.SIT_ID == SIT_ID).ToList();
            foreach (var item in Step)
            {
                db.SITSteps.Remove(item);
                db.SaveChanges();
            }
            var SIT = db.SITs.Where(m => m.SIT_ID == SIT_ID).FirstOrDefault();
            db.SITs.Remove(SIT);
            db.SaveChanges();
            return RedirectToAction("ShowSIT", "SIT");
        }
    }
}