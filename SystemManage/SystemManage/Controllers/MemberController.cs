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
        [HttpPost]
        public ActionResult AddMember(MemberModel model,int UserID)
        {
            ProjectMember pm = new ProjectMember();
            pm.UserID = UserID;
            pm.ProjectID = Convert.ToInt32(Session["ProjectID"]);
            pm.Role = model.Role;
            return RedirectToAction("ShowTeam");
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
                MemberList.Add(new MemberModel
                {
                    UserID = i.UserID,
                });
                var item2 = db.Users.Where(m => m.User_ID == i.UserID).ToList();
                foreach (var U in item2)
                {
                    UserList.Add(new UserModel
                    {
                        User_Email = U.User_Email,
                        User_Name = U.User_Name,
                        User_LastName = U.User_LastName,
                        Position_ID = U.Position_ID,
                        AVG = U.AVG,
                        Amount_Succ = U.Amount_Succ,
                        TotalCoding = U.TotalCoding,
                        LanguageID = U.LanguageID
                    });
                }
            }
            ViewBag.DataList = UserList;
            return View();
        }
    }
}