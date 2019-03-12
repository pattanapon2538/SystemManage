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
                if (model.SIT.ToString() == "ใช่")
                {
                    p.SIT_Menu = 1;
                }
                else if (model.SIT.ToString() == "ไม่")
                {
                    p.SIT_Menu = 0;
                }
                db.Projects.Add(p);
                db.SaveChanges();
                pm.ProjectID = p.ProjectID;
                pm.UserID = Convert.ToInt32(Session["userID"]);
                pm.Role = 1; //1 = PM, 2= Dev, 3=Test, 4=QA, 5= Customer
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                ModelState.Clear();
                //var Email = db.Users.Where(m => m.User_ID == p.CreateBy).FirstOrDefault();
                //string sender = Email.User_Email;
                ////string sender = "systemmanage59346@gmail.com";
                //string subject = model.ProjectName;
                ////string receiver = model.CreateBy.ToString();
                //string receiver = "plusth2538@gmail.com";
                //string mess = "ท่านได้มีการสร้างโครงการ" + model.ProjectName + "ตำแหน่งของคุณคือหัวโครงการ";
                //i.SendEmail(receiver, subject, mess, sender);
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
            ProjectController project = new ProjectController();
            int userID = Convert.ToInt32(Session["userID"]);
            project.Check_Project(userID);
            var member = db.ProjectMembers.Where(m => m.UserID == userID).OrderBy(m => m.ProjectID).ToList();
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
                    foreach (var s in item3)
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
                Model.status = ProjectModel.Status.ดำเนินการ;
            }
            else if (p.Status == 2)
            {
                Model.status = ProjectModel.Status.พัก;
            }
            else if (p.Status == 3)
            {
                Model.status = ProjectModel.Status.หยุดดำเนินการ;
            }
            if (p.SIT_Menu == 1)
            {
                Model.SIT = ProjectModel.SIT_State.ใช่;
            }
            else if (p.SIT_Menu == 0)
            {
                Model.SIT = ProjectModel.SIT_State.ไม่ใช่;
            }
            return View(Model);

        }
        public ActionResult SaveEdit(ProjectModel Model)
        {
            string status = null;
            Project p = db.Projects.Where(m => m.ProjectID == Model.ProjectID).FirstOrDefault();
            p.Name = Model.ProjectName;
            p.Description = Model.ProjectDescription;
            if (Model.status.ToString() == "หยุดดำเนินการ")
            {
                p.Status = 3;
            }
            else if (Model.status.ToString() == "พัก")
            {
                p.Status = 2;
            }
            else if (Model.status.ToString() == "ดำเนินการ")
            {
                p.Status = 1;
            }
            if (Model.SIT.ToString() == "ใช่")
            {
                p.SIT_Menu = 1;
            }
            else if (Model.SIT.ToString() == "ไม่ใช่")
            {
                p.SIT_Menu = 0;
            }
            p.UpdateDate = DateTime.Now;
            p.UpdateBy = Convert.ToInt32(Session["userID"]);
            db.SaveChanges();
            var PM = db.Users.Where(m => m.User_ID == p.CreateBy).FirstOrDefault();
            var p2 = db.ProjectMembers.Where(m => m.ProjectID == Model.ProjectID).ToList();
            foreach (var e in p2)
            {
                var Email = db.Users.Where(m => m.User_ID == e.UserID).FirstOrDefault();
                string sender = PM.User_Email;
                //string sender = "systemmanage59346@gmail.com";
                string subject = Model.ProjectName;
                string receiver = Email.User_Email;
                //string receiver = "plusth2538@gmail.com";
                if (Convert.ToInt32(Model.status) == 1)
                {
                     status = "ดำเนินการ";
                }
                else if(Convert.ToInt32(Model.status) == 2)
                {
                     status = "พัก";
                }
                else if (Convert.ToInt32(Model.status) == 1)
                {
                     status = "หยุดดำเนินการ";
                }
                string mess = "โครงการ" + Model.ProjectName + "ได้มีการแก้ไขข้อมูลสถานะของโครงการ" + status + "วันที่ที่แก้ไข" + p.UpdateDate.ToString() +"ผู้แก้ไข"  + PM.User_Name ;
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
            var CreateBy = db.Users.Where(m => m.User_ID == d.CreateBy).FirstOrDefault();
            var pm = db.ProjectMembers.Where(s => s.ProjectID == ProjectID).ToList();
            foreach (var c in pm)
            {
                DateTime time = DateTime.Now;
                var Email = db.Users.Where(m => m.User_ID == c.UserID).FirstOrDefault();
                string sender = Email.User_Email;
                //string sender = "systemmanage59346@gmail.com";
                string subject = d.Name;
                //string receiver = d.CreateBy.ToString(); รอส่งเมล์ให้ทั้งทีม
                string receiver = "plusth2538@gmail.com";
                string mess = "โครงการ" + d.Name + "ได้ถูกลบ" + "ผู้ลบโครงการ" + CreateBy.User_Name + "วันที่" + time ;
                InboxController i = new InboxController();
                i.SendEmail(receiver, subject, mess, sender);
                db.ProjectMembers.Remove(c);
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
            if (t.Count != 0)
            {
                double total = 100 / t.Count;
            }
            foreach (var item in t)
            {
                //double TaskPercent = (item.TotalPercent / 100) * total;
                dataPoints.Add(new Chart(item.TaskName, item.TotalPercent /*TaskPercent*/));
                TaskList.Add(new ProjectModel
                {
                    TaskName = item.TaskName,
                    TaskPercent = item.TotalPercent
                });
            }
            ViewBag.DataList2 = TaskList;
            ViewBag.DataList = JsonConvert.SerializeObject(dataPoints);
            return View(model);
        }
        public ActionResult Check_Project(int userID)
        {
            var user = db.ProjectMembers.Where(m => m.UserID == userID).ToList();
            foreach (var item in user)
            {
                var project = db.Projects.Where(m => m.ProjectID == item.ProjectID).FirstOrDefault();
                if (project.Status != 4)
                {
                    if (project.SIT_Menu == 0)
                    {
                        double count = 0;
                        var Task = db.Tasks.Where(m => m.ProjectID == item.ProjectID).ToList();
                        foreach (var item2 in Task)
                        {
                            count = count + item2.TotalPercent;
                        }
                        count = (count / Task.Count());
                        if (count == 100 && project.SendDate >= DateTime.Now)
                        {
                            project.Status = 4;
                            db.SaveChanges();
                            var PM = db.Users.Where(m => m.User_ID == project.CreateBy).FirstOrDefault();
                            PM.Amount_Succ = PM.Amount_Succ + 1;
                            db.SaveChanges();
                            InboxController Inbox = new InboxController();
                            var member = db.ProjectMembers.Where(m => m.ProjectID == project.ProjectID).ToList();
                            foreach (var c in member)
                            {
                                var re = db.Users.Where(m => m.User_ID == c.UserID).FirstOrDefault();
                                var se = db.Users.Where(m => m.User_ID == project.CreateBy).FirstOrDefault();
                                string receiver = re.User_Email;
                                string subject = project.Name;
                                string mess = "โครการ" + project.Name + "ได้มีสถานะเป็นเสร็จสมบูรณ์";
                                string sender = se.User_Email;
                                Inbox.SendEmail(receiver, subject, mess, sender);
                            }

                        }
                        else if (count == 100 && project.SendDate < DateTime.Now)
                        {
                            project.Status = 4;
                            db.SaveChanges();
                            var PM = db.Users.Where(m => m.User_ID == project.CreateBy).FirstOrDefault();
                            PM.Amount_Non = PM.Amount_Non + 1;
                            db.SaveChanges();
                            InboxController Inbox = new InboxController();
                            var member = db.ProjectMembers.Where(m => m.ProjectID == project.ProjectID).ToList();
                            foreach (var c in member)
                            {
                                var re = db.Users.Where(m => m.User_ID == c.UserID).FirstOrDefault();
                                var se = db.Users.Where(m => m.User_ID == project.CreateBy).FirstOrDefault();
                                string receiver = re.User_Email;
                                string subject = project.Name;
                                string mess = "โครการ" + project.Name + "ได้มีสถานะเป็นเสร็จสมบูรณ์";
                                string sender = se.User_Email;
                                Inbox.SendEmail(receiver, subject, mess, sender);
                            }
                        }
                    }
                    else if (project.SIT_Menu == 1)
                    {
                        var sit = db.SITs.Where(m => m.Project_ID == project.ProjectID).ToList();
                        double count = 0;
                        foreach (var i in sit)
                        {
                            if (i.Status == 3)
                            {
                                count = count + 1;
                                if (count == sit.Count() && project.SendDate <= DateTime.Now)
                                {
                                    project.Status = 4;
                                    db.SaveChanges();
                                    var PM = db.Users.Where(m => m.User_ID == project.CreateBy).FirstOrDefault();
                                    PM.Amount_Succ = PM.Amount_Succ + 1;
                                    db.SaveChanges();
                                    InboxController Inbox = new InboxController();
                                    var member = db.ProjectMembers.Where(m => m.ProjectID == project.ProjectID).ToList();
                                    foreach (var c in member)
                                    {
                                        var re = db.Users.Where(m => m.User_ID == c.UserID).FirstOrDefault();
                                        var se = db.Users.Where(m => m.User_ID == project.CreateBy).FirstOrDefault();
                                        string receiver = re.User_Email;
                                        string subject = project.Name;
                                        string mess = "โครการ" + project.Name + "ได้มีสถานะเป็นเสร็จสมบูรณ์";
                                        string sender = se.User_Email;
                                        Inbox.SendEmail(receiver, subject, mess, sender);
                                    }
                                }
                                else if (count == sit.Count() && project.SendDate > DateTime.Now)
                                {
                                    project.Status = 4;
                                    db.SaveChanges();
                                    var PM = db.Users.Where(m => m.User_ID == project.CreateBy).FirstOrDefault();
                                    PM.Amount_Non = PM.Amount_Non + 1;
                                    db.SaveChanges();
                                    InboxController Inbox = new InboxController();
                                    var member = db.ProjectMembers.Where(m => m.ProjectID == project.ProjectID).ToList();
                                    foreach (var c in member)
                                    {
                                        var re = db.Users.Where(m => m.User_ID == c.UserID).FirstOrDefault();
                                        var se = db.Users.Where(m => m.User_ID == project.CreateBy).FirstOrDefault();
                                        string receiver = re.User_Email;
                                        string subject = project.Name;
                                        string mess = "โครการ" + project.Name + "ได้มีสถานะเป็นเสร็จสมบูรณ์";
                                        string sender = se.User_Email;
                                        Inbox.SendEmail(receiver, subject, mess, sender);
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return View();
        }
    }
}