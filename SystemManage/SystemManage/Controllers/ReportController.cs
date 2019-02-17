using System;
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
            
            List<SubTaskModel> subTasksReport = new List<SubTaskModel>();
            int projectID = 2007;//Convert.ToInt32(Session["ProjectID"]);
            var t = db.Tasks.Where(m => m.ProjectID == projectID).ToList();
            var p = db.ProjectMembers.Where(m => m.ProjectID == projectID).ToList();
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
                    string Handle = u.User_Name;
                    string DevName = y.User_Name;
                    subTasksReport.Add(new SubTaskModel {
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
                        TesterName = tester.User_Name,
                        QAName = QA.User_Name,
                        SubDevSend = d.SubDevSend,
                        CreateDate = d.CreateDate,
                        UpdateDate = d.UpdateDate,
                        CreateBy = d.CreateBy,
                        TestID = c.TestID,
                        QAID = c.QAID
                });
                    foreach (var a in p) {
                        var Member_Name = a.Member_name;
                    }
                }
                Total = Total / s.Count;
                c.TotalPercent = Total;
                db.SaveChanges();
            }
            ViewBag.DataList = subTasksReport;
            return View();
        }
    }
}