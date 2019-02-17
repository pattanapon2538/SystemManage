using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;

namespace SystemManage.Controllers
{
    public class PositionController : Controller
    {
        Entities db = new Entities();
        // GET: Position_ID
        [HttpPost]
        public ActionResult AddPosition(PositionModel pm)
        {
                if (pm.Position_ID != 0)
                {
                    var result = db.Positions.Where(s => s.Position_ID == pm.Position_ID).FirstOrDefault();
                    result.Name = pm.Name;
                    result.Initial = pm.Initial;
                    result.Detail = pm.Detail;
                    result.UpdateDate = DateTime.Now;
                    result.UpdateBy = 11;
                    db.SaveChanges();
                    ModelState.Clear();
                }
                else
                {
                    Position r = new Position();
                    r.Position_ID = pm.Position_ID;
                    r.Name = pm.Name;
                    r.Initial = pm.Initial;
                    r.Detail = pm.Detail;
                    r.CreateDate = DateTime.Now;
                    r.CreateBy = 11;
                    db.Positions.Add(r);
                    db.SaveChanges();
                    ModelState.Clear();
                }

            List<PositionModel> positionsList = new List<PositionModel>();
            var item = db.Positions.OrderByDescending(s => s.Position_ID).ToList();
            foreach (var i in item)
            {
                positionsList.Add(new PositionModel
                {
                    Position_ID = i.Position_ID,
                    Name = i.Name,
                    Initial = i.Initial,
                    Detail = i.Detail,
                    CreateDate = i.CreateDate,
                    UpdateDate = i.UpdateDate,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                });
            }
            ViewBag.DataList = positionsList;
            return PartialView("ShowPosition");
        }

        public ActionResult ShowPosition()
        {
            List<PositionModel> positionsList = new List<PositionModel>();
            var item = db.Positions.OrderByDescending(s => s.Position_ID).ToList();
            foreach (var i in item)
            {
                positionsList.Add(new PositionModel
                {
                    Position_ID = i.Position_ID,
                    Name = i.Name,
                    Initial = i.Initial,
                    Detail = i.Detail,
                    CreateDate = i.CreateDate,
                    UpdateDate = i.UpdateDate,
                    CreateBy = i.CreateBy,
                    UpdateBy = i.UpdateBy,
                });
            }
            ViewBag.DataList = positionsList;
            return View();
        }

        [HttpPost]
        public ActionResult DetailPosition(string Position_ID)
        {
            PositionModel Model = new PositionModel();
            Position r = db.Positions.Where(w => w.Position_ID.ToString() == Position_ID).FirstOrDefault();
            Model.Position_ID = r.Position_ID;
            Model.Name = r.Name;
            Model.Initial = r.Initial;
            Model.Detail = r.Detail;
            return Json(new { Position_ID = r.Position_ID, Name = r.Name , Initial = r.Initial , Detail = r.Detail }, JsonRequestBehavior.AllowGet);
        }
       
        public ActionResult DeletePosition(int Position_ID)
        {
            Position r = db.Positions.Where(w => w.Position_ID == Position_ID).FirstOrDefault();
            db.Positions.Remove(r);
            db.SaveChanges();
            return RedirectToAction("ShowPosition");

        }
        
    }
}