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
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (cm.Contrat_ID != 0)
            {
                var result = db.Type_of_Contract.Where(m => m.Contrat_ID == cm.Contrat_ID).FirstOrDefault();
                result.Contrat_ID = cm.Contrat_ID;
                result.Contrat_Name = cm.Contrat_Name;
                result.Contrat_Detail = cm.Contrat_Detail;
                result.UpdateDate = DateTime.Now;
                result.UpdateBy = Convert.ToInt32(Session["userID"]); ;
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
                tc.CreateBy = Convert.ToInt32(Session["userID"]);
                db.Type_of_Contract.Add(tc);
                db.SaveChanges();
                ModelState.Clear();
            }
            //TypeOfCotractModel a = new TypeOfCotractModel();
            //List<TypeOfCotractModel> model = new List<TypeOfCotractModel>();
            //var item = db.Type_of_Contract.OrderByDescending(s=>s.Contrat_ID).ToList();
            //foreach (var i in item)
            //{
            //    model.Add(new TypeOfCotractModel
            //    {
            //        Contrat_ID = i.Contrat_ID,
            //        Contrat_Name = i.Contrat_Name,
            //        Contrat_Detail = i.Contrat_Detail,
            //        CreateDate = i.CreateDate,
            //        UpdateDate = i.UpdateDate,
            //        CreateBy = i.CreateBy,
            //        UpdateBy = i.UpdateBy,
            //    });
            //}
            //ViewBag.DataList = model;
            //return PartialView("ShowContact");
            return RedirectToAction("ShowContact", "Contact");
        }
        public ActionResult ShowContact()
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            List<TypeOfCotractModel> model = new List<TypeOfCotractModel>();
            TypeOfCotractModel a = new TypeOfCotractModel();
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
            if (Convert.ToInt32(Session["Alert_C"]) == 1)
            {
                a.Alert = 1;
                Session["Alert_C"] = 0;
                return View(a);
            }
            else
            {
                a.Alert = 0;
                return View(a);
            }
            
        }
        [HttpPost]
        public ActionResult DetailContact(string Contrat_ID)
        {
            if ((Session["userID"]) == null)
            {
                return RedirectToAction("Index", "Login");
            }
            TypeOfCotractModel model = new TypeOfCotractModel();
            Type_of_Contract tc = db.Type_of_Contract.Where(m => m.Contrat_ID.ToString() == Contrat_ID).FirstOrDefault();
            model.Contrat_ID = tc.Contrat_ID;
            model.Contrat_Name = tc.Contrat_Name;
            model.Contrat_Detail = tc.Contrat_Detail;
            return Json(new { Contrat_ID = tc.Contrat_ID, Contrat_Name = tc.Contrat_Name, Contrat_Detail = tc.Contrat_Detail }, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult DeleteContact(int Contrat_ID)
        {
            try
            {
                TypeOfCotractModel model = new TypeOfCotractModel();
                Type_of_Contract tc = db.Type_of_Contract.Where(m => m.Contrat_ID == Contrat_ID).FirstOrDefault();
                db.Type_of_Contract.Remove(tc);
                db.SaveChanges();
                return RedirectToAction("ShowContact");
            }
            catch (Exception)
            {
                Session["Alert_C"] = 1;
                return RedirectToAction("ShowContact", "Contact");
            }
        }
        
    }
}