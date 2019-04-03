using System;
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
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var userID = Convert.ToInt32(Session["userID"]);
            var projectID = Convert.ToInt32(Session["ProjectID"]);
            List<UserModel> UserList = new List<UserModel>();
            List<FollowModel> F_List = new List<FollowModel>();
            UserModel model = new UserModel();
            var f = db.Follows.Where(m => m.PM_ID == userID).OrderBy(m => m.Follow_ID).ToList();
            var user = db.Users.Where(m => m.Permisstion != "A" && m.Date_of_Ended >= DateTime.Now).OrderBy(m => m.User_ID).ToList();
            var PM = db.Projects.Where(m => m.ProjectID == projectID).FirstOrDefault();
            model.Follow_C = f.Count();
            model.ProjectCreateBy = PM.CreateBy;
            model.Project_Status = PM.Status;
            foreach (var u in f)
            {
                F_List.Add(new FollowModel {
                    PM_ID = u.PM_ID,
                    user_ID = u.user_ID
                });
            }
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
                var Contract = db.Type_of_Contract.Where(m => m.Contrat_ID == d.Contract_ID).FirstOrDefault();
                var skill = db.Skills.Where(m => m.User_ID == d.User_ID).ToList();
                string Languages = "";
                foreach (var item in skill)
                {
                    var language = db.Language_of_Type.Where(m => m.languageID == item.languageID).FirstOrDefault();
                    Languages = Languages + "" + language.Name;
                }
                UserList.Add(new UserModel
                {
                        NikcName = d.NickName,
                        ContactName = Contract.Contrat_Name,
                            Start = string.Format("{0:dd/MM/yyyy}", d.Date_of_Started),
                            End = string.Format("{0:dd/MM/yyyy}", d.Date_of_Ended),
                         Users_ID = d.User_ID,
                         User_Email = d.User_Email,
                         User_Name = d.User_Name,
                         User_LastName = d.User_LastName,
                         PositionName = PositionDB.Name,
                         language_string = Languages,
                         TotalCoding = d.TotalCoding,
                         Amount_Succ = SuccPercent,
                         AVG = Math.Round(d.AVG, 1) //ความยากของงาน
                });    
            }
                ViewBag.DataList = UserList;
                ViewBag.DataList2 = F_List;
                Session["set_Role"] = 0;
                return View(model);
        }
        public ActionResult AddMember(int UserID)
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ProjectMember pm = new ProjectMember();
            pm.UserID = UserID;
            pm.ProjectID = Convert.ToInt32(Session["ProjectID"]);
            var PoList = db.Users.Where(m => m.User_ID == UserID).FirstOrDefault();
            if (PoList.Position_ID == 113)//PositionID = 113 Dev 
            {
                pm.Role = 2;
                pm.Member_name = PoList.NickName;
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                //return RedirectToAction("ListMember");
            }
            else if (PoList.Position_ID == 110)//PositionID = 110 Tester
            {
                pm.Role = 3;
                pm.Member_name = PoList.NickName;
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                //return RedirectToAction("ListMember");
            }
            else if (PoList.Position_ID == 142)//PositionID = 142 QA 
            {
                pm.Role = 4;
                pm.Member_name = PoList.NickName;
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                //return RedirectToAction("ListMember");
            }
            else if (PoList.Position_ID == 143)//PositionID = 143 ลูกค้า 
            {
                pm.Role = 5;
                pm.Member_name = PoList.NickName;
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                //return RedirectToAction("ListMember");
            }
            else //ถ้าไม่เข้าเงื่อไขให้เป็น Dev ทั้งหมด
            {
                pm.Role = 2;
                pm.Member_name = PoList.NickName;
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
            }
            ///////////////////////////////////////
            var Cb = db.Projects.Where(m => m.ProjectID == pm.ProjectID).FirstOrDefault();
            var Email = db.Users.Where(m => m.User_ID == Cb.CreateBy).FirstOrDefault();
            var Sendto = db.Users.Where(m => m.User_ID == UserID).FirstOrDefault();
            var project = db.Projects.Where(m => m.ProjectID == pm.ProjectID).FirstOrDefault();
            string sender = Email.User_Email.ToString();
            //string sender = "systemmanage59346@gmail.com";
            string subject = Cb.Name;
            string receiver = Sendto.User_Email.ToString();
            string SendDate = string.Format("{0:dd/MM/yyyy}",project.SendDate);
            string CreateDate = string.Format("{0:dd/MM/yyyy}", project.CreateDate);
            //string receiver = "pattanapon2538@outlook.com";
            string mess = "<html><body><p>ถึงคุณ" + Sendto.User_Name + " " + Sendto.User_LastName + "</p><p>&nbsp;&nbsp;&nbsp;&nbsp; คุณได้รับสิทธิ์ในการเข้าถึงโครงการ "+ project.Name+" มีรายละเอียดโครงการดังนี้  "+project.Description+" กำหนดส่งวันที่ "+ SendDate + " สร้างเมื่อวันที่ "+ CreateDate + " มีหัวหน้าโครงการคือคุณ "+Email.User_Name+" "+Email.User_LastName+"</p><p></p>" + Email.User_Name + " " + Email.User_LastName + "</body></html>";
            
            InboxController i = new InboxController();
            i.SendEmail(receiver, subject, mess, sender);
            return RedirectToAction("ListMember");
        }
        public ActionResult DeleteMember(int UserID)
        {
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            var pm = db.ProjectMembers.Where(m => m.UserID == UserID && m.ProjectID == ProjectID).FirstOrDefault();
            db.ProjectMembers.Remove(pm);
            db.SaveChanges();
            var Cb = db.Projects.Where(m => m.ProjectID == pm.ProjectID).FirstOrDefault();
            var Email = db.Users.Where(m => m.User_ID == Cb.CreateBy).FirstOrDefault();
            var Sendto = db.Users.Where(m => m.User_ID == UserID).FirstOrDefault();
            var project = db.Projects.Where(m => m.ProjectID == pm.ProjectID).FirstOrDefault();
            string sender = Email.User_Email.ToString();
            //string sender = "systemmanage59346@gmail.com";
            string subject = Cb.Name;
            string receiver = Sendto.User_Email.ToString();
            string Exit_Date = string.Format("{0:dd/MM/yyyy}",DateTime.Now);
            //string receiver = "pattanapon2538@outlook.com";
            string mess = "<html><body><p>ถึงคุณ" + Sendto.User_Name + " " + Sendto.User_LastName + "</p><p>&nbsp;&nbsp;&nbsp;&nbsp; คุณถูกเชิญออกจากโครงการ " + project.Name + " ณ วันที่ "+Exit_Date+ "หากมีข้อสงสัยกรุณาติดต่อกับหัวหน้าโครงการ คุณ"+ Email.User_Name + " " + Email.User_LastName + "</p><p>" + Email.User_Name + " " + Email.User_LastName + "</p></body></html>";
            InboxController i = new InboxController();
            i.SendEmail(receiver, subject, mess, sender);
            return RedirectToAction("ShowMember","Member");
        }
        public ActionResult ShowMember()
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<UserModel> UserList = new List<UserModel>();
            int DataProjectID = Convert.ToInt32(Session["ProjectID"]);
            UserModel model = new UserModel();
            var item = db.ProjectMembers.Where(m => m.ProjectID == DataProjectID).ToList();
            var c = db.Projects.Where(m => m.ProjectID == DataProjectID).FirstOrDefault();
            foreach (var i in item)
            {
                double SuccPercent = 0;
                var item2 = db.Users.Where(m => m.User_ID == i.UserID).FirstOrDefault();
              
                    var totalProject = db.Projects.Where(m => m.CreateBy == i.UserID).ToList();
                    var totalSubDev = db.SubTasks.Where(m => m.SubDevID == i.UserID).ToList();
                    var totalSubTester = db.Tasks.Where(m => m.TestID == i.UserID).ToList();
                    var totalSubQA = db.Tasks.Where(m => m.QAID == i.UserID).ToList();
                    int totalWork = totalSubDev.Count + totalSubTester.Count + totalSubQA.Count + totalProject.Count;
                    if (totalWork == 0 && item2.Amount_Succ == 0)
                    {
                        SuccPercent = 0;
                    }
                    else
                    {
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
                    AVG =  Math.Round(item2.AVG, 1),
                });
            }
            model.ProjectCreateBy = c.CreateBy;
            model.Project_Status = c.Status;
            Session["set_Role"] = 1;
            ViewBag.DataList = UserList;
            return View(model);
        }
        public ActionResult HistoryUser(int userID)
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            UserModel model = new UserModel();
            List<UserModel> TaskList = new List<UserModel>();
            var u = db.Users.Where(m => m.User_ID == userID).FirstOrDefault();
            double SuccPercent = 0;
            double NonPercent = 0;
            var totalProject = db.Projects.Where(m => m.CreateBy == u.User_ID).ToList();
            var totalSubDev = db.SubTasks.Where(m => m.SubDevID == u.User_ID).ToList();
            var totalSubTester = db.Tasks.Where(m => m.TestID == u.User_ID).ToList();
            var totalSubQA = db.Tasks.Where(m => m.QAID == u.User_ID).ToList();
            int totalWork = totalSubDev.Count + totalSubTester.Count + totalSubQA.Count + totalProject.Count;
            if (u.Amount_Succ != 0)
            {
                SuccPercent = Convert.ToDouble(u.Amount_Succ) / totalWork;
            }
            if (u.Amount_Non != 0)
            {
                NonPercent = Convert.ToDouble(u.Amount_Non) / totalWork;
            }
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            var PM = db.ProjectMembers.Where(m => m.UserID == userID && m.ProjectID == projectID).FirstOrDefault();
            var crete = db.Projects.Where(m => m.ProjectID == projectID).FirstOrDefault();
            model.PositionList = db.Positions.ToList();
            model.ContractsList = db.Type_of_Contract.ToList();
            model.LanguageList = db.Language_of_Type.OrderBy(m => m.languageID).ToList();
            if (PM != null)
            {
                if (PM.Role == 1)
                {
                    model.Roles = UserModel._Role.ผู้จัดการโครงการ;
                }
                else if (PM.Role == 2)
                {
                    model.Roles = UserModel._Role.ผู้พัฒนาซอฟแวร์;
                }
                else if (PM.Role == 3)
                {
                    model.Roles = UserModel._Role.ผู้ทดสอบ;
                }
                else if (PM.Role == 4)
                {
                    model.Roles = UserModel._Role.ผู้ตรวจคุณภาพ;
                }
                else if (PM.Role == 5)
                {
                    model.Roles = UserModel._Role.ลูกค้า;
                }
            }
            model.Position_ID = u.Position_ID;
            model.Contract_ID = u.Contract_ID;
            model.ContractFrom = u.ContractFrom;
            model.Users_ID = u.User_ID;
            model.User_Name = u.User_Name;
            model.User_LastName = u.User_LastName;
            model.User_Email = u.User_Email;
            model.NikcName = u.NickName;
            model.Phone = u.Phone;
            model.Contract_ID = u.Contract_ID;
            model.Date_of_Started = u.Date_of_Started;
            model.Date_of_Ended = u.Date_of_Ended;
            model.Comment = u.comment;
            model.Percent_Succ = SuccPercent;
            model.Percent_Non = NonPercent;
            model.Amount_Succ = u.Amount_Succ;
            model.Amount_Non = u.Amount_Non;
            var skill = db.Skills.Where(m => m.User_ID == userID).ToList();
            foreach (var s in skill)
            {
                var lag = db.Language_of_Type.Where(m => m.languageID == s.languageID).FirstOrDefault();
                model.language_string = lag.Name +" "+ model.language_string;
            }
            model.ProjectCreateBy = crete.CreateBy;
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
                            Level = total.ToString("#.#"),
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
            return View(model);
        }
        public ActionResult Follows(string userID)
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int PM = Convert.ToInt32(Session["userID"]);
            int User = Convert.ToInt32(userID);
            var CF = db.Follows.Where(m => m.user_ID == User && m.PM_ID == PM).FirstOrDefault();
            if (CF != null)
            {
                db.Follows.Remove(CF);
                db.SaveChanges();
            }
            else
            {
                Follow f = new Follow();
                f.user_ID = Convert.ToInt32(userID);
                f.PM_ID = Convert.ToInt32(Session["userID"]);

                db.Follows.Add(f);
                db.SaveChanges();
            }
            return RedirectToAction("ListMember","Member");
            //return Json(JsonRequestBehavior.AllowGet);
        }
        public ActionResult Profile(int userID)
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            UserModel model = new UserModel();
            List<UserModel> TaskList = new List<UserModel>();
            List<LanguageOfTypeModel> Lg_List = new List<LanguageOfTypeModel>();
            var data = db.Language_of_Type.ToList();
            foreach (var l in data)
            {
                Lg_List.Add(new LanguageOfTypeModel {
                    Name = l.Name,
                    languageID = l.languageID
                });
            }
            ViewBag.DataLg = Lg_List;
            var skill = db.Skills.Where(m => m.User_ID == userID).ToList();
            var u = db.Users.Where(m => m.User_ID == userID).FirstOrDefault();
            double SuccPercent = 0;
            double NonPercent = 0;
            var totalProject = db.Projects.Where(m => m.CreateBy == u.User_ID).ToList();
            var totalSubDev = db.SubTasks.Where(m => m.SubDevID == u.User_ID).ToList();
            var totalSubTester = db.Tasks.Where(m => m.TestID == u.User_ID).ToList();
            var totalSubQA = db.Tasks.Where(m => m.QAID == u.User_ID).ToList();
            int totalWork = totalSubDev.Count + totalSubTester.Count + totalSubQA.Count + totalProject.Count;
            if (u.Amount_Succ != 0)
            {
                SuccPercent = Convert.ToDouble(u.Amount_Succ) / totalWork;
            }
            if (u.Amount_Non != 0)
            {
                NonPercent = Convert.ToDouble(u.Amount_Non) / totalWork;
            }
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            var PM = db.ProjectMembers.Where(m => m.UserID == userID && m.ProjectID == projectID).FirstOrDefault();
            model.PositionList = db.Positions.ToList();
            model.ContractsList = db.Type_of_Contract.ToList();
            if (PM != null)
            {
                if (PM.Role == 1)
                {
                    model.Roles = UserModel._Role.ผู้จัดการโครงการ;
                }
                else if (PM.Role == 2)
                {
                    model.Roles = UserModel._Role.ผู้พัฒนาซอฟแวร์;
                }
                else if (PM.Role == 3)
                {
                    model.Roles = UserModel._Role.ผู้ทดสอบ;
                }
                else if (PM.Role == 4)
                {
                    model.Roles = UserModel._Role.ผู้ตรวจคุณภาพ;
                }
                else if (PM.Role == 5)
                {
                    model.Roles = UserModel._Role.ลูกค้า;
                }
                model.open_Role = 1;
            }
            model.Position_ID = u.Position_ID;
            model.Contract_ID = u.Contract_ID;
            model.Users_ID = u.User_ID;
            model.User_Name = u.User_Name;
            model.User_LastName = u.User_LastName;
            model.NikcName = u.NickName;
            model.User_Email = u.User_Email;
            model.Phone = u.Phone;
            model.Contract_ID = u.Contract_ID;
            model.Date_of_Started = u.Date_of_Started;
            model.Date_of_Ended = u.Date_of_Ended;
            model.Comment = u.comment;
            model.Percent_Succ = SuccPercent;
            model.BirthDate = u.BirthDate;
            model.PathShow1 = u.AttachFile1;
            model.PathShow2 = u.AttachFile2;
            model.PathShow3 = u.AttachFile3;
            model.PathShow4 = u.AttachFile4;
            model.AttachShow1 = u.AttachShow1;
            model.AttachShow2 = u.AttachShow2;
            model.AttachShow3 = u.AttachShow3;
            model.AttachShow4 = u.AttachShow4;
            model.Permission = u.Permisstion;
            model.Address = u.Address;
            model.Percent_Non = NonPercent;
            model.Amount_Succ = u.Amount_Succ;
            model.Amount_Non = u.Amount_Non;
            if (u.Listening == 3)
            {
                model._Listening = UserModel.Levels.เก่ง;
            }
            else if (u.Listening == 2)
            {
                model._Listening = UserModel.Levels.ปานกลาง;
            }
            else if (u.Listening == 1)
            {
                model._Listening = UserModel.Levels.อ่อน;
            }
            /////////////////////////////////////
            if (u.Reading == 3)
            {
                model._Reading = UserModel.Levels.เก่ง;
            }
            else if (u.Reading == 2)
            {
                model._Reading = UserModel.Levels.ปานกลาง;
            }
            else if (u.Reading == 1)
            {
                model._Reading = UserModel.Levels.อ่อน;
            }
            ////////////////////////////////////
            if (u.Writng == 3)
            {
                model._Writng = UserModel.Levels.เก่ง;
            }
            else if (u.Writng == 2)
            {
                model._Writng = UserModel.Levels.ปานกลาง;
            }
            else if (u.Writng == 1)
            {
                model._Writng = UserModel.Levels.อ่อน;
            }
            ////////////////////////////////////
            if (u.Speaking == 3)
            {
                model._Speaking = UserModel.Levels.เก่ง;
            }
            else if (u.Speaking == 2)
            {
                model._Speaking = UserModel.Levels.ปานกลาง;
            }
            else if (u.Speaking == 1)
            {
                model._Speaking = UserModel.Levels.อ่อน;
            }
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
                    if (total == 0 && Level.Count == 0)
                    {
                        total = 0;
                    }
                    else
                    {
                        total = total / Level.Count;
                    }
                    TaskList.Add(new UserModel
                    {
                        TaskName = TaskName.Name,
                        Level = total.ToString("#.#"),
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
            return View(model);
        }
        public ActionResult CheckMember(string userID)
        {
            Boolean c = false;
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            int user_ID = Convert.ToInt32(userID);
            var m = db.ProjectMembers.Where(x => x.ProjectID == ProjectID && x.UserID == user_ID).ToList();
            if (m.Count == 0)
            {
                
                return RedirectToAction("AddMember", "Member", new { userID = userID });
            }
            else
            {
                return Json(new {c = true });
            }
        }
        //public ActionResult CheckDelete(string userID)
        //{
        //    Boolean c = false;
        //    int ProjectID = Convert.ToInt32(Session["ProjectID"]);
        //    int user_ID = Convert.ToInt32(userID);
        //    var m = db.Projects.Where(x => x.ProjectID == ProjectID).FirstOrDefault();
        //    if (m.CreateBy != user_ID)
        //    {
        //        return RedirectToAction("DeleteMember", "Member", new { userID = userID });
        //    }
        //    else
        //    {
        //        Session["Succ"] = false;
        //        return RedirectToAction("ShowMember","Member",new { c= true });
        //    }
        //}
        public ActionResult Edit_Profile(UserModel model)
        {
            try
            {
                Skill sk = new Skill();
                var r = db.Users.Where(m => m.User_ID == model.Users_ID).FirstOrDefault();
                r.User_ID = model.Users_ID;
                r.User_Email = model.User_Email;
                r.NickName = model.NikcName;
                r.User_Password = model.User_Password;
                r.Phone = model.Phone;
                if (model._Reading == UserModel.Levels.เก่ง)
                {
                    r.Reading = 3;
                }
                else if (model._Reading == UserModel.Levels.ปานกลาง)
                {
                    r.Reading = 2;
                }
                else if (model._Reading == UserModel.Levels.อ่อน)
                {
                    r.Reading = 1;
                }


                if (model._Writng == UserModel.Levels.เก่ง)
                {
                    r.Writng = 3;
                }
                else if (model._Writng == UserModel.Levels.ปานกลาง)
                {
                    r.Writng = 2;
                }
                else if (model._Writng == UserModel.Levels.อ่อน)
                {
                    r.Writng = 1;
                }

                if (model._Speaking == UserModel.Levels.เก่ง)
                {
                    r.Speaking = 3;
                }
                else if (model._Speaking == UserModel.Levels.ปานกลาง)
                {
                    r.Speaking = 2;
                }
                else if (model._Speaking == UserModel.Levels.อ่อน)
                {
                    r.Speaking = 1;
                }

                if (model._Listening == UserModel.Levels.เก่ง)
                {
                    r.Listening = 3;
                }
                else if (model._Listening == UserModel.Levels.ปานกลาง)
                {
                    r.Listening = 2;
                }
                else if (model._Listening == UserModel.Levels.อ่อน)
                {
                    r.Listening = 1;
                }
                r.UpdateBy = Convert.ToInt32(Session["userID"]);
                r.UpdateDate = DateTime.Now;
                db.SaveChanges();
                var skill = db.Skills.Where(m => m.User_ID == model.Users_ID).OrderBy(m => m.SkillsID).ToList();
                string[] txt = model.language_string.Split(",".ToCharArray());
                int item_skill = txt.Count() - 1;
                if (skill != null)
                {
                    if (skill.Count == item_skill)
                    {
                        int i = 0;
                        foreach (var s in skill)
                        {
                            s.languageID = Convert.ToInt32(txt[i]);
                            s.User_ID = model.Users_ID;
                            db.SaveChanges();
                            i++;
                        }
                    }
                    else
                    {
                        foreach (var s in skill)
                        {
                            db.Skills.Remove(s);
                            db.SaveChanges();
                        }
                        for (int i = 0; i < txt.Count() - 1; i++)
                        {
                            sk.languageID = Convert.ToInt32(txt[i]);
                            sk.User_ID = model.Users_ID;
                            db.Skills.Add(sk);
                            db.SaveChanges();
                        }
                    }
                }
                ModelState.Clear();
                return RedirectToAction("Profile", "Member", new { userID = model.Users_ID });
            }
            catch (Exception)
            {
                return RedirectToAction("Profile", "Member", new { userID = model.Users_ID });
            }
        }
        public ActionResult Edit_Member(UserModel model)
        {
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            var user = db.Users.Where(m => m.User_ID == model.Users_ID).FirstOrDefault();
            var ProjectMember = db.ProjectMembers.Where(m => m.UserID == model.Users_ID && m.ProjectID == projectID).FirstOrDefault();
            if (Convert.ToInt32(Session["userID"]) == model.Users_ID)
            {
                user.User_Email = model.User_Email;
                user.Phone = model.Phone;
                user.comment = model.Comment;
                user.NickName = model.NikcName;
                db.SaveChanges();
            }
            else
            {
                if (model.Roles == UserModel._Role.ผู้จัดการโครงการ)
                {
                    ProjectMember.Role = 1;
                }
                else if (model.Roles == UserModel._Role.ผู้ตรวจคุณภาพ)
                {
                    ProjectMember.Role = 4;
                }
                else if (model.Roles == UserModel._Role.ผู้ทดสอบ)
                {
                    ProjectMember.Role = 3;
                }
                else if (model.Roles == UserModel._Role.ผู้พัฒนาซอฟแวร์)
                {
                    ProjectMember.Role = 2;
                }
                else if (model.Roles == UserModel._Role.ลูกค้า)
                {
                    ProjectMember.Role = 5;
                }
                db.SaveChanges();
            }
            return RedirectToAction("HistoryUser", "Member",new {userID = model.Users_ID });
        }
    }
}