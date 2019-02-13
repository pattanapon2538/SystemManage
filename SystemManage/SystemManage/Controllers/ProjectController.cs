using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Models;
using SystemManage.Database;
using SystemManage.Controllers;
using Newtonsoft.Json;

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
            try
            {
                InboxController i = new InboxController();
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
                var Email = db.Users.Where(m => m.User_ID == p.CreateBy).FirstOrDefault();
                string sender = Email.User_Email;
                //string sender = "systemmanage59346@gmail.com";
                string subject = model.ProjectName;
                //string receiver = model.CreateBy.ToString();
                string receiver = "plusth2538@gmail.com";
                string mess = "ท่านได้มีการสร้างโครงการ" + model.ProjectName + "ตำแหน่งของคุณคือหัวโครงการ";
                i.SendEmail(receiver, subject, mess, sender);
                return RedirectToAction("ShowProject");
            }
            catch (Exception)
            {
                return View();
            }
        }
        public ActionResult ShowProject()
        {
            List<ProjectModel> projectlist = new List<ProjectModel>();
            ProjectModel model = new ProjectModel();
            int userID = Convert.ToInt32(Session["userID"]);
            var member = db.ProjectMembers.Where(m => m.UserID == userID).ToList();
            int countList = 0;
            double Percent = 0;
            foreach (var m in member)
            {
                model.UserRole = m.Role;
                var item = db.Projects.Where(p => p.ProjectID == m.ProjectID).FirstOrDefault();
                var pm = db.Users.Where(u => u.User_ID == item.CreateBy).FirstOrDefault();
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
                        CreateBy = pm.User_Name,
                        CreateDate = item.CreateDate,
                        UpdateDate = item.UpdateDate,
                    });
                countList = 0;
                Percent = 0;
            }
            ViewBag.dataList = projectlist;
            return View(model);
        }
        public ActionResult EditProject(String ProjectID)
        {
            ProjectModel Model = new ProjectModel();
            Project p = db.Projects.Where(m => m.ProjectID.ToString() == ProjectID).FirstOrDefault();
            Model.ProjectID = p.ProjectID;
            Model.ProjectName = p.Name;
            Session["ProjectName"] = p.Name;
            Model.ProjectDescription = p.Description;
            Model.ProjectSendDate = p.SendDate;
            if (p.Status == 1)
            {
                Model.status = ProjectModel.Status.Processing;
            }
            else if (p.Status == 2)
            {
                Model.status = ProjectModel.Status.Pause;
            }
            else if (p.Status == 3)
            {
                Model.status = ProjectModel.Status.Terminate;
            }
            return View(Model);

        }
        public ActionResult SaveEdit(ProjectModel Model)
        {
            Project p = db.Projects.Where(m => m.ProjectID == Model.ProjectID).FirstOrDefault();
            p.Name = Model.ProjectName;
            p.Description = Model.ProjectDescription;
            if (Model.status.ToString() == "Terminate")
            {
                p.Status = 3;
            }
            else if (Model.status.ToString() == "Pause")
            {
                p.Status = 2;
            }
            else if (Model.status.ToString() == "Processing")
            {
                p.Status = 1;
            }
            p.UpdateDate = DateTime.Now;
            p.UpdateBy = Convert.ToInt32(Session["userID"]);
            db.SaveChanges();
            var p2 = db.ProjectMembers.Where(m => m.ProjectID == Model.ProjectID).ToList();
            foreach (var e in p2)
            {
                var Email = db.Users.Where(m => m.User_ID == e.UserID).FirstOrDefault();
                string sender = Email.User_Email;
                //string sender = "systemmanage59346@gmail.com";
                string subject = Model.ProjectName;
                //string receiver = model.CreateBy.ToString();
                string receiver = "plusth2538@gmail.com";
                string mess = "โครงการ" + Model.ProjectName + "ได้มีการแก้ไขข้อมูลสถานะของโครงการ" + Model.status;
                InboxController i = new InboxController();
                i.SendEmail(receiver, subject, mess, sender);

            }
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
            var pm = db.ProjectMembers.Where(s => s.ProjectID == ProjectID).ToList();
            foreach (var c in pm)
            {
                var Email = db.Users.Where(m => m.User_ID == c.UserID).FirstOrDefault();
                string sender = Email.User_Email;
                //string sender = "systemmanage59346@gmail.com";
                string subject = d.Name;
                //string receiver = d.CreateBy.ToString(); รอส่งเมล์ให้ทั้งทีม
                string receiver = "plusth2538@gmail.com";
                string mess = "โครงการ" + d.Name + "ได้ถูกลบ";
                InboxController i = new InboxController();
                i.SendEmail(receiver, subject, mess, sender);
            }
            var pm2 = db.ProjectMembers.Where(s => s.ProjectID == ProjectID).FirstOrDefault();
            db.ProjectMembers.Remove(pm2);
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
        public ActionResult DetailProject(int ProjectID)
        {
            ProjectModel model = new ProjectModel();
            List<Chart> dataPoints = new List<Chart>();
            List<ProjectModel> TaskList = new List<ProjectModel>();
            var p = db.Projects.Where(m => m.ProjectID == ProjectID).FirstOrDefault();
            var t = db.Tasks.Where(m => m.ProjectID == ProjectID).ToList();
            foreach (var c in t)
            {
                double Total = 0;
                var s = db.SubTasks.Where(m => m.TaskID == c.TaskID).ToList();
                foreach (var d in s)
                {
                    Total = Total + d.SubPercent;
                }
                Total = Total / s.Count;
                c.TotalPercent = Total;
                db.SaveChanges();
            }
            Session["ProjectName"] = p.Name;
            model.ProjectDescription = p.Description;
            double total = 100/ t.Count;
            foreach (var item in t)
            {
                double TaskPercent = (item.TotalPercent / 100) * total;
                dataPoints.Add(new Chart(item.TaskName, TaskPercent));
                TaskList.Add(new ProjectModel {
                    TaskName = item.TaskName,
                    TaskPercent = item.TotalPercent
                });
            }
            ViewBag.DataList2 = TaskList;
            ViewBag.DataList = JsonConvert.SerializeObject(dataPoints);
            return View(model);
        }
    }
}