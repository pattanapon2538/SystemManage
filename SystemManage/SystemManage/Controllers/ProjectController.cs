using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Models;
using SystemManage.Database;

namespace SystemManage.Controllers
{
    
    public class ProjectController : Controller
    {
        Entities db = new Entities();
        // GET: Project
        public ActionResult AddProject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddProject(ProjectModel model)
        {
            Project p = new Project();
            p.Name = model.ProjectName;
            p.Description = model.ProjectDescription;
            p.CreateDate = DateTime.Now;
            p.SendDate = model.ProjectSendDate;
            p.Status = 1;
            p.CreateBy = 11;
            db.Projects.Add(p);
            db.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("ShowProject");
        }
        public ActionResult ShowProject()
        {
            List<ProjectModel> projectlist = new List<ProjectModel>();
            var item = db.Projects.OrderByDescending(m => m.ProjectID).ToList();
            foreach(var i in item)
            {
                projectlist.Add(new ProjectModel
                {
                 ProjectID = i.ProjectID,
                 ProjectName = i.Name,
                 ProjectDescription = i.Description,
                 ProjectStatus = i.Status,
                 ProjectSendDate = i.SendDate,
                 CreateDate = i.CreateDate,
                 UpdateDate = i.UpdateDate,
                }); 
            }
            ViewBag.DataList = projectlist;
            return View();
        }
        public ActionResult EditProject(String ProjectID)
        {
            ProjectModel Model = new ProjectModel();
            Project p = db.Projects.Where(m => m.ProjectID.ToString() == ProjectID).FirstOrDefault();
            Model.ProjectID = p.ProjectID;
            Model.ProjectName = p.Name;
            Model.ProjectDescription = p.Description;
            Model.ProjectSendDate = p.SendDate;
            return View(Model);

        }
        public ActionResult SaveEdit(ProjectModel Model)
        {
            Project p = db.Projects.Where(m => m.ProjectID == Model.ProjectID).FirstOrDefault();
            p.Name = Model.ProjectName;
            p.Description = Model.ProjectDescription;
            p.Status = Model.ProjectStatus;
            p.UpdateDate = DateTime.Now;
            p.UpdateBy = 11;
            db.SaveChanges();
            return RedirectToAction("ShowProject");

        }
        public ActionResult DeleteProject(int ProjectID)
        {
            Project d = db.Projects.Where(m => m.ProjectID == ProjectID).FirstOrDefault();
            db.Projects.Remove(d);
            db.SaveChanges();
            return RedirectToAction("ShowProject");
        }
        public ActionResult GetData(String ProjectID)
        {
            ProjectModel Model = new ProjectModel();
            Project p = db.Projects.Where(m => m.ProjectID.ToString() == ProjectID).FirstOrDefault();
            Model.ProjectID = p.ProjectID;
            return RedirectToAction("ShowTask","Task", new { ProjectID = Model.ProjectID });
        }
    }
}