﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Models;
using SystemManage.Database;

namespace SystemManage.Controllers
{
    public class MemberController : Controller
    {
        Entities db = new Entities();
        // GET: Member
        public ActionResult ListMember()
        {

            var projectID = Convert.ToInt32(Session["ProjectID"]);
            List<UserModel> UserList = new List<UserModel>();
            var user = db.Users.OrderByDescending(m => m.User_ID).ToList();
                foreach (var d in user)
                {
                double SuccPercent = 0;
                if (d.Amount_Succ != 0)
                {
                    var totalProject = db.Projects.Where(m => m.CreateBy == d.User_ID).ToList();
                    var totalSubDev = db.SubTasks.Where(m => m.SubDevID == d.User_ID).ToList();
                    var totalSubTester = db.Tasks.Where(m => m.TestID == d.User_ID).ToList();
                    var totalSubQA = db.Tasks.Where(m => m.QAID == d.User_ID).ToList();
                    int totalWork = totalSubDev.Count + totalSubTester.Count + totalSubQA.Count+ totalProject.Count;
                    SuccPercent = Convert.ToDouble(d.Amount_Succ) / totalWork;
                }
                var PositionDB = db.Positions.Where(m => m.Position_ID == d.Position_ID).FirstOrDefault();
                     UserList.Add(new UserModel
                     {
                         Users_ID = d.User_ID,
                         User_Email = d.User_Email,
                         User_Name = d.User_Name,
                         User_LastName = d.User_LastName,
                         PositionName = PositionDB.Name,
                         TotalCoding = d.TotalCoding,
                         Amount_Succ = SuccPercent,
                         AVG = d.AVG //ความยากของงาน
                     });
                         ViewBag.DataList = UserList;
                }
                return View();
        }
        public ActionResult AddMember(int UserID)
        {
            ProjectMember pm = new ProjectMember();
            pm.UserID = UserID;
            pm.ProjectID = Convert.ToInt32(Session["ProjectID"]);
            var PoList = db.Users.Where(m => m.User_ID == UserID).FirstOrDefault();
            if (PoList.Position_ID == 113)//PositionID = 113 Dev 
            {
                pm.Role = 2;
                pm.Member_name = PoList.User_Name;
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                return RedirectToAction("ListMember");
            }
            else if (PoList.Position_ID == 110)//PositionID = 110 Tester
            {
                pm.Role = 3;
                pm.Member_name = PoList.User_Name;
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                return RedirectToAction("ListMember");
            }
            else if (PoList.Position_ID == 142)//PositionID = 142 QA 
            {
                pm.Role = 4;
                pm.Member_name = PoList.User_Name;
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                return RedirectToAction("ListMember");
            }
            else if (PoList.Position_ID == 143)//PositionID = 143 ลูกค้า 
            {
                pm.Role = 5;
                pm.Member_name = PoList.User_Name;
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                return RedirectToAction("ListMember");
            }else //ถ้าไม่เข้าเงื่อไขให้เป็น Dev ทั้งหมด
            pm.Role = 2; 
            db.ProjectMembers.Add(pm);
            db.SaveChanges();
            ///////////////////////////////////////
            var Cb = db.Projects.Where(m => m.ProjectID == pm.ProjectID).FirstOrDefault();
            var Email = db.Users.Where(m => m.User_ID == Cb.CreateBy).FirstOrDefault();
            var Sendto = db.Users.Where(m => m.User_ID == UserID).FirstOrDefault();
            string sender = Email.User_Email.ToString();
            //string sender = "systemmanage59346@gmail.com";
            string subject = Cb.Name;
            string receiver = Sendto.User_Email.ToString();
            //string receiver = "pattanapon2538@outlook.com";
            string mess = Sendto.User_Name+"ได้รับสิทธ์ในการเข้าร่วมโครงการ"+Cb.Name;
            InboxController i = new InboxController();
            i.SendEmail(receiver, subject, mess, sender);
            return RedirectToAction("ListMember");
        }
        public ActionResult DeleteMember(int UserID)
        {
            var pm = db.ProjectMembers.Where(m => m.UserID == UserID).FirstOrDefault();
            db.ProjectMembers.Remove(pm);
            db.SaveChanges();
            var Cb = db.Projects.Where(m => m.ProjectID == pm.ProjectID).FirstOrDefault();
            var Email = db.Users.Where(m => m.User_ID == Cb.CreateBy).FirstOrDefault();
            var Sendto = db.Users.Where(m => m.User_ID == UserID).FirstOrDefault();
            string sender = Email.User_Email.ToString();
            //string sender = "systemmanage59346@gmail.com";
            string subject = Cb.Name;
            string receiver = Sendto.User_Email.ToString();
            //string receiver = "pattanapon2538@outlook.com";
            string mess = Sendto.User_Name + "คุณได้ออกจากโครงการ" + Cb.Name;
            InboxController i = new InboxController();
            i.SendEmail(receiver, subject, mess, sender);
            return RedirectToAction("ShowMember");
        }
        public ActionResult ShowMember()
        {
            List<UserModel> UserList = new List<UserModel>();
            int DataProjectID = Convert.ToInt32(Session["ProjectID"]);
            UserModel model = new UserModel();
            var item = db.ProjectMembers.Where(m => m.ProjectID == DataProjectID).ToList();
            var c = db.Projects.Where(m => m.ProjectID == DataProjectID).FirstOrDefault();
            foreach (var i in item)
            {
                double SuccPercent = 0;
                var item2 = db.Users.Where(m => m.User_ID == i.UserID).FirstOrDefault();
                if (item2.Amount_Succ != 0)
                {
                    var totalProject = db.Projects.Where(m => m.CreateBy == i.UserID).ToList();
                    var totalSubDev = db.SubTasks.Where(m => m.SubDevID == i.UserID).ToList();
                    var totalSubTester = db.Tasks.Where(m => m.TestID == i.UserID).ToList();
                    var totalSubQA = db.Tasks.Where(m => m.QAID == i.UserID).ToList();
                    int totalWork = totalSubDev.Count + totalSubTester.Count + totalSubQA.Count + totalProject.Count;
                    SuccPercent = Convert.ToDouble(item2.Amount_Succ) / totalWork;
                }
                var dbPosition = db.Positions.Where(m => m.Position_ID == item2.Position_ID).FirstOrDefault();
                UserList.Add(new UserModel
                {
                    Users_ID = item2.User_ID,
                    User_Email = item2.User_Email,
                    User_Name = item2.User_Name,
                    User_LastName = item2.User_LastName,
                    PositionName = dbPosition.Name,
                    TotalCoding = item2.TotalCoding,
                    Amount_Succ = SuccPercent,
                    AVG = item2.AVG,
                });
            }
            model.ProjectCreateBy = c.CreateBy;
            ViewBag.DataList = UserList;
            return View(model);
        }
        public ActionResult HistoryUser(int userID)
        {
            UserModel model = new UserModel();
            List<UserModel> TaskList = new List<UserModel>();
            var u = db.Users.Where(m => m.User_ID == userID).FirstOrDefault();
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            var PM = db.ProjectMembers.Where(m => m.UserID == userID && m.ProjectID == projectID).FirstOrDefault();
            model.Users_ID = u.User_ID;
            model.User_Name = u.User_Name;
            model.User_LastName = u.User_LastName;
            model.User_Email = u.User_Email;
            model.Phone = u.Phone;
            model.Contract_ID = u.Contract_ID;
            model.Position_ID = u.Position_ID;
            model.Date_of_Started = u.Date_of_Started;
            model.Date_of_Ended = u.Date_of_Ended;
            model.Role = PM.Role;
            model.Comment = u.comment;
            model.Amount_Succ = u.Amount_Succ;
            model.Amount_Non = u.Amount_Non;
            var Work = db.ProjectMembers.Where(m => m.UserID == userID).ToList();
            foreach (var item in Work)
            {
                if (item.Role == 1)
                {
                        var TaskName = db.Projects.Where(m => m.ProjectID == item.ProjectID).FirstOrDefault();
                        double total = 0;
                        var Level = db.Tasks.Where(m => m.ProjectID == item.ProjectID).ToList();
                        foreach (var count in Level)
                        {
                            total = Convert.ToDouble(count.Task_level) + total;
                        }
                        total = total / Level.Count;
                        TaskList.Add(new UserModel
                        {
                            TaskName = TaskName.Name,
                            Level = total.ToString(),
                            RoundCoding = 0
                        });
                    }
                else if (item.Role == 2)
                {
                    var TaskDev = db.SubTasks.Where(m => m.SubDevID == userID).ToList();
                    foreach (var item3 in TaskDev)
                    {
                        var Task = db.Tasks.Where(m => m.TaskID == item3.TaskID).FirstOrDefault();
                        TaskList.Add(new UserModel
                        {
                            TaskName = item3.SubName,
                            Level = Task.Task_level.ToString(),
                            RoundCoding = item3.RoundCoding
                        });
                    }
                }
                else if (item.Role == 3)
                {
                    var TaskTester = db.Tasks.Where(m => m.TestID == userID).ToList();
                    foreach (var item4 in TaskTester)
                    {
                        TaskList.Add(new UserModel
                        {
                            TaskName = item4.TaskName,
                            Level = item4.Task_level.ToString(),
                            RoundCoding = 0
                        });
                    }
                }
                else if (item.Role == 4)
                {
                    var TaskQA = db.Tasks.Where(m => m.QAID == userID).ToList();
                    foreach (var item5 in TaskQA)
                    {
                        TaskList.Add(new UserModel
                        {
                            TaskName = item5.TaskName,
                            Level = item5.Task_level.ToString(),
                            RoundCoding = 0
                        });
                    }
                }
            }
            ViewBag.DataList = TaskList;
            return View();
        }
    }
}