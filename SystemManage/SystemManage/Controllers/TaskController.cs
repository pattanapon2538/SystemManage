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
            t.TotalPercent = 0;
            t.Task_level = model.Task_level;
            t.DescriptionTest = model.DescriptionTest;
            t.TestID = 1; //session[UserId]
            t.TestSentDate = model.TestSentDate;
            t.TestStatus = model.TestStatus.ToString();
            t.DescriptionQA = model.DescriptionQA;
            t.QAID = 1; //session[UserID]
            t.QASentDate = model.QASentDate;
            t.QAStatus = model.QAStatus;
            t.AttachFile = model.AttachFile;
            t.AttachShow = model.AttachShow;
            t.CreateDate = DateTime.Now;
            t.CreateBy = 1;//Session[UserID]
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
                    st.SubDevID = 11;//Session[UserID]
                    st.SubPercent = 0;
                    st.SubStatus = 1;
                    st.SubDevSend = DateTime.Now;
                    st.CreateDate = DateTime.Now;
                    db.SubTasks.Add(st);
                    db.SaveChanges();
                }
                ModelState.Clear();
                return View();
            }
            st.TaskID = t.TaskID;
            st.SubName = model.SubTasksName[0].ToString();
            st.SubDescriptionDev = model.SubTasksDis[0].ToString();
            st.SubDevID = 11;//Session[UserID]
            st.SubPercent = 0;
            st.SubStatus = 1;
            st.SubDevSend = DateTime.Now;
            st.CreateDate = DateTime.Now;
            db.SubTasks.Add(st);
            db.SaveChanges();
            ModelState.Clear();
            return View();
        }
        public ActionResult ShowTask(String ProjectID)
        {
            ProjectModel Model = new ProjectModel();
            Project p = db.Projects.Where(m => m.ProjectID.ToString() == ProjectID).FirstOrDefault();
            Session["ProjectID"] = p.ProjectID; //ปิดเพื่อ Test 
            //Session["Member"] = p.ProjectID;
            List<SubTaskModel> SubTaskList = new List<SubTaskModel>();
            List<TaskModel> TaskList = new List<TaskModel>();
            var item = db.Tasks.Where(m => m.ProjectID.ToString() == ProjectID).ToList();
            string Taskname = null;
            foreach (var i in item)
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
                foreach(var s in item2)
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
            ViewBag.DataList2 = SubTaskList;
            ViewBag.DataList = TaskList;
            return View();
        }
        public ActionResult DetailTask(String TaskID)
        {
            TaskModel model = new TaskModel();
            var t = db.Tasks.Where(m => m.TaskID.ToString() == TaskID).FirstOrDefault();
            var st = db.SubTasks.Where(m => m.TaskID.ToString() == TaskID).FirstOrDefault();
            model.TaskID = t.TaskID;
            model.TaskName = t.TaskName;
            model.Task_level = t.Task_level;
            model.TotalPercent = t.TotalPercent;
            model.SubDevID = st.SubDevID;
            model.SubTasksDes = st.SubDescriptionDev;
            model.SubTaskNames = st.SubName;
            model.QAID = t.QAID.ToString();
            model.TestID = t.TestID.ToString();
            model.DescriptionQA = t.DescriptionQA;
            return View(model);
        }
        public ActionResult EditTask(int TaskID)
        {
            TaskModel model = new TaskModel();
            var t = db.Tasks.Where(m => m.TaskID == TaskID).FirstOrDefault();
            t.TaskID = model.TaskID;
            t.TaskName = model.TaskName;
            t.Task_level = model.Task_level;
            t.TotalPercent = model.TotalPercent;
            t.QAID = Convert.ToInt32(model.QAID);
            t.TestID = Convert.ToInt32(model.TestID);
            db.SaveChanges();
            return RedirectToAction("ShowTask");
        }
        public ActionResult DeleteTask(int TaskID)
        {
            var t = db.Tasks.Where(m => m.TaskID == TaskID).FirstOrDefault();
            var st = db.SubTasks.Where(m => m.TaskID == TaskID).FirstOrDefault();
            db.Tasks.Remove(t);
            db.SubTasks.Remove(st);
            db.SaveChanges();
            return RedirectToAction("ShowTask");
        }
    }
}