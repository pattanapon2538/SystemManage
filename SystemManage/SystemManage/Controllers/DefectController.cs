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
            if (Convert.ToInt32(Session["Defect_SIT"]) == 1)
            {
                DefectModel model = new DefectModel();
                int SIT_ID = Convert.ToInt32(Session["SIT_ID"]);
                var SIT = db.SITs.Where(m => m.SIT_ID == SIT_ID).FirstOrDefault();
                var user = db.Users.Where(m => m.User_ID == SIT.Dev_ID).FirstOrDefault();
                model.TaskName = SIT.Name;
                model.DevName = user.User_Name;
                return View(model);
            }
            else
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
        }
        [HttpPost]
        public ActionResult AddDefect(DefectModel model)
        {
            if (model.DetailList[0] != "")
            {
                int projectID = Convert.ToInt32(Session["ProjectID"]);
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
                        d.Project_ID = projectID;
                        db.Defects.Add(d);
                        db.SaveChanges();
                        var s = db.SubTasks.Where(m => m.SubID == model.Sub_ID).FirstOrDefault();
                        s.HaveDefect = 1;
                        s.SubStatus = 5;
                        s.SubPercent = 25;
                        db.SaveChanges();
                        var Email = db.Users.Where(m => m.User_ID == d.CreateBy).FirstOrDefault();
                        var Sendto = db.Users.Where(m => m.User_ID == s.SubDevID).FirstOrDefault();
                        string sender = Email.User_Email.ToString();
                        //string sender = "systemmanage59346@gmail.com";
                        string subject = "Defect หัวข้อ" + st.SubName;
                        string receiver = Sendto.User_Email.ToString();
                        //string receiver = "pattanapon2538@outlook.com";
                        string mess = "มีข้อผิดพลาดในหัวข้อ" + st.SubName;
                        InboxController ms = new InboxController();
                        ms.SendEmail(receiver, subject, mess, sender);
                    }
                }
                else
                {
                    d.Sub_ID = model.Sub_ID;
                    d.Detail = model.DetailList[0].ToString();
                    d.SendDate = model.SendDateList[0];
                    d.Status = 0;
                    d.AttachFile = "0";
                    d.AttachShow = "0";
                    d.CreateDate = DateTime.Now;
                    d.CreateBy = Convert.ToInt32(Session["userID"]);
                    d.Project_ID = projectID;
                    db.Defects.Add(d);
                    db.SaveChanges();
                    var s = db.SubTasks.Where(m => m.SubID == model.Sub_ID).FirstOrDefault();
                    s.HaveDefect = 1;
                    s.SubStatus = 5;
                    db.SaveChanges();
                    var Email = db.Users.Where(m => m.User_ID == d.CreateBy).FirstOrDefault();
                    var Sendto = db.Users.Where(m => m.User_ID == s.SubDevID).FirstOrDefault();
                    string sender = Email.User_Email.ToString();
                    //string sender = "systemmanage59346@gmail.com";
                    string subject = "Defect หัวข้อ" + st.SubName;
                    string receiver = Sendto.User_Email.ToString();
                    //string receiver = "pattanapon2538@outlook.com";
                    string mess = "มีข้อผิดพลาดในหัวข้อ" + st.SubName;
                    InboxController ms = new InboxController();
                    ms.SendEmail(receiver, subject, mess, sender);
                }
                return RedirectToAction("ShowDefect");
            }
            return View(new DefectModel {
            Error_Detail = "กรุณาใส่ข้อมูลให้ครบถ้วน"});
        }
        public ActionResult ShowDefect()
        {
            Session["SIT_Defect"] = 0;
            List<DefectModel> DefectList = new List<DefectModel>();
            DefectModel model = new DefectModel();
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            var item = db.Defects.Where(m => m.Project_ID == projectID).OrderByDescending(s => s.Defect_ID).ToList();
            foreach (var i in item)
            {
                model.CreateBy = i.CreateBy;
                var item2 = db.SubTasks.Where(m => m.SubID == i.Sub_ID).FirstOrDefault();
                var item3 = db.Tasks.Where(m => m.TaskID == item2.TaskID).FirstOrDefault();
                var Dev = db.Users.Where(m => m.User_ID == item2.SubDevID).FirstOrDefault();
                var Tester = db.Users.Where(m => m.User_ID == item3.TestID).FirstOrDefault();
                var QA = db.Users.Where(m => m.User_ID == item3.QAID).FirstOrDefault();
                //////////////////////////////////////////////////////////////////
                int number = 0;
                var d = db.Defects.Where(m => m.Sub_ID == i.Sub_ID).ToList();
                foreach (var c in d)
                {
                    if (c.Status == 2)
                    {
                        number = number + 1;
                        if (number == d.Count)
                        {
                            var subtask = db.SubTasks.Where(m => m.SubID == c.Sub_ID).FirstOrDefault();
                            subtask.HaveDefect = 0;
                            var Handle = db.ProjectMembers.Where(m => m.UserID == subtask.Handle).FirstOrDefault();
                            if (Handle.Role == 3)
                            {
                                subtask.SubStatus = 2;
                            }
                            else if (Handle.Role == 4)
                            {
                                subtask.SubStatus = 3;
                            }
                            else if (Handle.Role == 5)
                            {
                                subtask.SubStatus = 4;
                            }
                            db.SaveChanges();
                            number = 0;
                        }
                    }

                }

                DefectList.Add(new DefectModel
                {
                    TaskName = item3.TaskName,
                    Defect_ID = i.Defect_ID,
                    SubTaskName = item2.SubName,
                    Detail = i.Detail,
                    Status = i.Status,
                    DevName = Dev.User_Name,
                    TestName = Tester.User_Name,
                    QAName = QA.User_Name,
                    CreateBy = i.CreateBy
                });
            }
            ViewBag.DataList = DefectList;
            Session["Save_Defect"] = 0;
            return View(model);
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
            model.Comment_Dev = item.Comment_Dev;
            model.Comment_Test = item.Comment_Test;
            if (item.Status == 0)
            {
                model.StatusDev = DefectModel.StatusDefectDev.อยู่ระหว่างการตอบรับ;
            }
            else if (item.Status == 1)
            {
                model.StatusDev = DefectModel.StatusDefectDev.กำลังแก้ไข;
            }
            else if (item.Status == 2)
            {
                model.StatusDev = DefectModel.StatusDefectDev.แก้ไขแล้ว;
            }
            else if (item.Status == 3)
            {
                model.StatusTest = DefectModel.StatusDefectTest.ปิด;
            }
            else if (item.Status == 4)
            {
                model.StatusTest = DefectModel.StatusDefectTest.แก้ไชใหม่;
            }
            return View(model);
        }
        public ActionResult SaveData(DefectModel model)
        {
            var i = db.Defects.Where(m => m.Defect_ID == model.Defect_ID).FirstOrDefault();
            i.Detail = model.Detail;
            i.SendDate = model.SendDate;
            i.UpdateDate = DateTime.Now;
            i.Comment_Dev = model.Comment_Dev;
            i.Comment_Test = model.Comment_Test;
            if (model.StatusDev.ToString() == "อยู่ระหว่างการตอบรับ" && model.CreateBy != Convert.ToInt32(Session["userID"]))
            {
                i.Status = 0;
            }
            else if (model.StatusDev.ToString() == "กำลังแก้ไข" && model.CreateBy != Convert.ToInt32(Session["userID"]))
            {
                i.Status = 1;
            }
            else if (model.StatusTest.ToString() == "แก้ไขแล้ว" && model.CreateBy == Convert.ToInt32(Session["userID"]))
            {
                i.Status = 2;
            }
            else if (model.StatusTest.ToString() == "ปิด" && model.CreateBy == Convert.ToInt32(Session["userID"]))
            {
                i.Status = 3;
            }
            else if (model.StatusTest.ToString() == "แก้ไชใหม่" && model.CreateBy == Convert.ToInt32(Session["userID"]))
            {
                i.Status = 4;
            }
            //i.AttachFile =
            //i.AttachShow
            i.UpdateBy = Convert.ToInt32(Session["userID"]);
            db.SaveChanges();
            Session["Save_Defect"] = 1;
            return RedirectToAction("DetailDefect", "Defect", new { DefectID  = i.Defect_ID});
        }
        public ActionResult DeleteDefect(int DefectID)
        {
            var i = db.Defects.Where(m => m.Defect_ID == DefectID).FirstOrDefault();
            db.Defects.Remove(i);
            db.SaveChanges();
            var Email = db.Users.Where(m => m.User_ID == i.CreateBy).FirstOrDefault();
            var u = db.SubTasks.Where(m => m.SubID == i.Sub_ID).FirstOrDefault();
            var Sendto = db.Users.Where(m => m.User_ID == u.SubDevID).FirstOrDefault();
            string sender = Email.User_Email.ToString();
            //string sender = "systemmanage59346@gmail.com";
            string subject = "Defect" + u.SubName;
            string receiver = Sendto.User_Email.ToString();
            //string receiver = "pattanapon2538@outlook.com";
            string mess = "ได้ลบหัวข้อ" + u.SubName;
            InboxController ms = new InboxController();
            ms.SendEmail(receiver, subject, mess, sender);
            return RedirectToAction("ShowDefect", "Defect");
        }
        public ActionResult Defect_Dev(int SubID)
        {
            DefectModel model = new DefectModel();
            List<DefectModel> DefectList = new List<DefectModel>();
            var Defect = db.Defects.Where(m => m.Sub_ID == SubID).OrderBy(m => m.Defect_ID).ToList();
            foreach (var item in Defect)
            {
                var Dev = db.SubTasks.Where(m => m.SubID == item.Sub_ID).FirstOrDefault();
                var Task = db.Tasks.Where(m => m.TaskID == Dev.TaskID).FirstOrDefault();
                var User = db.Users.Where(m => m.User_ID == Dev.SubDevID).FirstOrDefault();
                model.DevID = Dev.SubDevID;
                model.TaskName = Task.TaskName;
                model.SubTaskName = Dev.SubName;
                model.DevName = User.User_Name;
                if (item.Status == 0)
                {
                    DefectList.Add(new DefectModel
                    {
                        Defect_ID = item.Defect_ID,
                        Detail = item.Detail,
                        Status = item.Status,
                    });
                }
                else if (item.Status == 1)
                {
                    DefectList.Add(new DefectModel
                    {
                        Defect_ID = item.Defect_ID,
                        Detail = item.Detail,
                        Status = item.Status,
                    });
                }
                else if (item.Status == 2)
                {
                    DefectList.Add(new DefectModel
                    {
                        Defect_ID = item.Defect_ID,
                        Detail = item.Detail,
                        Status = item.Status,
                    });
                }
                else if (item.Status == 3)
                {
                    DefectList.Add(new DefectModel
                    {
                        Defect_ID = item.Defect_ID,
                        Detail = item.Detail,
                        Status = item.Status,
                    });
                }
            }
            ViewBag.DataList = DefectList; ;
            return View(model);
        }
        public ActionResult Send_Defect()
        {
            return RedirectToAction("ShowDefect");
        }
    }
}