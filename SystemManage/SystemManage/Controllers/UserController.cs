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
        [HttpPost]
        public ActionResult AddUser(UserModel model)
        {
            if (model.Users_ID != 0)
            {
                var r = db.Users.Where(m => m.User_ID == model.Users_ID).FirstOrDefault();
                r.User_ID = model.Users_ID;
                r.NickName = model.NikcName;
                r.User_Email = model.User_Email;
                r.User_Name = model.User_Name;
                r.User_Password = model.User_Password;
                r.User_LastName = model.User_LastName;
                r.Gender = model.Gender;
                r.Address = model.Address;
                r.Phone = model.Phone;
                r.Contract_ID = model.Contract_ID;
                r.Position_ID = model.Position_ID;
                r.ContractFrom = model.ContractFrom;
                r.Date_of_Started = model.Date_of_Started;
                r.Date_of_Ended = model.Date_of_Ended;
                r.Reading = Convert.ToInt32(model.Reading);
                r.Speaking = Convert.ToInt32(model.Speaking);
                r.Writng = Convert.ToInt32(model.Writng);
                db.SaveChanges();
                ModelState.Clear();
            }
            else
            {
                User u = new User();
                Skill s = new Skill();
                u.User_Email = model.User_Email;
                u.User_Password = model.User_Password;
                u.User_Name = model.User_Name;
                u.NickName = model.NikcName;
                u.User_LastName = model.User_LastName;
                if (model.Gender == "ชาย")
                {
                    u.Gender = "M";
                }
                else
                {
                    u.Gender = "F";
                } 
                u.BirthDate = model.BirthDate;
                u.Address = model.Address;
                u.Contract_ID = model.Contract_ID; //Ref ??? Table  Contract
                u.ContractFrom = model.ContractFrom;
                u.Date_of_Started = model.Date_of_Started;
                u.Date_of_Ended = model.Date_of_Ended;
                u.Permisstion = model.Permission;
                u.Position_ID = model.Position_ID;
                u.comment = model.Comment;
                u.LanguageID = model.LanguageID; //Ref ??? Table Skills
                u.AVG = 0;
                u.Amount_Succ =0;
                u.Amount_Non = 0;
                u.TotalCoding = 0;
                if (model.Speaking == "สูง")
                {
                    u.Speaking = 3;
                }
                else if (model.Speaking == "ปานกลาง")
                {
                    u.Speaking = 2;
                }
                else if (model.Speaking == "ต่ำ")
                {
                    u.Speaking = 1;
                }
                /////////////////////////////////////////
                if (model.Reading == "สูง")
                {
                    u.Reading = 3;
                }
                else if (model.Reading == "ปานกลาง")
                {
                    u.Reading = 2;
                }
                else if (model.Reading == "ต่ำ")
                {
                    u.Reading = 1;
                }
                /////////////////////////////////////
                if (model.Writng == "สูง")
                {
                    u.Writng = 3;
                }
                else if (model.Writng == "ปานกลาง")
                {
                    u.Writng = 2;
                }
                else if (model.Writng == "ต่ำ")
                {
                    u.Writng = 1;
                }
                /////////////////////////////////////
                if (model.Listening == "สูง")
                {
                    u.Listening = 3;
                }
                else if (model.Listening == "ปานกลาง")
                {
                    u.Listening = 2;
                }
                else if (model.Listening == "ต่ำ")
                {
                    u.Listening = 1;
                }
                u.CreateDate = DateTime.Now;
                u.CreateBy = Convert.ToInt32(Session["userID"]);
                db.Users.Add(u);
                db.SaveChanges();
                s.User_ID = u.User_ID;
                s.languageID = model.LanguageID;
                db.Skills.Add(s);
                db.SaveChanges();
                ModelState.Clear();
            }
            UserModel ModelList = new UserModel();
            ModelList.PositionList = db.Positions.OrderByDescending(m => m.Position_ID).ToList();
            ModelList.ContractsList = db.Type_of_Contract.OrderByDescending(m => m.Contrat_ID).ToList();
            ModelList.LanguageList = db.Language_of_Type.OrderByDescending(m => m.languageID).ToList();
            List<UserModel> UserList = new List<UserModel>();
            var item = db.Users.OrderByDescending(m => m.User_ID).ToList();
            foreach (var i in item)
            {
                var P_Name = db.Positions.Where(m => m.Position_ID == i.Position_ID).FirstOrDefault();
                var C_Name = db.Type_of_Contract.Where(m => m.Contrat_ID == i.Contract_ID).FirstOrDefault();
                UserList.Add(new UserModel {
                    Users_ID = i.User_ID,
                    User_Email = i.User_Email,
                    User_Name = i.User_Name,
                    User_LastName = i.User_LastName,
                    Gender = i.Gender,
                    PositionName = P_Name.Name,
                    ContactName = C_Name.Contrat_Name
                });
            }
            ViewBag.DataList = UserList;
            return PartialView("ShowUser",ModelList);
        }
        public ActionResult ShowUser()
        {
            UserModel ModelList = new UserModel();
            ModelList.PositionList = db.Positions.ToList<Position>();
            ModelList.ContractsList = db.Type_of_Contract.ToList<Type_of_Contract>();
            ModelList.LanguageList = db.Language_of_Type.ToList<Language_of_Type>();
            List<UserModel> model = new List<UserModel>();
            var item = db.Users.ToList();
            foreach (var i in item)
            {
                var P_Name = db.Positions.Where(m => m.Position_ID == i.Position_ID).FirstOrDefault();
                var C_Name = db.Type_of_Contract.Where(m => m.Contrat_ID == i.Contract_ID).FirstOrDefault();
                model.Add(new UserModel
                {
                    Users_ID = i.User_ID,
                    User_Email = i.User_Email,
                    User_Name = i.User_Name,
                    NikcName = i.NickName,
                    User_LastName = i.User_LastName,
                    Gender = i.Gender,
                    PositionName = P_Name.Name,
                    ContactName = C_Name.Contrat_Name
                });
            }
            ViewBag.DataList = model;
            return View(ModelList);
        }
        [HttpPost]
        public ActionResult DetailUser(string Users_ID)
        {
            UserModel model = new UserModel();
            int userId = Convert.ToInt32(Users_ID);
            var u = db.Users.Where(m => m.User_ID == userId).FirstOrDefault();
            model.Users_ID = u.User_ID;
            model.User_Name = u.User_Name;
            model.User_LastName = u.User_LastName;
            model.NikcName = u.NickName;
            model.User_Email = u.User_Email;
            model.BirthDate = u.BirthDate;
            model.Address = u.Address;
            model.Contract_ID = u.Contract_ID;
            model.ContractFrom = u.ContractFrom;
            model.Position_ID = u.Position_ID;
            model.Phone = u.Phone;
            model.Permission = u.Permisstion;
            model.Date_of_Started = u.Date_of_Started;
            model.Date_of_Ended = u.Date_of_Ended;
            model.User_Password = u.User_Password;
            model.LanguageID = u.LanguageID;
            if (u.Gender == "M")
            {
                model.Genders = UserModel.Sex.ชาย;
            }
            else if (u.Gender == "F")
            {
                model.Genders = UserModel.Sex.หญิง;
            }
            ////////////////////
            if (u.Speaking == 3)
            {
                model._Speaking = UserModel.Levels.สูง;
            }
            else if (u.Speaking == 2)
            {
                model._Speaking = UserModel.Levels.ปานกลาง;
            }
            else if (u.Speaking == 1)
            {
                model._Speaking = UserModel.Levels.ต่ำ;
            }
            /////////////////////////////////////////
            if (u.Reading == 3)
            {
                model._Reading = UserModel.Levels.สูง;
            }
            else if (u.Reading == 2)
            {
                model._Reading = UserModel.Levels.ปานกลาง;
            }
            else if (u.Reading == 1)
            {
                model._Reading = UserModel.Levels.ต่ำ;
            }
            /////////////////////////////////////
            if (u.Writng == 3)
            {
                model._Writng = UserModel.Levels.สูง;
            }
            else if (u.Writng == 2)
            {
                model._Writng = UserModel.Levels.ปานกลาง;
            }
            else if (u.Writng == 1)
            {
                model._Writng = UserModel.Levels.ต่ำ;
            }
            /////////////////////////////////////
            if (u.Listening == 3)
            {
                model._Listening = UserModel.Levels.สูง;
            }
            else if (u.Listening == 2)
            {
                model._Listening = UserModel.Levels.ปานกลาง;
            }
            else if (u.Listening == 1)
            {
                model._Listening = UserModel.Levels.ต่ำ;
            }
            return Json(new { UserID = model.Users_ID ,
            Name = model.User_Name ,
            LastName = model.User_LastName ,
            Email = model.User_Email ,
            BirthDate = model.BirthDate,
            Address = model.Address,
            Contract = model.Contract_ID,
            ContractFrom = model.ContractFrom,
            Postition = model.Position_ID,
            Phone = model.Phone,
            Permission = model.Permission,
            Started = model.Date_of_Started,
            Ended = model.Date_of_Ended,
            Password = model.User_Password,
            Language = model.LanguageID,
            Genders = model.Genders,
            Speaking = model._Speaking,
            Reading = model._Reading,
            Writng= model._Writng,
            Listening = model._Listening
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteUser(int userID)
        {
            var user = db.Users.Where(m => m.User_ID == userID).FirstOrDefault();
            var skills = db.Skills.Where(m => m.User_ID == user.User_ID).ToList();
            foreach (var t in skills)
            {
                db.Skills.Remove(t);
                db.SaveChanges();
            }
            db.Users.Remove(user);
            db.SaveChanges();
            return RedirectToAction("ShowUser");
        }
    }
    
}