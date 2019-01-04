using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Models;
using SystemManage.Database;

namespace SystemManage.Controllers
{
    
    public class ProjectController : Controller
    {
        Entities db = new Entities();
        // GET: Project
        public ActionResult AddProject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProject(ProjectModel model)
        {
            Project p = new Project();
            ProjectMember pm = new ProjectMember();
            p.Name = model.ProjectName;
            p.Description = model.ProjectDescription;
            p.SendDate = model.ProjectSendDate;
            p.Status = 1;
            p.CreateBy = Convert.ToInt32(Session["userID"]);
            p.CreateDate = DateTime.Now;
            db.Projects.Add(p);
            db.SaveChanges();
            pm.ProjectID = p.ProjectID;
            pm.UserID = Convert.ToInt32(Session["userID"]);
            pm.Role = 1; //1 = PM, 2= Dev, 3=Test, 4=QA, 5= Customer
            db.ProjectMembers.Add(pm);
            db.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("ShowProject");
        }
        public ActionResult ShowProject()
        {
            List<ProjectModel> projectlist = new List<ProjectModel>();
            int userID = Convert.ToInt32(Session["userID"]);
            var member = db.ProjectMembers.Where(m => m.UserID == userID).ToList();
            int countList = 0;
            double Percent = 0;
            foreach (var m in member)
            {
                var item = db.Projects.Where(p => p.ProjectID == m.ProjectID).FirstOrDefault();
                Project po = db.Projects.Where(pos => pos.ProjectID == item.ProjectID).FirstOrDefault();
                var item2 = db.Tasks.Where(t => t.ProjectID == item.ProjectID).ToList();
                foreach (var t in item2)
                {
                    var item3 = db.SubTasks.Where(st => st.TaskID == t.TaskID).ToList();
                    foreach(var s in item3)
                    {
                        Percent = Percent + s.SubPercent;
                        ++countList;
                    }
                }
                if (countList != 0)
                {
                    Percent = Percent / countList;
                    po.TotalPercent = Convert.ToInt32(Percent);
                    db.SaveChanges();
                }
                else
                {
                    Percent = 0;
                    po.TotalPercent = Convert.ToInt32(Percent);
                    db.SaveChanges();
                }
                    projectlist.Add(new ProjectModel
                    {
                        ProjectID = item.ProjectID,
                        ProjectName = item.Name,
                        ProjectRole = m.Role,
                        TotalPercent = Percent,
                        ProjectDescription = item.Description,
                        ProjectStatus = item.Status,
                        ProjectSendDate = item.SendDate,
                        CreateDate = item.CreateDate,
                        UpdateDate = item.UpdateDate,
                    });
                countList = 0;
                Percent = 0;
            }
            ViewBag.dataList = projectlist;
            return View();
        }
        public ActionResult EditProject(String ProjectID)
        {
            ProjectModel Model = new ProjectModel();
            Project p = db.Projects.Where(m => m.ProjectID.ToString() == ProjectID).FirstOrDefault();
            Model.ProjectID = p.ProjectID;
            Model.ProjectName = p.Name;
            Model.ProjectDescription = p.Description;
            Model.ProjectSendDate = p.SendDate;
            return View(Model);

        }
        public ActionResult SaveEdit(ProjectModel Model)
        {
            Project p = db.Projects.Where(m => m.ProjectID == Model.ProjectID).FirstOrDefault();
            p.Name = Model.ProjectName;
            p.Description = Model.ProjectDescription;
            p.Status = Model.ProjectStatus;
            p.UpdateDate = DateTime.Now;
            p.UpdateBy = Convert.ToInt32(Session["userID"]);
            db.SaveChanges();
            return RedirectToAction("ShowProject");

        }
        public ActionResult DeleteProject(int ProjectID)
        {
            Project d = db.Projects.Where(m => m.ProjectID == ProjectID).FirstOrDefault();
            var t = db.Tasks.Where(m => m.ProjectID == d.ProjectID).ToList();
            foreach (var item in t)
            {
                var st = db.SubTasks.Where(m => m.TaskID == item.TaskID).ToList();
                foreach (var item2 in st)
                {
                    db.SubTasks.Remove(item2);
                }
                db.Tasks.Remove(item);
            }
            db.Projects.Remove(d);
            db.SaveChanges();
            return RedirectToAction("ShowProject");
        }
        public ActionResult GetData(String ProjectID)
        {
            ProjectModel Model = new ProjectModel();
            Project p = db.Projects.Where(m => m.ProjectID.ToString() == ProjectID).FirstOrDefault();
            Session["ProjectID"] = p.ProjectID;
            Session["ProjectName"] = p.Name;
            return RedirectToAction("ShowTask", "Task");
        }
    }
}