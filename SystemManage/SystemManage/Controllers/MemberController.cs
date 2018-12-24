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
            List<UserModel> UserList = new List<UserModel>();
            var item = db.Users.OrderByDescending(m => m.User_ID).ToList();
            foreach(var i in item)
            {
                var PositionDB = db.Positions.Where(m => m.Position_ID == i.Position_ID).FirstOrDefault();
                UserList.Add(new UserModel
                {
                    Users_ID = i.User_ID,
                    User_Email = i.User_Email,
                    User_Name = i.User_Name,
                    User_LastName = i.User_LastName,
                    PositionName = PositionDB.Name,
                    TotalCoding = i.TotalCoding,
                    Amount_Succ = i.Amount_Succ,
                    AVG = i.AVG
                });
                ViewBag.DataList = UserList;
            }
            return View();
        }
        public ActionResult AddMember(int UserID)
        {
            ProjectMember pm = new ProjectMember();
            pm.UserID = UserID;
            pm.ProjectID = Convert.ToInt32(Session["ProjectID"]);
            var PoList = db.Users.Where(m => m.User_ID == UserID).FirstOrDefault();
            if (PoList.Position_ID == 113)//PositionID = 113 Dev 
            {
                pm.Role = 2; 
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                return RedirectToAction("ListMember");
            }
            else if (PoList.Position_ID == 110)//PositionID = 110 Tester
            {
                pm.Role = 3; 
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                return RedirectToAction("ListMember");
            }
            else if (PoList.Position_ID == 142)//PositionID = 142 QA 
            {
                pm.Role = 4; 
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                return RedirectToAction("ListMember");
            }
            else if (PoList.Position_ID == 143)//PositionID = 143 ลูกค้า 
            {
                pm.Role = 5; 
                db.ProjectMembers.Add(pm);
                db.SaveChanges();
                return RedirectToAction("ListMember");
            }else //ถ้าไม่เข้าเงื่อไขให้เป็น Dev ทั้งหมด
            pm.Role = 2; 
            db.ProjectMembers.Add(pm);
            db.SaveChanges();
            return RedirectToAction("ListMember");
        }
        public ActionResult DeleteMember(int UserID)
        {
            var pm = db.ProjectMembers.Where(m => m.UserID == UserID).FirstOrDefault();
            db.ProjectMembers.Remove(pm);
            db.SaveChanges();
            return RedirectToAction("ShowTeam");
        }
        public ActionResult ShowMember()
        {
            List<MemberModel> MemberList = new List<MemberModel>();
            List<UserModel> UserList = new List<UserModel>();
            int DataProjectID = Convert.ToInt32(Session["ProjectID"]);
            var item = db.ProjectMembers.Where(m => m.ProjectID == DataProjectID).ToList();
            foreach (var i in item)
            {
                var item2 = db.Users.Where(m => m.User_ID == i.UserID).FirstOrDefault();
                var dbPosition = db.Positions.Where(m => m.Position_ID == item2.Position_ID).FirstOrDefault();
                UserList.Add(new UserModel
                {
                    Users_ID = item2.User_ID,
                    User_Email = item2.User_Email,
                    User_Name = item2.User_Name,
                    User_LastName = item2.User_LastName,
                    PositionName = dbPosition.Name,
                    TotalCoding = item2.TotalCoding,
                    Amount_Succ = item2.Amount_Succ,
                    AVG = item2.AVG
                });
            }
            ViewBag.DataList = UserList;
            return View();
        }
    }
}