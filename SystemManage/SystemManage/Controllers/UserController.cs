using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;

namespace SystemManage.Controllers
{
    public class UserController : Controller
    {
        Entities db = new Entities();
        // GET: User
        public ActionResult AddUser()
        {
            UserModel u = new UserModel();
            u.PositionList = db.Positions.ToList<Position>();
            u.ContractsList = db.Type_of_Contract.ToList<Type_of_Contract>();
            u.LanguageList = db.Language_of_Type.ToList<Language_of_Type>();
            return View(u);
        }
        [HttpPost]
        public ActionResult AddUser(UserModel model)
        {
            User u = new User();
            u.User_ID = model.Users_ID;
            u.User_Email = model.User_Email;
            u.User_Password = model.User_Password;
            u.User_Name = model.User_Name;
            u.User_LastName = model.User_LastName;
            u.Gender = "m";
            u.BirthDate = DateTime.Now;
            u.Address = model.Address;
            u.Contract_ID = model.Contract_ID; //Ref ??? Table  Contract
            u.ContractFrom = model.ContractFrom;
            u.Date_of_Started = DateTime.Now;
            u.Date_of_Ended = DateTime.Now;
            u.AttachFile1 = model.AttachFile1;
            u.AttachFile2 = model.AttachFile2;
            u.AttachFile3 = model.AttachFile3;
            u.AttachFile4 = model.AttachFile4;
            u.AttachShow1 = model.AttachShow1;
            u.AttachShow2 = model.AttachShow2;
            u.AttachShow3 = model.AttachShow3;
            u.AttachShow4 = model.AttachShow4;
            u.Permisstion = "1";
            u.Position_ID = model.Position_ID;
            u.comment = model.comment;
            u.LanguageID = model.LanguageID; //Ref ??? Table Skills
            u.AVG = model.AVG;
            u.Amount_Succ = model.Amount_Succ;
            u.Amount_Non = model.Amount_Non;
            u.TotalCoding = model.TotalCoding;
            u.Speaking = model.Speaking;
            u.Reading = model.Reading;
            u.Listening = model.Listening;
            u.Writng = model.Writng;
            u.CreateDate = DateTime.Now;
            u.CreateBy = 11; //[Session]
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("ShowUser");
        }
        public ActionResult ShowUser()
        {
            List<UserModel> model = new List<UserModel>();
            var item = db.Users.ToList();
            foreach (var i in item)
            {
                model.Add(new UserModel
                {
                    Users_ID = i.User_ID,
                    User_Email = i.User_Email,
                    User_Password = i.User_Password,
                    User_Name = i.User_Name,
                    User_LastName = i.User_LastName,
                    Gender = i.Gender,
                    Address = i.Address,
                    Contract_ID = i.Contract_ID,
                    Position_ID = i.Position_ID,
                    LanguageID = i.LanguageID,
                });
            }
            ViewBag.DataList = model;
            return View();
        }
    }
       
}