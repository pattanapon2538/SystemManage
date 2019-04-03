using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;
using SystemManage.Controllers;
using System.IO;

namespace SystemManage.Controllers
{
    public class TaskController : Controller
    {
        Entities db = new Entities();
        // GET: Task
        public ActionResult AddTask()
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TaskModel t = new TaskModel();
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            var L = db.Tasks.Where(m => m.ProjectID == ProjectID).FirstOrDefault();
            t.DevList = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 2).ToList();
            t.TestList = db.ProjectMembers.Where(m => m.Role == 3 && m.ProjectID == ProjectID).ToList();
            t.QAList = db.ProjectMembers.Where(m => m.Role == 4 && m.ProjectID == ProjectID).ToList();
            return View(t);
        }
        [HttpPost]
        public ActionResult AddTask(TaskModel model,SubTask modelSub)
        {
            try { 
                InboxController sendMail = new InboxController();
                Task t = new Task();
                SubTask st = new SubTask();
                InboxController e = new InboxController();
                t.ProjectID = Convert.ToInt32(Session["ProjectID"]);
                t.TaskName = model.TaskName;
                t.DescriptionTask = model.DescriptionTask;
                t.TotalPercent = 0;
                if (model.level.ToString() == "ง่าย")
                {
                    t.Task_level = 1;
                }
                else if (model.level.ToString() == "ปานกลาง")
                {
                    t.Task_level = 3;
                }
                else if (model.level.ToString() == "ยาก")
                {
                    t.Task_level = 5;
                }
                t.DescriptionTest = model.DescriptionTest;
                t.TestID = model.TestID; //เลือกการค้นหาจาก Table Member ที่ Role เป็น Tester = 4 
                t.TestSentDate = model.TestSentDate;
                t.TestStatus = model.TestStatus;
                t.DescriptionQA = model.DescriptionQA;
                t.QAID = model.QAID; //เลือกการค้นหาจาก Table Member ที่ Role เป็น QA = 3
                t.QASentDate = model.QASentDate;
                t.QAStatus = model.QAStatus;
                if (model.AttachFile != null)
                {
                    var Upload = Upload_FileTask(model.AttachFile);
                    string[] txt = Upload.Split(",".ToCharArray());
                    string fileName = txt[1];
                    t.AttachFile = txt[0];
                    t.AttachShow = fileName;
                }
                t.CreateDate = DateTime.Now;
                t.CreateBy = Convert.ToInt32(Session["userID"]);
                db.Tasks.Add(t);
                db.SaveChanges();
                //var tester = db.Tasks.Where(m => m.TestID == model.TestID).ToList();
                //float total_tester = 0;
                //foreach (var item in tester)
                //{
                //    total_tester = total_tester + item.Task_level;
                //}
                //total_tester = total_tester / tester.Count;
                //var User_Tester = db.Users.Where(m => m.User_ID == model.TestID).FirstOrDefault();
                //User_Tester.AVG = total_tester;
                //db.SaveChanges();
                //var QA = db.Tasks.Where(m => m.QAID == model.QAID).ToList();
                //float total_QA = 0;
                //foreach (var item2 in QA)
                //{
                //    total_QA = total_QA + item2.Task_level;
                //}
                //total_QA = total_QA / QA.Count;
                //var User_QA = db.Users.Where(m => m.User_ID == model.QAID).FirstOrDefault();
                //User_QA.AVG = total_QA;
                //db.SaveChanges();
                /////////////////////////////////////
                var Host = db.Users.Where(m => m.User_ID == t.CreateBy).FirstOrDefault();
                var recevier = db.Users.Where(m => m.User_ID == model.TestID).FirstOrDefault();
                string senders = Host.User_Email.ToString();
                //string sender = "systemmanage59346@gmail.com";
                string subjects = t.TaskName + "" + st.SubName;
                string receivers_Tester = recevier.User_Email.ToString();
                //string receiver = "pattanapon2538@outlook.com";
                string messs_Tester = null;
                if (model.AttachFile != null)
                {
                    string Path_file = "http://localhost:8080" + t.AttachFile;
                    messs_Tester = "<html><body><p>ถึงคูณ " + receivers_Tester + ",</p><p>&nbsp;&nbsp;&nbsp;&nbsp; คุณได้รับมอบหมายงาน" + model.TaskName + "</p><p>กำหนดส่งภายในวันที่" + model.TestSentDate + "</p>เพื่อทำการทดสอบ</p> <p> </br></p></body></html>";
                }
                else
                {
                    messs_Tester = "<html><body><p>ถึงคูณ " + receivers_Tester + ",</p><p>&nbsp;&nbsp;&nbsp;&nbsp; คุณได้รับมอบหมายงาน" + model.TaskName + "</p><p>กำหนดส่งภายในวันที่" + model.TestSentDate + "</p>เพื่อทำการทดสอบ</p> <p> </br></p></body></html>";
                }
                e.SendEmail(receivers_Tester, subjects, messs_Tester, senders);
                ///////////////////////////////////////
                string receivers_QA = recevier.User_Email.ToString();
                //string receiver = "pattanapon2538@outlook.com";
                string messs_QA = "คุณได้รับงานใหม่" + t.TaskName + "และมีมีงานรองคือ" + st.SubName;
                e.SendEmail(receivers_Tester, subjects, messs_QA, senders);
                var count = model.SubTasksName.Count;
                if (count > 1)
                {
                    for (int item = 0; item < count; item++)
                    {
                        st.TaskID = t.TaskID;
                        st.SubName = model.SubTasksName[item].ToString();
                        st.SubDescriptionDev = model.SubTasksDis[item].ToString();
                        st.SubDevID = model.SubTaskDevID[item];//เลือกการค้นหาจาก Table Member ที่ Role เป็น Dev = 2
                        st.SubPercent = 0;
                        st.Handle = model.SubTaskDevID[item];
                        st.SubStatus = 0;
                        st.SubDevSend = model.SubTaskSendDate[item];
                        st.CreateDate = DateTime.Now;
                        st.CreateBy = Convert.ToInt32(Session["userID"]);
                        if (model.AttachFile_List[item] != null)
                        {
                            var Upload_Sub = Upload_FileTask(model.AttachFile_List[item]);
                            string[] txt = Upload_Sub.Split(",".ToCharArray());
                            string fileName = txt[1];
                            st.AttachFile = txt[0];
                            st.AttachShow = fileName;
                        }
                        db.SubTasks.Add(st);
                        db.SaveChanges();
                        var Sendtos = db.Users.Where(m => m.User_ID == st.SubDevID).FirstOrDefault();
                        //string sender = "systemmanage59346@gmail.com";
                        string receiversDev = Sendtos.User_Email.ToString();
                        //string receiver = "pattanapon2538@outlook.com";
                        string messs_Dev = "คุณได้รับงานใหม่" + t.TaskName + "และมีมีงานรองคือ" + st.SubName;
                        e.SendEmail(receiversDev, subjects, messs_Dev, senders);
                        //int total = 0;
                        //float AVG = 0;
                        /////Dev
                        //var vs = db.SubTasks.Where(m => m.SubDevID == st.SubDevID).ToList();
                        //foreach (var h in vs)
                        //{
                        //    var y = db.Tasks.Where(m => m.TaskID == h.TaskID).FirstOrDefault();
                        //    total = total + y.Task_level;
                        //}
                        //AVG = total / vs.Count;
                        //var us = db.Users.Where(m => m.User_ID == st.SubDevID).FirstOrDefault();
                        //us.AVG = AVG;
                        //db.SaveChanges();

                    }
                    return RedirectToAction("ShowTask", "Task");
                }
                else
                {
                    st.TaskID = t.TaskID;
                    st.SubName = model.SubTasksName[0].ToString();
                    st.SubDescriptionDev = model.SubTasksDis[0].ToString();
                    st.SubDevID = model.SubTaskDevID[0];////เลือกการค้นหาจาก Table Member ที่ Role เป็น Dev = 2
                    st.SubPercent = 0;
                    st.SubStatus = 0;
                    st.Handle = model.SubTaskDevID[0];
                    st.SubDevSend = model.SubTaskSendDate[0];
                    st.CreateDate = DateTime.Now;
                    st.CreateBy = Convert.ToInt32(Session["userID"]);
                    if (model.AttachFile_List != null)
                    {
                        if (model.AttachFile_List[0] != null)
                        {
                            var Upload_Sub = Upload_FileTask(model.AttachFile_List[0]);
                            string[] txt = Upload_Sub.Split(",".ToCharArray());
                            string fileName = txt[1];
                            st.AttachFile = txt[0];
                            st.AttachShow = fileName;
                        }
                    }
                    db.SubTasks.Add(st);
                    db.SaveChanges();
                    //var PoinCode2 = db.Users.Where(m => m.User_ID == st.SubDevID).FirstOrDefault();
                    //PoinCode2.TotalCoding = PoinCode2.TotalCoding + 1;
                    //db.SaveChanges();
                    var Email = db.Users.Where(m => m.User_ID == t.CreateBy).FirstOrDefault();
                    var Sendto = db.Users.Where(m => m.User_ID == st.SubDevID).FirstOrDefault();
                    string sender = Email.User_Email.ToString();
                    //string sender = "systemmanage59346@gmail.com";
                    string subject = t.TaskName + "" + st.SubName;
                    string receiver = Sendto.User_Email.ToString();
                    //string receiver = "pattanapon2538@outlook.com";
                    string mess = "คุณได้รับงานใหม่" + t.TaskName + "และมีมีงานรองคือ" + st.SubName;
                    InboxController i = new InboxController();
                    i.SendEmail(receiver, subject, mess, sender);
                    //int totals = 0;
                    //float AVGs = 0;
                    //var v = db.SubTasks.Where(m => m.SubDevID == st.SubDevID).ToList();
                    //foreach (var h in v)
                    //{
                    //    var y = db.Tasks.Where(m => m.TaskID == h.TaskID).FirstOrDefault();
                    //    totals = totals + y.Task_level;
                    //}
                    //AVGs = totals / v.Count;
                    //var u = db.Users.Where(m => m.User_ID == st.SubDevID).FirstOrDefault();
                    //u.AVG = AVGs;
                    //db.SaveChanges();
                    return RedirectToAction("ShowTask", "Task");
                }
                
            }
            catch (Exception)
            {
                TaskModel t = new TaskModel();
                int ProjectID = Convert.ToInt32(Session["ProjectID"]);
                var L = db.Tasks.Where(m => m.ProjectID == ProjectID).FirstOrDefault();
                t.DevList = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 2).ToList();
                t.TestList = db.ProjectMembers.Where(m => m.Role == 3 && m.ProjectID == ProjectID).ToList();
                t.QAList = db.ProjectMembers.Where(m => m.Role == 4 && m.ProjectID == ProjectID).ToList();
                return View(t);
            }
        }
        public ActionResult ShowTask()
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ProjectController P = new ProjectController();
            Session["Save"] = 0;
            Session["SIT_Defect"] = 0;
            ReportController R = new ReportController();
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            int userID = Convert.ToInt32(Session["userID"]);
            P.Check_Project(userID);
            R.Summary(ProjectID);
            Sum_AVG(ProjectID);
            string Taskname = null;
            string Handle = null;
            string DevName = null;
            TaskModel model = new TaskModel();
            List<SubTaskModel> SubTaskList = new List<SubTaskModel>();
            List<TaskModel> TaskList = new List<TaskModel>();
            var ProjectManager = db.Projects.Where(m => m.ProjectID == ProjectID).FirstOrDefault();
            var item = db.Tasks.Where(m => m.ProjectID == ProjectID).ToList();
            var r = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.UserID == userID).FirstOrDefault();
            model.CreateBy = ProjectManager.CreateBy;
            model.Project_Status = ProjectManager.Status;
            foreach (var i in item)
            {
                //PM และ CM
                if (r.Role == 1 || r.Role == 5)
                {
                    //TaskList.Add(new TaskModel
                    //{
                    //    TaskID = i.TaskID,
                    //    TaskName = i.TaskName,
                    //    TotalPercent = i.TotalPercent,
                    //    CreateDate = i.CreateDate,
                    //    UpdateDate = i.UpdateDate,
                    //    CreateBy = i.CreateBy
                    //});
                    Taskname = i.TaskName;
                    var item2 = db.SubTasks.Where(m => m.TaskID == i.TaskID).OrderByDescending(t => t.TaskID).ToList();
                    foreach (var s in item2)
                    {
                        var nameDev = db.Users.Where(m => m.User_ID == s.SubDevID).FirstOrDefault();
                        DevName = nameDev.User_Name;
                        if (s.Handle == 0)
                        {
                            Handle = "ลูกค้า";
                        }
                        else
                        {
                            var nameHandle = db.Users.Where(m => m.User_ID == s.Handle).FirstOrDefault();
                            Handle = nameHandle.User_Name;
                        }
                        //    number = number + s.SubPercent;
                        SubTaskList.Add(new SubTaskModel
                        {
                            TaskID = s.TaskID,
                            TaskName = Taskname,
                            SubID = s.SubID,
                            SubName = s.SubName,
                            Handle = Handle,
                            SubStatus = s.SubStatus,
                            SubPercent = s.SubPercent,
                            SubDescriptionDev = s.SubDescriptionDev,
                            SubDevID = DevName,
                            SubDevSend = s.SubDevSend,
                            CreateDate = s.CreateDate,
                            UpdateDate = s.UpdateDate,
                            CreateBy = s.CreateBy
                        });
                      //  ++countList;
                    }
                }
                // Dev
                else if (r.Role == 2)
                {
                    var st = db.SubTasks.Where(m => m.TaskID == i.TaskID && m.SubDevID == userID).ToList();
                    foreach (var s in st)
                    {
                        var nameDev = db.Users.Where(m => m.User_ID == s.SubDevID).FirstOrDefault();
                        DevName = nameDev.User_Name;
                        if (s.Handle == 0)
                        {
                            Handle = "ลูกค้า";
                        }
                        else
                        {
                            var nameHandle = db.Users.Where(m => m.User_ID == s.Handle).FirstOrDefault();
                            Handle = nameHandle.User_Name;
                        }
                        var t = db.Tasks.Where(m => m.TaskID == s.TaskID).FirstOrDefault();
                        TaskList.Add(new TaskModel
                        {
                            TaskID = t.TaskID,
                            TaskName = t.TaskName,
                            TotalPercent = t.TotalPercent,
                            CreateDate = t.CreateDate,
                            UpdateDate = t.UpdateDate,
                            CreateBy = t.CreateBy
                        });
                        SubTaskList.Add(new SubTaskModel
                        {
                            TaskID = s.TaskID,
                            TaskName = t.TaskName,
                            SubID = s.SubID,
                            SubName = s.SubName,
                            Handle = Handle,
                            SubStatus = s.SubStatus,
                            SubPercent = s.SubPercent,
                            SubDescriptionDev = s.SubDescriptionDev,
                            SubDevID = DevName,
                            SubDevSend = s.SubDevSend,
                            CreateDate = s.CreateDate,
                            UpdateDate = s.UpdateDate,
                            CreateBy = s.CreateBy

                        });
                    }
                }
                //Tester
                else if (r.Role == 3)
                {
                    var t = db.Tasks.Where(m => m.TaskID == i.TaskID && m.TestID == userID).ToList();
                    foreach (var it in t)
                    {
                        TaskList.Add(new TaskModel
                        {
                            TaskID = it.TaskID,
                            TaskName = it.TaskName,
                            TotalPercent = it.TotalPercent,
                            CreateDate = it.CreateDate,
                            UpdateDate = it.UpdateDate,
                            CreateBy = it.CreateBy
                        });
                        var st = db.SubTasks.Where(m => m.TaskID == it.TaskID).ToList();
                        foreach (var si in st)
                        {
                            var nameDev = db.Users.Where(m => m.User_ID == si.SubDevID).FirstOrDefault();
                            DevName = nameDev.User_Name;
                            if (si.Handle == 0)
                            {
                                Handle = "ลูกค้า";
                            }
                            else
                            {
                                var nameHandle = db.Users.Where(m => m.User_ID == si.Handle).FirstOrDefault();
                                Handle = nameHandle.User_Name;
                            }
                            SubTaskList.Add(new SubTaskModel
                            {
                                TaskID = si.TaskID,
                                TaskName = it.TaskName,
                                SubID = si.SubID,
                                SubName = si.SubName,
                                Handle = Handle,
                                SubStatus = si.SubStatus,
                                SubPercent = si.SubPercent,
                                SubDescriptionDev = si.SubDescriptionDev,
                                SubDevID = DevName,
                                SubDevSend = si.SubDevSend,
                                CreateDate = si.CreateDate,
                                UpdateDate = si.UpdateDate,
                                CreateBy = si.CreateBy
                            });
                        }
                    }
                }
                //QA
                else if (r.Role == 4)
                {
                    var t = db.Tasks.Where(m => m.TaskID == i.TaskID && m.QAID == userID).ToList();
                    foreach (var it in t)
                    {
                        TaskList.Add(new TaskModel
                        {
                            TaskID = it.TaskID,
                            TaskName = it.TaskName,
                            TotalPercent = it.TotalPercent,
                            CreateDate = it.CreateDate,
                            UpdateDate = it.UpdateDate,
                            CreateBy = it.CreateBy
                        });
                        var st = db.SubTasks.Where(m => m.TaskID == it.TaskID).ToList();
                        foreach (var si in st)
                        {
                            var nameDev = db.Users.Where(m => m.User_ID == si.SubDevID).FirstOrDefault();
                            DevName = nameDev.User_Name;
                            if (si.Handle == 0)
                            {
                                Handle = "ลูกค้า";
                            }
                            else {
                                var nameHandle = db.Users.Where(m => m.User_ID == si.Handle).FirstOrDefault();
                                Handle = nameHandle.User_Name;
                            }
                            SubTaskList.Add(new SubTaskModel
                            {
                                TaskID = si.TaskID,
                                TaskName = it.TaskName,
                                SubID = si.SubID,
                                SubName = si.SubName,
                                Handle = Handle,
                                SubStatus = si.SubStatus,
                                SubPercent = si.SubPercent,
                                SubDescriptionDev = si.SubDescriptionDev,
                                SubDevID = DevName,
                                SubDevSend = si.SubDevSend,
                                CreateDate = si.CreateDate,
                                UpdateDate = si.UpdateDate,
                                CreateBy = si.CreateBy
                            });
                        }
                    }
                }
            }
          
            ViewBag.DataList2 = SubTaskList;
            ViewBag.DataList = TaskList;
            return View(model);
        }
        public ActionResult DetailTask(int SubID)
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            Session["SubID"] = SubID;
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            int userID = Convert.ToInt32(Session["userID"]);
            TaskModel model = new TaskModel();
            var st = db.SubTasks.Where(m => m.SubID == SubID).FirstOrDefault();
            var t = db.Tasks.Where(m => m.TaskID == st.TaskID).FirstOrDefault();
            model.DevList = db.ProjectMembers.Where(m => m.ProjectID == t.ProjectID && m.Role == 2).ToList();
            model.SubTaskID = st.SubID;
            model.SubTaskNames = st.SubName;
            model.SubTasksDes = st.SubDescriptionDev;
            model.SubDevID = st.SubDevID;
            model.Handle = st.Handle;
            model.Status = st.SubStatus;
            model.HaveDefect = st.HaveDefect;
            model.SubTaskDateSend = st.SubDevSend;
            model.TestList = db.ProjectMembers.Where(m => m.ProjectID == t.ProjectID && m.Role == 3).ToList();
            model.QAList = db.ProjectMembers.Where(m => m.ProjectID == t.ProjectID && m.Role == 4).ToList();
            model.TaskID = t.TaskID;
            model.TaskName = t.TaskName;
            model.Show_Path = t.AttachFile;
            model.Show_FileName = t.AttachShow;
            model.Show_FileName_Sub = st.AttachShow;
            model.Show_Path_Sub = st.AttachFile;
            if (t.Task_level == 1)
            {
                model.level = TaskModel.LevelTask.ง่าย;
            }
            else if (t.Task_level == 2)
            {
                model.level = TaskModel.LevelTask.ปานกลาง;
            }
            else if (t.Task_level == 3)
            {
                model.level = TaskModel.LevelTask.ยาก;
            }
            model.DescriptionTask = t.DescriptionTask;
            model.TestID = t.TestID;
            model.DescriptionTest = t.DescriptionTest;
            model.TestSentDate = t.TestSentDate;
            model.QAID = t.QAID;
            model.QASentDate = t.QASentDate;
            model.DescriptionQA = t.DescriptionQA;
            model.CreateBy = t.CreateBy;
            model.Send = 0;
            model.Comment_CM = st.Comment_CM;
            model.Comment_Dev = st.Comment_Dev;
            model.Comment_Tester = st.Comment_Tester;
            model.Comment_QA = st.Comment_QA;
            return View(model);
        }
        public ActionResult EditTask(TaskModel model)
        {
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            int userID = Convert.ToInt32(Session["userID"]);
            SubTaskSubmisstion ts = new SubTaskSubmisstion();
            SubTask st = db.SubTasks.Where(m => m.SubID == model.SubTaskID).FirstOrDefault();
            Task t = db.Tasks.Where(m => m.TaskID == st.TaskID).FirstOrDefault();
            User PointCode = db.Users.Where(m => m.User_ID == userID).FirstOrDefault();
            ProjectMember r = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.UserID == userID).FirstOrDefault();
            //PM
            if (r.Role == 1)
            {
                st.SubName = model.SubTaskNames;
                st.SubDevID = model.SubDevID;
                st.SubDescriptionDev = model.SubTasksDes;
                st.SubDevSend = model.SubTaskDateSend;
                st.Handle = model.SubDevID;
                st.SubStatus = 0;
                st.SubPercent = 0;
                st.UpdateBy = Convert.ToInt32(Session["userID"]);
                st.UpdateDate = DateTime.Now;
                if (model.AttachFile_Task != null)
                {
                    var Upload_Sub = Upload_FileTask(model.AttachFile_Task);
                    string[] txt = Upload_Sub.Split(",".ToCharArray());
                    string fileName = txt[1];
                    st.AttachFile = txt[0];
                    st.AttachShow = fileName;
                }
                db.SaveChanges();
                t.TaskName = model.TaskName;
                if (model.level.ToString() == "ง่าย")
                {
                    t.Task_level = 1;
                }
                else if (model.level.ToString() == "ปานกลาง")
                {
                    t.Task_level = 2;
                }
                else if (model.level.ToString() == "ยาก")
                {
                    t.Task_level = 3;
                }
                t.DescriptionTask = model.DescriptionTask;
                t.TestID = model.TestID;
                t.DescriptionTest = model.DescriptionTest;
                t.TestSentDate = model.TestSentDate;
                t.TestStatus = 0; 
                t.QAID = model.QAID;
                t.DescriptionQA = model.DescriptionQA;
                t.QASentDate = model.QASentDate;
                t.QAStatus = 0;
                if (model.AttachFile != null)
                {
                    var Upload_Task = Upload_FileTask(model.AttachFile);
                    string[] txt_Name = Upload_Task.Split(",".ToCharArray());
                    t.AttachFile = txt_Name[0];
                    t.AttachShow = txt_Name[1];
                }
                t.UpdateDate = DateTime.Now;
                t.UpdateBy = Convert.ToInt32(Session["userID"]);
                db.SaveChanges();
                ts.SubID = st.SubID;
                ts.HandleBy = st.SubDevID;
                ts.SubmitDate = DateTime.Now;
                ts.AssignDate = st.CreateDate;
                ts.Deadline = st.SubDevSend;
                db.SubTaskSubmisstions.Add(ts);
                db.SaveChanges();
                var Email = db.Users.Where(m => m.User_ID == t.CreateBy).FirstOrDefault();
                var Sendto = db.Users.Where(m => m.User_ID == model.SubDevID).FirstOrDefault();
                string sender = Email.User_Email.ToString();
                //string sender = "systemmanage59346@gmail.com";
                string subject = t.TaskName + "" + st.SubName;
                string receiver = Sendto.User_Email.ToString();
                //string receiver = "pattanapon2538@outlook.com";
                string mess = "งานของคุณได้มีการแก้ไข" + t.TaskName + "และมีมีงานรองคือ" + st.SubName;
                InboxController i = new InboxController();
                i.SendEmail(receiver, subject, mess, sender);
                return RedirectToAction("ShowTask","Task");
            }
            //Dev
            else if (r.Role == 2)
            {
                st.Comment_Dev = model.Comment_Dev;
                db.SaveChanges();
                Session["Save"] = 1;
                return RedirectToAction("DetailTask", "Task", new { SubID = model.SubTaskID });
            }
            //Tester
            else if (r.Role == 3)
            {
                st.Comment_Tester = model.Comment_Tester;
                db.SaveChanges();
                Session["Save"] = 1;
                return RedirectToAction("DetailTask", "Task", new { SubID = model.SubTaskID });
            }
            //QA
            else if (r.Role == 4)
            {
                st.Comment_QA = model.Comment_QA;
                db.SaveChanges();
                Session["Save"] = 1;
                return RedirectToAction("DetailTask", "Task", new { SubID = model.SubTaskID });

            }
            //CM
            else if (r.Role == 5)
            {
                st.Comment_CM = model.Comment_CM;
                db.SaveChanges();
                Session["Save"] = 1;
                return RedirectToAction("DetailTask", "Task", new { SubID = model.SubTaskID });
            }
            return RedirectToAction("ShowTask");
        }
        public ActionResult DeleteTask(int SubID)
        {
            var st = db.SubTasks.Where(m => m.SubID == SubID).FirstOrDefault();
            var t = db.Tasks.Where(m => m.TaskID == st.TaskID).FirstOrDefault();
            var d = db.SubTasks.Where(m => m.TaskID == t.TaskID).ToList();
            if (st.AttachFile != null)
            {
                System.IO.File.Delete("C:/Users/SWNGMN/source/repos/SystemManage/SystemManage/SystemManage" + st.AttachFile);
            }
            db.SubTasks.Remove(st);
            db.SaveChanges();
            var d2 = db.SubTasks.Where(m => m.TaskID == t.TaskID).ToList();
            if (d2.Count == 0)
            {
                if (t.AttachFile != null)
                {
                    System.IO.File.Delete("C:/Users/SWNGMN/source/repos/SystemManage/SystemManage/SystemManage" + t.AttachFile);
                }
                db.Tasks.Remove(t);
                db.SaveChanges();
            }
            var Email = db.Users.Where(m => m.User_ID == t.CreateBy).FirstOrDefault();
            var Sendto = db.Users.Where(m => m.User_ID == st.Handle).FirstOrDefault();
            string sender = Email.User_Email.ToString();
            //string sender = "systemmanage59346@gmail.com";
            string subject = t.TaskName + "" + st.SubName;
            string receiver = Sendto.User_Email.ToString();
            //string receiver = "pattanapon2538@outlook.com";
            string mess = "ยืนยันการทำงานของงาน" + t.TaskName + "และมีมีงานรองคือ" + st.SubName;
            InboxController i = new InboxController();
            i.SendEmail(receiver, subject, mess, sender);
            return RedirectToAction("ShowTask");
        }
        public ActionResult SendTask(int SubID)
        {
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            int userID = Convert.ToInt32(Session["userID"]);
            SubTaskSubmisstion ts = new SubTaskSubmisstion();
            SubTask st = db.SubTasks.Where(m => m.SubID == SubID).FirstOrDefault();
            Task t = db.Tasks.Where(m => m.TaskID == st.TaskID).FirstOrDefault();
            User PointCode = db.Users.Where(m => m.User_ID == userID).FirstOrDefault();
            ProjectMember r = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.UserID == userID).FirstOrDefault();
            //Dev
            if (r.Role == 2)
            {
                st.SubStatus = 1;
                st.SubPercent = 25;
                st.Handle = t.TestID;
                st.UpdateDate = DateTime.Now;
                st.UpdateBy = Convert.ToInt32(Session["userID"]);
                ++st.RoundCoding;
                ++PointCode.TotalCoding;
                db.SaveChanges();
                t.UpdateDate = DateTime.Now;
                t.UpdateBy = Convert.ToInt32(Session["userID"]);
                db.SaveChanges();
                ts.SubID = st.SubID;
                ts.HandleBy = t.TestID;
                ts.SubmitDate = DateTime.Now;
                ts.AssignDate = t.CreateDate;
                ts.Deadline = t.TestSentDate;
                db.SubTaskSubmisstions.Add(ts);
                db.SaveChanges();
                var Email = db.Users.Where(m => m.User_ID == st.UpdateBy).FirstOrDefault();
                var Sendto = db.Users.Where(m => m.User_ID == t.TestID).FirstOrDefault();
                string sender = Email.User_Email.ToString();
                //string sender = "systemmanage59346@gmail.com";
                string subject = t.TaskName + "" + st.SubName;
                string receiver = Sendto.User_Email.ToString();
                //string receiver = "pattanapon2538@outlook.com";
                string mess = "ยืนยันการทำงานของงาน" + t.TaskName + "และมีมีงานรองคือ" + st.SubName;
                InboxController i = new InboxController();
                i.SendEmail(receiver, subject, mess, sender);
                if (st.SubDevSend >= st.UpdateDate)
                {
                    PointCode.Amount_Succ = PointCode.Amount_Succ + 1;
                    db.SaveChanges();
                }
                else if (st.SubDevSend < st.UpdateDate)
                {
                    PointCode.Amount_Non = PointCode.Amount_Non + 1;
                    db.SaveChanges();
                }
                Session["Save"] = 0;
                return RedirectToAction("ShowTask");
            }
            //Tester
            else if (r.Role == 3)
            {
                
                st.SubStatus = 2;
                st.SubPercent = 50;
                st.Handle = t.QAID;
                st.UpdateDate = DateTime.Now;
                st.UpdateBy = Convert.ToInt32(Session["userID"]);
                db.SaveChanges();
                t.UpdateDate = DateTime.Now;
                t.UpdateBy = Convert.ToInt32(Session["userID"]);
                db.SaveChanges();
                ts.SubID = st.SubID;
                ts.HandleBy = t.QAID;
                ts.SubmitDate = DateTime.Now;
                ts.AssignDate = t.CreateDate;
                ts.Deadline = t.QASentDate;
                db.SubTaskSubmisstions.Add(ts);
                db.SaveChanges();
                var Email = db.Users.Where(m => m.User_ID == st.UpdateBy).FirstOrDefault();
                var Sendto = db.Users.Where(m => m.User_ID == t.QAID).FirstOrDefault();
                string sender = Email.User_Email.ToString();
                //string sender = "systemmanage59346@gmail.com";
                string subject = t.TaskName + "" + st.SubName;
                string receiver = Sendto.User_Email.ToString();
                //string receiver = "pattanapon2538@outlook.com";
                string mess = "ยืนยันการทำงานของงาน" + t.TaskName + "และมีมีงานรองคือ" + st.SubName;
                InboxController i = new InboxController();
                i.SendEmail(receiver, subject, mess, sender);
                if (t.TestSentDate >= t.UpdateDate)
                {
                    PointCode.Amount_Succ = PointCode.Amount_Succ + 1;
                    db.SaveChanges();
                }
                else if (t.TestSentDate < t.UpdateDate)
                {
                    PointCode.Amount_Non = PointCode.Amount_Non + 1;
                    db.SaveChanges();
                }
                Session["Save"] = 0;
                return RedirectToAction("ShowTask");
            }
            //QA
            else if (r.Role == 4)
            {
                st.SubStatus = 3;
                st.SubPercent = 75;
                st.UpdateDate = DateTime.Now;
                st.Handle = 0; //ลูกค้า
                st.UpdateBy = Convert.ToInt32(Session["userID"]);
                db.SaveChanges();
                t.UpdateDate = DateTime.Now;
                t.UpdateBy = Convert.ToInt32(Session["userID"]);
                db.SaveChanges();
                ts.SubID = st.SubID;
                ts.HandleBy = t.QAID;
                ts.SubmitDate = DateTime.Now;
                ts.AssignDate = t.CreateDate;
                ts.Deadline = t.QASentDate;
                db.SubTaskSubmisstions.Add(ts);
                db.SaveChanges();
                var Email = db.Users.Where(m => m.User_ID == st.UpdateBy).FirstOrDefault();
                var Sendto = db.Users.Where(m => m.User_ID == t.CreateBy).FirstOrDefault();
                string sender = Email.User_Email.ToString();
                //string sender = "systemmanage59346@gmail.com";
                string subject = t.TaskName + "" + st.SubName;
                string receiver = Sendto.User_Email.ToString();
                //string receiver = "pattanapon2538@outlook.com";
                string mess = "ยืนยันการทำงานของงาน" + t.TaskName + "และมีมีงานรองคือ" + st.SubName;
                InboxController i = new InboxController();
                i.SendEmail(receiver, subject, mess, sender);
                if (t.QASentDate >= t.UpdateDate)
                {
                    PointCode.Amount_Succ = PointCode.Amount_Succ + 1;
                    db.SaveChanges();
                }
                else if (t.QASentDate < t.UpdateDate)
                {
                    PointCode.Amount_Non = PointCode.Amount_Non + 1;
                    db.SaveChanges();
                }
                Session["Save"] = 0;
                return RedirectToAction("ShowTask");
            }
            //CM
            else if (r.Role == 5)
            {
                st.SubStatus = 4;
                st.SubPercent = 100;
                st.UpdateDate = DateTime.Now;
                st.Handle = Convert.ToInt32(Session["userID"]);
                st.UpdateBy = Convert.ToInt32(Session["userID"]);
                db.SaveChanges();
                t.UpdateDate = DateTime.Now;
                t.UpdateBy = Convert.ToInt32(Session["userID"]);
                db.SaveChanges();
                ts.SubID = st.SubID;
                ts.HandleBy = Convert.ToInt32(Session["userID"]);
                ts.SubmitDate = DateTime.Now;
                ts.AssignDate = t.CreateDate;
                ts.Deadline = DateTime.Now;
                db.SubTaskSubmisstions.Add(ts);
                db.SaveChanges();
                var Email = db.Users.Where(m => m.User_ID == st.UpdateBy).FirstOrDefault();
                var Sendto = db.Users.Where(m => m.User_ID == t.CreateBy).FirstOrDefault();
                string sender = Email.User_Email.ToString();
                //string sender = "systemmanage59346@gmail.com";
                string subject = t.TaskName + "" + st.SubName;
                string receiver = Sendto.User_Email.ToString();
                //string receiver = "pattanapon2538@outlook.com";
                string mess = "ยืนยันการทำงานของงาน" + t.TaskName + "และมีมีงานรองคือ" + st.SubName;
                InboxController i = new InboxController();
                i.SendEmail(receiver, subject, mess, sender);
                Session["Save"] = 0;
                return RedirectToAction("ShowTask");
            }
            return RedirectToAction("ShowTask");
        }
        /// //////////////////
        private bool isValidContentType(string contentType)
        {
            return contentType.Equals("image/png") || contentType.Equals("image/jpg") || contentType.Equals("application/pdf") || contentType.Equals("image/jpeg");
        }

        private bool isValidContentLength(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1; // 1MB
        }
        public string Upload_FileTask(HttpPostedFileBase File)
        {
            if (!isValidContentType(File.ContentType))
            {
                ViewBag.Error = "เฉพาะไฟล์ jpg png pdf";
                return ("Error");
            }
            else if (!isValidContentLength(File.ContentLength))
            {
                ViewBag.Error = "ไฟล์มีขนาดใหญ่เกินไป (1MB)";
                return ("Error");
            }
            else
            {
                if (File.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(File.FileName);
                    var path = Path.Combine(Server.MapPath("~/Upload/") + fileName);
                    string i = "/Upload/" + fileName +","+ fileName;
                    File.SaveAs(path);
                    ViewBag.fileName = File.FileName;
                    if (File.ContentType.Equals("application/pdf"))
                    {
                        return i;
                    }
                    else

                        return i;
                }
                return ("Error");
            }
        }
        public ActionResult Sum_AVG(int Project_ID)
        {
            var Member = db.ProjectMembers.Where(m => m.ProjectID == Project_ID && m.Role != 5).ToList();
            foreach (var item in Member)
            {
                float total = 0;
                float total2 = 0;
                var PM = db.Projects.Where(m => m.CreateBy == item.UserID).ToList();
                    foreach (var i in PM)
                    {
                        var PM_Task = db.Tasks.Where(m => m.ProjectID == i.ProjectID).ToList();
                        foreach (var i2 in PM_Task)
                        {
                            total2 = total2 + i2.Task_level;
                        }
                        if (PM_Task.Count == 0)
                        {
                            total2 = 0;
                        }
                        else
                        {
                            total2 = total2 / PM_Task.Count;
                        }
                        total = total + total2;
                        total2 = 0;
                    }
                var tester = db.Tasks.Where(m => m.TestID == item.UserID).ToList();
                foreach (var item2 in tester)
                {
                    total = total + item2.Task_level;
                }
                var QA = db.Tasks.Where(m => m.QAID == item.UserID).ToList();
                foreach(var item3 in QA)
                {
                    total = total + item3.Task_level;
                }
                var Dev = db.SubTasks.Where(m => m.SubDevID == item.UserID).ToList();
                foreach(var item4 in Dev)
                {
                    var Task = db.Tasks.Where(m => m.TaskID == item4.TaskID).FirstOrDefault();
                    total = total + Task.Task_level;
                }
                if (total != 0)
                { 
                    total = total / (tester.Count + QA.Count + Dev.Count + PM.Count);
                    var User = db.Users.Where(m => m.User_ID == item.UserID).FirstOrDefault();
                    User.AVG = total;
                    db.SaveChanges();
                }
            }
            return View();
        }

    }
}