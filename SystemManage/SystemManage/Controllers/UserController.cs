using System;
using System.Collections.Generic;
using System.IO;
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
            Skill s = new Skill();
            u.User_ID = model.Users_ID;
            u.User_Email = model.User_Email;
            u.User_Password = model.User_Password;
            u.User_Name = model.User_Name;
            u.User_LastName = model.User_LastName;
            u.Gender = model.Gender;
            u.BirthDate = DateTime.Now;
            u.Address = model.Address;
            u.Contract_ID = model.Contract_ID; //Ref ??? Table  Contract
            u.ContractFrom = model.ContractFrom;
            u.Date_of_Started = DateTime.Now;
            u.Date_of_Ended = DateTime.Now;
            //String fileName = Path.GetFileNameWithoutExtension(model.File1.FileName);
            //String extension = Path.GetExtension(model.File1.FileName);
            //fileName = fileName + DateTime.Now.ToString("dd/mm/yyyy") + extension;
            //model.AttachFile1 = "~/Uploade/" + fileName;
            //fileName = Path.Combine(Server.MapPath("~/Uploade/"), fileName);
            //model.File1.SaveAs(fileName);
            //u.AttachFile1 = model.AttachFile1;
            //u.AttachFile2 = model.AttachFile2;
            //u.AttachFile3 = model.AttachFile3;
            //u.AttachFile4 = model.AttachFile4;
            //u.AttachShow1 = model.AttachShow1;
            //u.AttachShow2 = model.AttachShow2;
            //u.AttachShow3 = model.AttachShow3;
            //u.AttachShow4 = model.AttachShow4;
            u.Permisstion = model.Permission;
            u.Position_ID = model.Position_ID;
            u.comment = model.comment;
            u.LanguageID = model.LanguageID; //Ref ??? Table Skills
            s.languageID = model.LanguageID;
            u.AVG = model.AVG;
            u.Amount_Succ = model.Amount_Succ;
            u.Amount_Non = model.Amount_Non;
            u.TotalCoding = model.TotalCoding;
            u.Speaking = Convert.ToInt32(model.Speaking);
            u.Reading = Convert.ToInt32(model.Reading);
            u.Listening = Convert.ToInt32(model.Listening);
            u.Writng = Convert.ToInt32(model.Writng);
            u.CreateDate = DateTime.Now;
            u.CreateBy = 11; //[Session]
            db.Users.Add(u);
            s.User_ID = u.User_ID;
            db.Skills.Add(s);
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