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
        public ActionResult AddTask(TaskModel model,SubTask modelSub)
        {
            Task t = new Task();
            SubTask st = new SubTask();
            t.ProjectID = Convert.ToInt32(Session["ProjectID"]);
            t.TaskName = model.TaskName;
            t.DescriptionTask = model.DescriptionTask;
            t.TotalPercent = 0;
            t.Task_level = model.Task_level;
            t.DescriptionTest = model.DescriptionTest;
            t.TestID = model.TaskID; //เลือกการค้นหาจาก Table Member ที่ Role เป็น Tester = 4 
            t.TestSentDate = model.TestSentDate;
            t.TestStatus = model.TestStatus.ToString();
            t.DescriptionQA = model.DescriptionQA;
            t.QAID = model.QAID; //เลือกการค้นหาจาก Table Member ที่ Role เป็น QA = 3
            t.QASentDate = model.QASentDate;
            t.QAStatus = model.QAStatus;
            t.AttachFile = model.AttachFile;
            t.AttachShow = model.AttachShow;
            t.CreateDate = DateTime.Now;
            t.CreateBy = Convert.ToInt32(Session["userID"]);
            db.Tasks.Add(t);
            db.SaveChanges();
            var count= model.SubTasksName.Count;
            if(count > 1)
            {
                for (int item = 0; item < count; item++)
                {
                    st.TaskID = t.TaskID;
                    st.SubName = model.SubTasksName[item].ToString();
                    st.SubDescriptionDev = model.SubTasksDis[item].ToString();
                    st.SubDevID = model.SubTaskDevID[item] ;//เลือกการค้นหาจาก Table Member ที่ Role เป็น Dev = 2
                    st.SubPercent = 0;
                    st.SubStatus = 1;
                    st.SubDevSend = DateTime.Now;
                    st.CreateDate = DateTime.Now;
                    db.SubTasks.Add(st);
                    db.SaveChanges();
                }
                ModelState.Clear();
                return RedirectToAction("ShowTask");
            }
            st.TaskID = t.TaskID;
            st.SubName = model.SubTasksName[0].ToString();
            st.SubDescriptionDev = model.SubTasksDis[0].ToString();
            st.SubDevID = model.SubTaskDevID[0];////เลือกการค้นหาจาก Table Member ที่ Role เป็น Dev = 2
            st.SubPercent = 0;
            st.SubStatus = 1;
            st.SubDevSend = DateTime.Now;
            st.CreateDate = DateTime.Now;
            db.SubTasks.Add(st);
            db.SaveChanges();
            ModelState.Clear();
            return View();
        }
        public ActionResult ShowTask()
        {
            ProjectModel Model = new ProjectModel();
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            int userID = Convert.ToInt32(Session["userID"]);
            string Taskname = null;
            List<SubTaskModel> SubTaskList = new List<SubTaskModel>();
            List<TaskModel> TaskList = new List<TaskModel>();
            var item = db.Tasks.Where(m => m.ProjectID == ProjectID).ToList();
            var r = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.UserID == userID).FirstOrDefault();
            foreach (var i in item)
            {
                if (r.Role == 1)
                {
                    TaskList.Add(new TaskModel
                    {
                        TaskID = i.TaskID,
                        TaskName = i.TaskName,
                        TotalPercent = i.TotalPercent,
                        CreateDate = i.CreateDate,
                        UpdateDate = i.UpdateDate,
                    });
                    Taskname = i.TaskName;
                    var item2 = db.SubTasks.Where(m => m.TaskID == i.TaskID).OrderByDescending(t => t.TaskID).ToList();
                    foreach (var s in item2)
                    {
                        SubTaskList.Add(new SubTaskModel
                        {
                            TaskID = s.TaskID,
                            TaskName = Taskname,
                            SubID = s.SubID,
                            SubName = s.SubName,
                            SubStatus = s.SubStatus,
                            SubPercent = s.SubPercent,
                            SubDescriptionDev = s.SubDescriptionDev,
                            SubDevID = s.SubDevID.ToString(),
                            SubDevSend = s.SubDevSend,
                            CreateDate = s.CreateDate,
                            UpdateDate = s.UpdateDate,
                        });
                    }
                }
                else if (r.Role == 2)
                {
                    var st = db.SubTasks.Where(m => m.TaskID == i.TaskID && m.SubDevID == userID).ToList();
                    foreach (var s in st)
                    {
                        var t = db.Tasks.Where(m => m.TaskID == s.TaskID).FirstOrDefault();
                        TaskList.Add(new TaskModel
                        {
                            TaskID = t.TaskID,
                            TaskName = t.TaskName,
                            TotalPercent = t.TotalPercent,
                            CreateDate = t.CreateDate,
                            UpdateDate = t.UpdateDate,
                        });
                        SubTaskList.Add(new SubTaskModel
                        {
                            TaskID = s.TaskID,
                            TaskName = Taskname,
                            SubID = s.SubID,
                            SubName = s.SubName,
                            SubStatus = s.SubStatus,
                            SubPercent = s.SubPercent,
                            SubDescriptionDev = s.SubDescriptionDev,
                            SubDevID = s.SubDevID.ToString(),
                            SubDevSend = s.SubDevSend,
                            CreateDate = s.CreateDate,
                            UpdateDate = s.UpdateDate,
                            
                        });
                    }
                }
            }
            ViewBag.DataList2 = SubTaskList;
            ViewBag.DataList = TaskList;
            return View();
        }
        public ActionResult DetailTask(String SubID)
        {
            TaskModel model = new TaskModel();
            var st = db.SubTasks.Where(m => m.SubID.ToString() == SubID).FirstOrDefault();
            model.SubTaskID = st.SubID;
            model.SubTaskNames = st.SubName;
            model.SubTasksDes = st.SubDescriptionDev;
            model.SubDevID = st.SubDevID;
            model.SubTaskDateSend = st.SubDevSend;
            var t = db.Tasks.Where(m => m.TaskID == st.TaskID).FirstOrDefault();
            model.TaskID = t.TaskID;
            model.TaskName = t.TaskName;
            model.DescriptionTask = t.DescriptionTask;
            model.TestID = t.TestID;
            model.DescriptionTest = t.DescriptionTest;
            model.TestSentDate = t.TestSentDate;
            model.QAID = t.QAID;
            model.QASentDate = t.QASentDate;
            model.DescriptionQA = t.DescriptionQA;
            return View(model);
        }
        public ActionResult EditTask(TaskModel model)
        {
            var st = db.SubTasks.Where(m => m.SubID == model.SubTaskID).FirstOrDefault();
            st.SubName = model.SubTaskNames;
            st.SubDevID = model.SubDevID;
            st.SubDescriptionDev = model.SubTasksDes;
            st.SubDevSend = model.SubTaskDateSend;
            db.SaveChanges();
            var t = db.Tasks.Where(m => m.TaskID == st.TaskID).FirstOrDefault();
            t.TaskName = model.TaskName;
            t.Task_level = 1; //Set Dropdown
            t.DescriptionTask = model.DescriptionTask;
            t.TestID = model.TestID;
            t.DescriptionTest = model.DescriptionTest;
            t.TestSentDate = model.TestSentDate;
            t.TestStatus = "t"; //รอเขียนเงื่อนไข
            t.QAID = model.QAID;
            t.DescriptionQA = model.DescriptionQA;
            t.QASentDate = model.QASentDate;
            t.QAStatus = 0; //รอเขียนเงื่อนไข แก้ type ด้วย
            t.UpdateDate = DateTime.Now;
            t.UpdateBy = 0; //Session[User]
            db.SaveChanges();
            return RedirectToAction("ShowTask");
        }
        public ActionResult DeleteTask(int SubID)
        {
            var st = db.SubTasks.Where(m => m.SubID == SubID).FirstOrDefault();
            db.SubTasks.Remove(st);
            db.SaveChanges();
            return RedirectToAction("ShowTask");
        }
    }
}