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
        public ActionResult CheckLogin(string User_Email, string User_Password)
        {
            if(!string.IsNullOrEmpty(User_Email) && !string.IsNullOrEmpty(User_Password))
            {
                var user = db.Users.Where(c => c.User_Email == User_Email && c.User_Password == User_Password).FirstOrDefault();
                if (user != null)
                {
                    Session["userName"] = user.User_Email;
                    return RedirectToAction("Index", "Languages"); 
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