using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SystemManage.Database;

namespace SystemManage.Controllers
{
    public class LoginController : Controller
    {
        Entities db = new Entities();
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CheckLogin(string User_Email, string User_Password,string LoginErrorMessage)
        {
            if(!string.IsNullOrEmpty(User_Email) && !string.IsNullOrEmpty(User_Password))
            {
                var user = db.Users.Where(c => c.User_Email == User_Email && c.User_Password == User_Password).FirstOrDefault();
                if (user != null)
                {
                    Session["UserName"] = user.User_Email;       
                    Session["Fullname"] = user.User_Name + " " + user.User_LastName;       
                    return RedirectToAction("Index","Member");
                }
            }
            return View("Index");
        }
        public ActionResult logOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "Login");

        }
    }
}