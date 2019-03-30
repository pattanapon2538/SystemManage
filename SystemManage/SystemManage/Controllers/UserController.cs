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
                Skill s = new Skill();
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
                if (model.AttachFile1 != null)
                {
                    var Upload = Upload_File_User(model.AttachFile1);
                    string[] text = Upload.Split(",".ToCharArray());
                    r.AttachFile1 = text[0];
                    r.AttachShow1 = text[1];
                }
                if (model.AttachFile2 != null)
                {
                    var Upload = Upload_File_User(model.AttachFile2);
                    string[] text = Upload.Split(",".ToCharArray());
                    r.AttachFile2 = text[0];
                    r.AttachShow2 = text[1];
                }
                if (model.AttachFile3 != null)
                {
                    var Upload = Upload_File_User(model.AttachFile3);
                    string[] text = Upload.Split(",".ToCharArray());
                    r.AttachFile3 = text[0];
                    r.AttachShow3 = text[1];
                }
                if (model.AttachFile4 != null)
                {
                    var Upload = Upload_File_User(model.AttachFile4);
                    string[] text = Upload.Split(",".ToCharArray());
                    r.AttachFile4 = text[0];
                    r.AttachShow4 = text[1];
                }
                var skill = db.Skills.Where(m => m.User_ID == model.Users_ID).OrderBy(m => m.SkillsID).ToList();
                if (skill.Count() != 0)
                {
                    string[] txt = model.Select_Laguages.Split(",".ToCharArray());
                    int item_skill = txt.Count() - 1;
                    if (skill != null)
                    {
                        if (skill.Count == item_skill)
                        {
                            int i = 0;
                            foreach (var sk in skill)
                            {
                                sk.languageID = Convert.ToInt32(txt[i]);
                                sk.User_ID = model.Users_ID;
                                db.SaveChanges();
                                i++;
                            }
                        }
                        else
                        {
                            foreach (var sk in skill)
                            {
                                db.Skills.Remove(sk);
                                db.SaveChanges();
                            }
                            for (int i = 0; i < txt.Count() - 1; i++)
                            {
                                s.languageID = Convert.ToInt32(txt[i]);
                                s.User_ID = model.Users_ID;
                                db.Skills.Add(s);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                if (model.Speaking == "เก่ง")
                {
                    r.Speaking = 2;
                }
                else if (model.Speaking == "ปานกลาง")
                {
                    r.Speaking = 1;
                }
                else if (model.Speaking == "อ่อน")
                {
                    r.Speaking = 0;
                }
                /////////////////////////////////////////
                if (model.Reading == "เก่ง")
                {
                    r.Reading = 2;
                }
                else if (model.Reading == "ปานกลาง")
                {
                    r.Reading = 1;
                }
                else if (model.Reading == "อ่อน")
                {
                    r.Reading = 0;
                }
                /////////////////////////////////////
                if (model.Writng == "เก่ง")
                {
                    r.Writng = 2;
                }
                else if (model.Writng == "ปานกลาง")
                {
                    r.Writng = 1;
                }
                else if (model.Writng == "อ่อน")
                {
                    r.Writng = 0;
                }
                /////////////////////////////////////
                if (model.Listening == "เก่ง")
                {
                    r.Listening = 2;
                }
                else if (model.Listening == "ปานกลาง")
                {
                    r.Listening = 1;
                }
                else if (model.Listening == "อ่อน")
                {
                    r.Listening = 0;
                }
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
                if (model.AttachFile1 != null)
                {
                    var Upload = Upload_File_User(model.AttachFile1);
                    string[] text = Upload.Split(",".ToCharArray());
                    u.AttachFile1 = text[0];
                    u.AttachShow1 = text[1];
                }
                if (model.AttachFile2 != null)
                {
                    var Upload = Upload_File_User(model.AttachFile2);
                    string[] text = Upload.Split(",".ToCharArray());
                    u.AttachFile2 = text[0];
                    u.AttachShow2 = text[1];
                }
                if (model.AttachFile3 != null)
                {
                    var Upload = Upload_File_User(model.AttachFile3);
                    string[] text = Upload.Split(",".ToCharArray());
                    u.AttachFile3 = text[0];
                    u.AttachShow3 = text[1];
                }
                if (model.AttachFile4 != null)
                {
                    var Upload = Upload_File_User(model.AttachFile4);
                    string[] text = Upload.Split(",".ToCharArray());
                    u.AttachFile4 = text[0];
                    u.AttachShow4 = text[1];
                }
                u.AVG = 0;
                u.Amount_Succ =0;
                u.Amount_Non = 0;
                u.TotalCoding = 0;
                if (model.Speaking == "เก่ง")
                {
                    u.Speaking = 2;
                }
                else if (model.Speaking == "ปานกลาง")
                {
                    u.Speaking = 1;
                }
                else if (model.Speaking == "อ่อน")
                {
                    u.Speaking = 0;
                }
                /////////////////////////////////////////
                if (model.Reading == "เก่ง")
                {
                    u.Reading = 2;
                }
                else if (model.Reading == "ปานกลาง")
                {
                    u.Reading = 1;
                }
                else if (model.Reading == "อ่อน")
                {
                    u.Reading = 0;
                }
                /////////////////////////////////////
                if (model.Writng == "เก่ง")
                {
                    u.Writng = 2;
                }
                else if (model.Writng == "ปานกลาง")
                {
                    u.Writng = 1;
                }
                else if (model.Writng == "อ่อน")
                {
                    u.Writng = 0;
                }
                /////////////////////////////////////
                if (model.Listening == "เก่ง")
                {
                    u.Listening = 2;
                }
                else if (model.Listening == "ปานกลาง")
                {
                    u.Listening = 1;
                }
                else if (model.Listening == "อ่อน")
                {
                    u.Listening = 0;
                }
                u.CreateDate = DateTime.Now;
                u.CreateBy = Convert.ToInt32(Session["userID"]);
                db.Users.Add(u);
                db.SaveChanges();
                ModelState.Clear();
                if (model.Select_Laguages != null)
                {
                    string[] txt = model.Select_Laguages.Split(",".ToCharArray());
                    for (int c = 0; c < txt.Count(); c++)
                    {
                        if (txt[c] != "")
                        {
                            s.User_ID = u.User_ID;
                            s.languageID = Convert.ToInt32(txt[c]);
                            db.Skills.Add(s);
                            db.SaveChanges();
                        }
                    }
                }
            }
            //return RedirectToAction("ShowUser", "User");
            UserModel ModelList = new UserModel();
            ModelList.PositionList = db.Positions.OrderByDescending(m => m.Position_ID).ToList();
            ModelList.ContractsList = db.Type_of_Contract.OrderByDescending(m => m.Contrat_ID).ToList();
            ModelList.LanguageList = db.Language_of_Type.OrderByDescending(m => m.languageID).ToList();
            List<UserModel> UserList = new List<UserModel>();
            List<LanguageOfTypeModel> ListData = new List<LanguageOfTypeModel>();
            var item2 = db.Language_of_Type.ToList();
            var item = db.Users.OrderByDescending(m => m.User_ID).ToList();
            foreach (var i in item)
            {
                var P_Name = db.Positions.Where(m => m.Position_ID == i.Position_ID).FirstOrDefault();
                var C_Name = db.Type_of_Contract.Where(m => m.Contrat_ID == i.Contract_ID).FirstOrDefault();
                UserList.Add(new UserModel
                {
                    Users_ID = i.User_ID,
                    User_Email = i.User_Email,
                    User_Name = i.User_Name,
                    User_LastName = i.User_LastName,
                    Gender = i.Gender,
                    PositionName = P_Name.Name,
                    ContactName = C_Name.Contrat_Name
                });
            }
            foreach (var c in item2)
            {
                ListData.Add(new LanguageOfTypeModel
                {
                    languageID = c.languageID,
                    Name = c.Name
                });
            }
            ViewBag.DataList = UserList;
            ViewBag.DataSkill = ListData;
            return PartialView("ShowUser", ModelList);
        }
        public ActionResult ShowUser()
        {
            UserModel ModelList = new UserModel();
            ModelList.Date_of_Started = DateTime.Now;
            ModelList.Date_of_Ended = DateTime.Now;
            ModelList.BirthDate = DateTime.Now;
            ModelList.PositionList = db.Positions.ToList<Position>();
            ModelList.ContractsList = db.Type_of_Contract.ToList<Type_of_Contract>();
            ModelList.LanguageList = db.Language_of_Type.ToList<Language_of_Type>();
            ModelList.Gender = null;
            List<UserModel> model = new List<UserModel>();
            List<LanguageOfTypeModel> ListData = new List<LanguageOfTypeModel>();
            var item = db.Users.ToList();
            var item2 = db.Language_of_Type.ToList();
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
            foreach (var c in item2)
            {
                ListData.Add(new LanguageOfTypeModel {
                    languageID = c.languageID,
                    Name = c.Name
                });
            }
            ViewBag.DataList = model;
            ViewBag.DataSkill = ListData;
            if (Convert.ToInt32(Session["Alert_U"]) == 1)
            {
                ModelList.Alert = 1;
                Session["Alert_U"] = 0;
                return View(ModelList);
            }
            else
            {
                ModelList.Alert = 0;
                return View(ModelList);
            }
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
            model.Gender = u.Gender;
            model.Permission = u.Permisstion;
            model.Date_of_Started = u.Date_of_Started;
            model.Date_of_Ended = u.Date_of_Ended;
            model.User_Password = u.User_Password;
            if (u.Gender == "M")
            {
                model.Genders = UserModel.Sex.ชาย;
            }
            else if (u.Gender == "F")
            {
                model.Genders = UserModel.Sex.หญิง;
            }
            ////////////////////
            if (u.Speaking == 2)
            {
                model._Speaking = UserModel.Levels.เก่ง;
            }
            else if (u.Speaking == 1)
            {
                model._Speaking = UserModel.Levels.ปานกลาง;
            }
            else if (u.Speaking == 0)
            {
                model._Speaking = UserModel.Levels.อ่อน;
            }
            /////////////////////////////////////////
            if (u.Reading == 2)
            {
                model._Reading = UserModel.Levels.เก่ง;
            }
            else if (u.Reading == 1)
            {
                model._Reading = UserModel.Levels.ปานกลาง;
            }
            else if (u.Reading == 0)
            {
                model._Reading = UserModel.Levels.อ่อน;
            }
            /////////////////////////////////////
            if (u.Writng == 2)
            {
                model._Writng = UserModel.Levels.เก่ง;
            }
            else if (u.Writng == 1)
            {
                model._Writng = UserModel.Levels.ปานกลาง;
            }
            else if (u.Writng == 0)
            {
                model._Writng = UserModel.Levels.อ่อน;
            }
            /////////////////////////////////////
            if (u.Listening == 2)
            {
                model._Listening = UserModel.Levels.เก่ง;
            }
            else if (u.Listening == 1)
            {
                model._Listening = UserModel.Levels.ปานกลาง;
            }
            else if (u.Listening == 0)
            {
                model._Listening = UserModel.Levels.อ่อน;
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
            Language = model.LanguageID,
            Genders = model.Genders,
            Gender = model.Gender,
            Speaking = model._Speaking,
            Reading = model._Reading,
            Writng= model._Writng,
            Listening = model._Listening
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult DeleteUser(int userID)
        {
            try
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
            catch (Exception)
            {
                Session["Alert_U"] = 1;
                return RedirectToAction("ShowUser", "User");
            }
        }
        private bool isValidContentType(string contentType)
        {
            return contentType.Equals("image/png") || contentType.Equals("image/jpg") || contentType.Equals("application/pdf") || contentType.Equals("image/jpeg");
        }

        private bool isValidContentLength(int contentLength)
        {
            return ((contentLength / 1024) / 1024) < 1; // 1MB
        }
        public string Upload_File_User(HttpPostedFileBase File)
        {
            if (!isValidContentType(File.ContentType))
            {
                ViewBag.Error = "เฉพาะไฟล์ jpg png pdf";
                return ("Error");
            }
            else if (!isValidContentLength(File.ContentLength))
            {
                ViewBag.Error = "ไฟล์มีขนาดใหญ่เกินไป (1MB)";
                return ("Error");
            }
            else
            {
                if (File.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(File.FileName);
                    var path = Path.Combine(Server.MapPath("~/Upload/") + fileName);
                    string i = "/Upload/" + fileName + "," + fileName;
                    File.SaveAs(path);
                    ViewBag.fileName = File.FileName;
                    if (File.ContentType.Equals("application/pdf"))
                    {
                        return i;
                    }
                    else

                        return i;
                }
                return ("Error");
            }
        }
    }
    
}