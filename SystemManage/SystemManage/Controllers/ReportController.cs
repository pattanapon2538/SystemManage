﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;

namespace SystemManage.Controllers
{
    public class ReportController : Controller
    {
        Entities db = new Entities();

        // GET: Report
        public ActionResult Report()
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            string Handle,DevName,TesterName,QAName = "";
            SubTaskModel model = new SubTaskModel();
            List<SubTaskModel> subTasksReport = new List<SubTaskModel>();
            List<SubTaskModel> Team = new List<SubTaskModel>();
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            var project = db.Projects.Where(m => m.ProjectID == projectID).FirstOrDefault();
            model.ProjectDetail = project.Description;
            var t = db.Tasks.Where(m => m.ProjectID == projectID).ToList();
            foreach (var c in t)
            {
                double Total = 0;
                var s = db.SubTasks.Where(m => m.TaskID == c.TaskID).ToList();
                foreach (var d in s)
                {
                    Total = Total + d.SubPercent;
                    var u = db.Users.Where(m => m.User_ID == d.Handle).FirstOrDefault();
                    var y = db.Users.Where(m => m.User_ID == d.SubDevID).FirstOrDefault();
                    var tester = db.Users.Where(m => m.User_ID == c.TestID).FirstOrDefault();
                    var QA = db.Users.Where(m => m.User_ID == c.QAID).FirstOrDefault();
                    if (u != null)
                    {
                         Handle = u.User_Name;
                    }
                    else
                    {
                         Handle = "ไม่ผู้รับผิดชอบ";
                    }
                    if (y != null)
                    {
                        DevName = y.User_Name;
                    }
                    else
                    {
                        DevName = "ไม่มีผู้รับผิดชอบ";
                    }
                    if (tester != null)
                    {
                        TesterName = tester.User_Name;
                    }
                    else
                    {
                        TesterName = "ไม่มีผู้รับผิดชอบ";
                    }
                    if (QA != null)
                    {
                        QAName = QA.User_Name;
                    }
                    else
                    {
                        QAName = "ไม่มีผู้รับผิดชอบ";
                    }
                    subTasksReport.Add(new SubTaskModel
                    {
                        TaskID = c.TaskID,
                        TaskName = c.TaskName,
                        percent_task = c.TotalPercent,
                        SubID = d.SubID,
                        SubName = d.SubName,
                        Handle = Handle,
                        SubStatus = d.SubStatus,
                        SubPercent = d.SubPercent,
                        SubDescriptionDev = d.SubDescriptionDev,
                        SubDevID = DevName,
                        TesterName = TesterName,
                        QAName = QAName,
                        SubDevSend = d.SubDevSend,
                        CreateDate = d.CreateDate,
                        UpdateDate = d.UpdateDate,
                        CreateBy = d.CreateBy,
                    });
                }
                Total = Total / s.Count;
                c.TotalPercent = Total;
                db.SaveChanges();
            }
            string P_Name = null, D_Name = null, T_Name = null, Q_Name = null;
            var p = db.ProjectMembers.Where(m => m.ProjectID == projectID).OrderBy(m=>m.Role).ToList();
            foreach (var item in p)
            {
                if (item.Role == 1)
                {
                    var N = db.Users.Where(m => m.User_ID == item.UserID).FirstOrDefault();
                    P_Name = N.User_Name;
                }
                else if (item.Role == 2)
                {
                    var N = db.Users.Where(m => m.User_ID == item.UserID).FirstOrDefault();
                    D_Name = N.User_Name;
                }
                else if (item.Role == 3)
                {
                    var N = db.Users.Where(m => m.User_ID == item.UserID).FirstOrDefault();
                    T_Name = N.User_Name;
                }
                else if (item.Role == 4)
                {
                    var N = db.Users.Where(m => m.User_ID == item.UserID).FirstOrDefault();
                    Q_Name = N.User_Name;
                }
                Team.Add(new SubTaskModel {
                    PM = P_Name,
                    Dev = D_Name,
                    Tester = T_Name,
                    QA = Q_Name,
                    Role = item.Role
                });
            }
            ViewBag.DataList2 = Team;
            ViewBag.DataList = subTasksReport;
            return View(model);
        }
        public ActionResult Summary(int projectID)
        {
            var t = db.Tasks.Where(m => m.ProjectID == projectID).ToList();
            foreach (var c in t)
            {
                double Total = 0;
                var s = db.SubTasks.Where(m => m.TaskID == c.TaskID).ToList();
                foreach (var d in s)
                {
                    Total = Total + d.SubPercent;
                }
                if (Total != 0)
                {
                    Total = Total / s.Count;
                    c.TotalPercent = Total;
                    db.SaveChanges();
                }
                else
                {
                    c.TotalPercent = 0;
                    db.SaveChanges();
                }
            }
            return View();
        }
    }
}