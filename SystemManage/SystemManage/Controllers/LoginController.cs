using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;
using SystemManage.Models;

namespace SystemManage.Controllers
{
    public class LoginController : Controller
    {
        Entities db = new Entities();
        // GET: Login
        public ActionResult Index()
        {
            UserModel model = new UserModel();
            Session["userID"] = null;
            Session["ProjectID"] = null;
            return View(model);
        }
        public ActionResult CheckLogin(string User_Email, string User_Password)
        {
            try
            {
                if (!string.IsNullOrEmpty(User_Email) && !string.IsNullOrEmpty(User_Password))
                {
                    var user = db.Users.Where(c => c.User_Email == User_Email && c.User_Password == User_Password).FirstOrDefault();
                    if (user.Permisstion == "A")
                    {
                        if (user != null)
                        {
                            Session["userID"] = user.User_ID;
                            Session["userName"] = user.User_Name;
                            return RedirectToAction("ShowPosition", "Position");
                        }
                    }
                    else if (user.Permisstion == "P" || user.Permisstion == "C")
                    {
                        if (user != null)
                        {
                            Session["userID"] = user.User_ID;
                            Session["userName"] = user.User_Name;
                            return RedirectToAction("ShowProject", "Project");
                        }

                    }
                    else
                    {
                        return PartialView("Index" , new UserModel
                        {
                            User_EmailError = "รหัสผิด",
                            User_PasswordError = "รหัสผิด",
                        });
                    }
                }
                return View("Index");
            }
            catch (Exception)
            {
                return PartialView("Index" , new UserModel
                {
                    User_EmailError = "รหัสผิด",
                    User_PasswordError = "รหัสผิด",
                });
            }
        }
        
    }
}