using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;
using System.Threading;
using SystemManage.Controllers;
using System.IO;

namespace SystemManage.Controllers
{

    public class SITController : Controller
    {
        Entities db = new Entities();
        // GET: SIT
        public ActionResult AddSIT()
        {
            SITModel SIT = new SITModel();
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            SIT.Task = db.Tasks.Where(m => m.ProjectID == ProjectID && m.TotalPercent == 100).ToList();
            SIT.Tester = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 3).OrderBy(m => m.UserID).ToList();
            SIT.Dev = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 2).OrderBy(m => m.UserID).ToList();
            SIT.QA = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 4).OrderBy(m => m.UserID).ToList();
            return View(SIT);
        }
        [HttpPost]
        public ActionResult AddSIT(SITModel model)
        {
            try
            {
                SIT s = new SIT();
                Database.SITStep Step = new Database.SITStep();
                int ProjectID = Convert.ToInt32(Session["ProjectID"]);
                s.Project_ID = ProjectID;
                s.Name = model.Name;
                s.Detail = model.Detail;
                s.Tester_ID = model.Tester_ID;
                s.Dev_ID = model.Dev_ID;
                s.QA_ID = model.QA_ID;
                s.Send_Date_T = model.Send_Date_T;
                s.Send_Date_Q = model.Send_Date_Q;
                s.Handle = model.Tester_ID;
                if (model.AttachFile != null)
                {
                    var Upload = Upload_FileSIT(model.AttachFile);
                    string[] txt = Upload.Split(",".ToCharArray());
                    s.AttachFile = txt[0];
                    s.AttachShow = txt[1];
                }
                s.Status = 0;
                s.CreateDate = DateTime.Now;
                s.CreateBy = Convert.ToInt32(Session["userID"]);
                db.SITs.Add(s);
                db.SaveChanges();
                var c = model.TaskList.Count();
                for (int item = 0; item < c; item++)
                {
                    Step.Task_ID = model.TaskList[item];
                    Step.Step = item;
                    Step.SIT_ID = s.SIT_ID;
                    db.SITSteps.Add(Step);
                    db.SaveChanges();
                }
                return RedirectToAction("ShowSIT", "SIT");
            }
            catch {
                SITModel SIT = new SITModel();
                int ProjectID = Convert.ToInt32(Session["ProjectID"]);
                SIT.Task = db.Tasks.Where(m => m.ProjectID == ProjectID && m.TotalPercent == 100).ToList();
                SIT.Tester = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 3).OrderBy(m => m.UserID).ToList();
                SIT.Dev = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 2).OrderBy(m => m.UserID).ToList();
                SIT.QA = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 4).OrderBy(m => m.UserID).ToList();
                return View(SIT);
            }
        }
        public ActionResult ShowSIT()
        {
            Session["Defect_SIT"] = 0;
            SITModel data = new SITModel();
            List<SITModel> model = new List<SITModel>();
            ReportController R = new ReportController();
            int projectID = Convert.ToInt32(Session["ProjectID"]);
            R.Summary(projectID);
            var project = db.Projects.Where(m => m.ProjectID == projectID).FirstOrDefault();
            data.CreateBy = project.CreateBy;
            data.Project_Status = project.Status;
            var sit = db.SITs.Where(m => m.Project_ID == projectID).ToList();
            Session["Show"] = 0;
            foreach (var n in sit)
            {
                var QA = db.Users.Where(m => m.User_ID == n.QA_ID).FirstOrDefault();
                var Tester = db.Users.Where(m => m.User_ID == n.Tester_ID).FirstOrDefault();
                var Dev = db.Users.Where(m => m.User_ID == n.Dev_ID).FirstOrDefault();
                model.Add(new SITModel
                {
                    SIT_ID = n.SIT_ID,
                    Name = n.Name,
                    Detail = n.Detail,
                    Status = n.Status,
                    QA_Name = QA.User_Name,
                    Tester_Name = Tester.User_Name,
                    Dev_Name = Dev.User_Name,
                    CreateDate = n.CreateDate
                });
            }
            ViewBag.DataList = model;
            return View(data);
        }
        public ActionResult DeleteSIT(int SIT_ID)
        {
            var Step = db.SITSteps.Where(m => m.SIT_ID == SIT_ID).ToList();
            foreach (var item in Step)
            {
                db.SITSteps.Remove(item);
                db.SaveChanges();
            }
            var SIT = db.SITs.Where(m => m.SIT_ID == SIT_ID).FirstOrDefault();
            db.SITs.Remove(SIT);
            db.SaveChanges();
            return RedirectToAction("ShowSIT", "SIT");
        }
        public ActionResult DetailSIT(int SIT_ID)
        {
            int ddd = Convert.ToInt32(Session["userID"]);
            var SIT = db.SITs.Where(m => m.SIT_ID == SIT_ID).FirstOrDefault();
            SITModel model = new SITModel();
            model.SIT_ID = SIT.SIT_ID;
            model.Name = SIT.Name;
            model.Detail = SIT.Detail;
            model.Tester_ID = SIT.Tester_ID;
            model.Dev_ID = SIT.Dev_ID;
            model.Send_Date_T = SIT.Send_Date_T;
            model.Send_Date_Q = SIT.Send_Date_Q;
            model.Status = SIT.Status;
            model.Handle = SIT.Handle;
            model.Comment_CM = SIT.Comment_CM;
            model.Comment_Dev = SIT.Comment_Dev;
            model.Comment_QA = SIT.Comment_QA;
            model.Commnet_T = SIT.Commnet_Test;
            model.QA_ID = SIT.QA_ID;
            model.AttachShow = SIT.AttachShow;
            model.Show_Path = SIT.AttachFile;
            model.CreateBy = SIT.CreateBy;
            int ProjectID = Convert.ToInt32(Session["ProjectID"]);
            model.Task = db.Tasks.Where(m => m.ProjectID == ProjectID).ToList();
            model.Tester = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 3).OrderBy(m => m.UserID).ToList();
            model.Dev = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 2).OrderBy(m => m.UserID).ToList();
            model.QA = db.ProjectMembers.Where(m => m.ProjectID == ProjectID && m.Role == 4).OrderBy(m => m.UserID).ToList();
            model.CreateBy = SIT.CreateBy;
            var Step = db.SITSteps.Where(m => m.SIT_ID == SIT_ID).OrderBy(m => m.Step_ID).ToList();
            model.state = Step.Count();
            List<SITModel> SIT_List = new List<SITModel>();
            foreach (var item in Step)
            {
                var task = db.Tasks.Where(m => m.TaskID == item.Task_ID).FirstOrDefault();
                SIT_List.Add(new SITModel {
                    _Step_ID = item.Step_ID,
                    Task_Name = task.TaskName,
                    ID_Task = item.Task_ID,
                });
            }
            ViewBag.DataList = SIT_List;
            return View(model);
        }
        public ActionResult EditSIT(SITModel model)
        {
            int user = Convert.ToInt32(Session["userID"]);
            var s = db.SITs.Where(m => m.SIT_ID == model.SIT_ID).FirstOrDefault();
            if (model.CreateBy == user)
            {
                s.Name = model.Name;
                s.Detail = model.Detail;
                s.Dev_ID = model.Dev_ID;
                s.Tester_ID = model.Tester_ID;
                s.QA_ID = model.QA_ID;
                if (model.AttachFile != null)
                {
                    var Upload = Upload_FileSIT(model.AttachFile);
                    string[] txt = Upload.Split(",".ToCharArray());
                    s.AttachFile = txt[0];
                    s.AttachShow = txt[1];
                }
                s.Send_Date_T = model.Send_Date_T;
                s.Send_Date_Q = model.Send_Date_Q;
                s.UpdateBy = Convert.ToInt32(Session["userID"]);
                s.UpdateDate = DateTime.Now;
                db.SaveChanges();
                int c = model.TaskList.Count;
                //if (c > 1)
                //{
                //    for (int i = 0; i < c; i++)
                //    {
                //        var step = db.SITSteps.Where(m => m.Step_ID == model.StepList[i]).OrderBy(m => m.Step_ID).FirstOrDefault();
                //        step.Task_ID = model.TaskList[i];
                //        db.SaveChanges();
                //    }
                //}
                if (c > 1)
                {
                    for (int i = 0; i < c; i++)
                    {
                        int List = model.StepList[i];
                        var step = db.SITSteps.Where(m => m.Step_ID == List).OrderBy(m => m.Step_ID).FirstOrDefault();
                        db.SITSteps.Remove(step);
                        db.SaveChanges();
                    }
                    SITStep db_step = new SITStep();
                    for (int item = 0; item < c; item++)
                    {
                        db_step.Task_ID = model.TaskList[item];
                        db_step.Step = item;
                        db_step.SIT_ID = s.SIT_ID;
                        db.SITSteps.Add(db_step);
                        db.SaveChanges();
                    }
                }
            }
            else if (model.Tester_ID == user)
            {
                s.Commnet_Test = model.Commnet_T;
                s.UpdateBy = Convert.ToInt32(Session["userID"]);
                s.UpdateDate = DateTime.Now;
                db.SaveChanges();
                Session["Show"] = 1;
                return RedirectToAction("DetailSIT", new { SIT_ID = model.SIT_ID });
            }
            else if (model.QA_ID == user)
            {
                s.Comment_QA = model.Comment_QA;
                s.UpdateBy = Convert.ToInt32(Session["userID"]);
                s.UpdateDate = DateTime.Now;
                db.SaveChanges();
                Session["Show"] = 1;
                return RedirectToAction("DetailSIT", new { SIT_ID = model.SIT_ID });
            }
            else if (model.Handle == 0)
            {
                s.Comment_CM = model.Comment_CM;
                s.UpdateBy = Convert.ToInt32(Session["userID"]);
                s.UpdateDate = DateTime.Now;
                Session["Show"] = 1;
                return RedirectToAction("DetailSIT", new { SIT_ID = model.SIT_ID });
            }
            return RedirectToAction("ShowSIT");
        }
        public ActionResult SendSIT(int SIT_ID)
        {
            var SIT = db.SITs.Where(m => m.SIT_ID == SIT_ID).FirstOrDefault();
            if (SIT.Status == 0)
            {
                SIT.Status = 1;
                SIT.Handle = SIT.QA_ID;
                SIT.UpdateBy = Convert.ToInt32(Session["userID"]);
                SIT.UpdateDate = DateTime.Now;
                db.SaveChanges();
            }
            else if (SIT.Status == 1)
            {
                SIT.Status = 2;
                SIT.Handle = 0;
                SIT.UpdateBy = Convert.ToInt32(Session["userID"]);
                SIT.UpdateDate = DateTime.Now;
                db.SaveChanges();
            }
            else if (SIT.Status == 2)
            {
                SIT.Status = 3;
                SIT.UpdateBy = Convert.ToInt32(Session["userID"]);
                SIT.UpdateDate = DateTime.Now;
                db.SaveChanges();
            }
            return RedirectToAction("ShowSIT", "SIT");
        }
        public ActionResult Defect_SIT(int SIT_ID)
        {
            Session["SIT_ID"] = SIT_ID;
            Session["Defect_SIT"] = 1;
            return RedirectToAction("AddDefect", "Defect");
        }
        private bool isValidContentType(string contentType)
        {
            return contentType.Equals("image/png") || contentType.Equals("image/jpg") || contentType.Equals("application/pdf") || contentType.Equals("image/jpeg");
        }

        private bool isValidContentLength(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1; // 1MB
        }
        public string Upload_FileSIT(HttpPostedFileBase File)
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
                    string i = "/Upload/" + fileName + "," + fileName;
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
    }
}