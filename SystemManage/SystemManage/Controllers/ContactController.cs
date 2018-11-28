using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;

namespace SystemManage.Controllers
{
    public class ContactController : Controller
    {
        Entities db = new Entities();
        // GET: Contact
        [HttpPost]
        public ActionResult AddContact(TypeOfCotractModel cm)
        {
            if (cm.Contrat_ID != 0)
            {
                var result = db.Type_of_Contract.Where(m => m.Contrat_ID == cm.Contrat_ID).FirstOrDefault();
                cm.Contrat_ID = result.Contrat_ID;
                cm.Contrat_Name = result.Contrat_Name;
                cm.Contrat_Detail = result.Contrat_Detail;
                cm.UpdateDate = DateTime.Now;
                cm.UpdateBy = 11;
                db.SaveChanges();
                ModelState.Clear();
            }
            else
            {
                // var user = "Plus";//Session["userName"].ToString(); Test
                Type_of_Contract tc = new Type_of_Contract();
                tc.Contrat_ID = cm.Contrat_ID;
                tc.Contrat_Name = cm.Contrat_Name;
                tc.Contrat_Detail = cm.Contrat_Detail;
                tc.CreateDate = DateTime.Now;
                tc.CreateBy = 11;
                db.Type_of_Contract.Add(tc);
                db.SaveChanges();
                ModelState.Clear();
            }
            
            List<TypeOfCotractModel> model = new List<TypeOfCotractModel>();
            var item = db.Type_of_Contract.OrderByDescending(s=>s.Contrat_ID).ToList();
            foreach (var i in item)
            {
                model.Add(new TypeOfCotractModel
                {
                    Contrat_ID = i.Contrat_ID,
                    Contrat_Name = i.Contrat_Name,
                    Contrat_Detail = i.Contrat_Detail,
                    CreateDate = i.CreateDate,
                    UpdateDate = i.UpdateDate,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                });
            }
            ViewBag.DataList = model;
            return PartialView("ShowContact");
        }
        public ActionResult ShowContact()
        {
            List<TypeOfCotractModel> model = new List<TypeOfCotractModel>();
            var item = db.Type_of_Contract.OrderByDescending(s => s.Contrat_ID).ToList();
            foreach (var i in item)
            {
                model.Add(new TypeOfCotractModel
                {
                    Contrat_ID = i.Contrat_ID,
                    Contrat_Name = i.Contrat_Name,
                    Contrat_Detail = i.Contrat_Detail,
                    CreateDate = i.CreateDate,
                    UpdateDate = i.UpdateDate,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                });
            }
            ViewBag.DataList = model;
            return View();
        }
        [HttpPost]
        public ActionResult DetailContact(string Contrat_ID)
        {
            TypeOfCotractModel model = new TypeOfCotractModel();
            Type_of_Contract tc = db.Type_of_Contract.Where(m => m.Contrat_ID.ToString() == Contrat_ID).FirstOrDefault();
            model.Contrat_ID = tc.Contrat_ID;
            model.Contrat_Name = tc.Contrat_Name;
            model.Contrat_Detail = tc.Contrat_Detail;
            return Json(new { Contrat_ID = tc.Contrat_ID, Contrat_Name = tc.Contrat_Name, Contrat_Detail = tc.Contrat_Detail }, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult DeleteContact(int Contrat_ID)
        {
            TypeOfCotractModel model = new TypeOfCotractModel();
            Type_of_Contract tc = db.Type_of_Contract.Where(m => m.Contrat_ID == Contrat_ID).FirstOrDefault();
            db.Type_of_Contract.Remove(tc);
            db.SaveChanges();
            return RedirectToAction("ShowContact");
        }
        //public ActionResult EditContact(Type_of_Contract model)
        //{
        //    Type_of_Contract tc = db.Type_of_Contract.Where(m => m.Contrat_ID == model.Contrat_ID).FirstOrDefault();
        //    tc.Contrat_Name = model.Contrat_Name;
        //    tc.Contrat_Detail = model.Contrat_Detail;
        //    tc.UpdateDate = DateTime.Now;
        //    tc.UpdateBy = 11;
        //    db.SaveChanges();
        //    return RedirectToAction("ShowContact");
        //}
    }
}