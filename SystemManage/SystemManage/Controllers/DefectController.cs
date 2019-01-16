using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;

namespace SystemManage.Controllers
{
    public class DefectController : Controller
    {
        Entities db = new Entities();
        // GET: Defect
        public ActionResult AddDefect()
        {
            int SubID = Convert.ToInt32(Session["SubID"]);
            DefectModel model = new DefectModel();
            var item = db.SubTasks.Where(m => m.SubID == SubID).FirstOrDefault();
            var item2 = db.Tasks.Where(m => m.TaskID == item.TaskID).FirstOrDefault();
            var item3 = db.Users.Where(m => m.User_ID == item.SubDevID).FirstOrDefault();
            model.Sub_ID = SubID;
            model.SubTaskName = item.SubName;
            model.TaskName = item2.TaskName;
            model.DevName = item3.User_Email;
            return View(model);
        }
        [HttpPost]
        public ActionResult AddDefect(DefectModel model)
        {
            Defect d = new Defect();
            SubTask st = new SubTask();
            int SubID = Convert.ToInt32(Session["SubID"]);
            var count = model.DetailList.Count;
            if (count > 1)
            {
                for (int i = 0; i < count; ++i)
                {
                    d.Sub_ID = model.Sub_ID;
                    d.Detail = model.DetailList[i].ToString();
                    d.SendDate = model.SendDateList[i];
                    d.Status = 0;
                    d.AttachFile = "0";
                    d.AttachShow = "0";
                    d.CreateDate = DateTime.Now;
                    d.CreateBy = Convert.ToInt32(Session["userID"]);
                    db.Defects.Add(d);
                    db.SaveChanges();
                    var s = db.SubTasks.Where(m => m.SubID == model.Sub_ID).FirstOrDefault();
                    s.HaveDefect = 1;
                    s.SubStatus = 5;
                    db.SaveChanges();
                }
            }
            else {
                d.Sub_ID = model.Sub_ID;
                d.Detail = model.DetailList[0].ToString();
                d.SendDate = model.SendDateList[0];
                d.Status = 0;
                d.AttachFile = "0";
                d.AttachShow = "0";
                d.CreateDate = DateTime.Now;
                d.CreateBy = Convert.ToInt32(Session["userID"]);
                db.Defects.Add(d);
                db.SaveChanges();
                var s = db.SubTasks.Where(m => m.SubID == model.Sub_ID).FirstOrDefault();
                s.HaveDefect = 1;
                s.SubStatus = 5;
                db.SaveChanges();
            }
            return RedirectToAction("ShowDefect");
        }
        public ActionResult ShowDefect()
        {
            List<DefectModel> DefectList = new List<DefectModel>();
            var item = db.Defects.OrderByDescending(s => s.Defect_ID).ToList();

            foreach(var i in item)
            {
                var item2 = db.SubTasks.Where(m => m.SubID == i.Sub_ID).FirstOrDefault();
                var item3 = db.Tasks.Where(m => m.TaskID == item2.TaskID).FirstOrDefault();
                var Dev = db.Users.Where(m => m.User_ID == item2.SubDevID).FirstOrDefault();
                var Tester = db.Users.Where(m => m.User_ID == item3.TestID).FirstOrDefault();
                var QA = db.Users.Where(m => m.User_ID == item3.QAID).FirstOrDefault();
                
                DefectList.Add(new DefectModel
                {
                    TaskName = item3.TaskName,
                    Defect_ID = i.Defect_ID,
                    SubTaskName = item2.SubName,
                    Detail = i.Detail,
                    Status = i.Status,
                    DevName = Dev.User_Email,
                    TestName = Tester.User_Email,
                    QAName = QA.User_Email,
                    CreateBy = i.CreateBy
                });
            }
            ViewBag.DataList = DefectList;
            return View();
        }
        public ActionResult DetailDefect(int DefectID)
        {
            DefectModel model = new DefectModel();
            var item = db.Defects.Where(m => m.Defect_ID == DefectID).FirstOrDefault();
            var SubTask = db.SubTasks.Where(m => m.SubID == item.Sub_ID).FirstOrDefault();
            var Task = db.Tasks.Where(m => m.TaskID == SubTask.TaskID).FirstOrDefault();
            var Dev = db.Users.Where(m => m.User_ID == SubTask.SubDevID).FirstOrDefault();
            model.Defect_ID = item.Defect_ID;
            model.TaskName = Task.TaskName;
            model.SubTaskName = SubTask.SubName;
            model.Detail = item.Detail;
            model.DevID = SubTask.SubDevID;
            model.DevName = Dev.User_Email;
            model.Status = item.Status;
            model.SendDate = item.SendDate;
            model.CreateBy = item.CreateBy;
            if (item.Status == 0)
            {
                model.StatusDev = DefectModel.StatusDefectDev.Coding;
            }
            else if (item.Status == 1)
            {
                model.StatusDev = DefectModel.StatusDefectDev.FIXD;
            }
            else if (item.Status == 2)
            {
                model.StatusTest = DefectModel.StatusDefectTest.Close;
            }
            else if (item.Status == 3)
            {
                model.StatusTest = DefectModel.StatusDefectTest.ReCoding;
            }
            return View(model);
        }
        public ActionResult SeveData(DefectModel model)
        {
            var i = db.Defects.Where(m => m.Defect_ID == model.Defect_ID).FirstOrDefault();
            i.Detail = model.Detail;
            i.SendDate = model.SendDate;
            i.UpdateDate = DateTime.Now;
            if (model.StatusDev.ToString() == "Coding" && model.CreateBy != Convert.ToInt32(Session["userID"]))
            {
                i.Status = 0;
            }
            else if (model.StatusDev.ToString() == "FIXD" && model.CreateBy != Convert.ToInt32(Session["userID"]))
            {
                i.Status = 1;
            }
            else if (model.StatusTest.ToString() == "Close" && model.CreateBy == Convert.ToInt32(Session["userID"]))
            {
                i.Status = 2;
            }
            else if (model.StatusTest.ToString() == "Recoding" && model.CreateBy == Convert.ToInt32(Session["userID"]))
            {
                i.Status = 3;
            }
            //i.AttachFile =
            //i.AttachShow
            i.UpdateBy = Convert.ToInt32(Session["userID"]);
            db.SaveChanges();
            return RedirectToAction("ShowDefect", "Defect");
        }
        public ActionResult DeleteDefect(int DefectID)
        {
            var i = db.Defects.Where(m => m.Defect_ID == DefectID).FirstOrDefault();
            db.Defects.Remove(i);
            db.SaveChanges();
            return RedirectToAction("ShowDefect","Defect");
        }
        
    }
}