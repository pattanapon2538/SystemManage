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
        // GET: Role
        public ActionResult AddPosition()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddPosition(PositionModel model)
        {
            Position r = new Position();
            var users = 1; //[Session].Save
            r.Position_ID = model.Position_ID;
            r.Name = model.Name;
            r.Initial = model.Initial;
            r.Detail = model.Detail;
            r.CreateDate = DateTime.Now;
            r.CreateBy = users;
            db.Positions.Add(r);
            db.SaveChanges();
            return RedirectToAction("ShowPosition");
        }
        public ActionResult ShowPosition()
        {
            List<PositionModel> model = new List<PositionModel>();
            var item = db.Positions.ToList();
            foreach(var i in item)
            {
                model.Add(new PositionModel
                {
                    Position_ID = i.Position_ID,
                    Name = i.Name,
                    Initial = i.Initial,
                    Detail = i.Detail,
                    CreateDate = i.CreateDate,
                    UpdateDate = i.UpdateDate,
                    CreateBy = i.CreateBy,
                   // UpdateBy = i.UpdateBy
                });
            }
            ViewBag.DataLsit = model;
            return View();
        }
        public ActionResult DetailPosition(int Position_ID)
        {
            PositionModel Model = new PositionModel();
            Position r = db.Positions.Where(w => w.Position_ID == Position_ID).FirstOrDefault();
            Model.Position_ID = r.Position_ID;
            Model.Name = r.Name;
            Model.Initial = r.Initial;
            Model.Detail = r.Detail;
            return View(Model);
        }
        public ActionResult EditPosition(int Position_ID)
        {
            PositionModel Model = new PositionModel();
            Position r = db.Positions.Where(w => w.Position_ID == Position_ID).FirstOrDefault();
            r.Name = Model.Name;
            r.Initial = Model.Initial;
            r.Detail = Model.Detail;
            r.UpdateBy = 11;
            r.UpdateDate = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("ShowPosition");
        }
        public ActionResult DeletePosition(int Position_ID)
        {
            PositionModel Model = new PositionModel();
            Position r = db.Positions.Where(w => w.Position_ID == Position_ID).FirstOrDefault();
            db.Positions.Remove(r);
            db.SaveChanges();
            return RedirectToAction("ShowPosition");

        }
    }
}