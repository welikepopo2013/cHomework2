using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class HomeController : Controller
    {
        
        // GET: Home
        public ActionResult Index()
        {
            if (Session["LoginID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
        }

        public ActionResult ReturnHome()
        {
            return View("Index");
        }

        public ActionResult ChangePassword()
        {
            return View("ChangePassword");
        }

        

        public ActionResult Logout()
        {
            int loginID = (int)Session["LoginID"];
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }


        [HttpPost]
        public ActionResult ModifyPassword(LoginInfo loginModel)
        {
            string LoginName = (string)Session["LoginName"];
            SystemDataBaseEntities1 db = new SystemDataBaseEntities1();
            var loginDetails = db.LoginInfoes.Where(x => x.LoginName == LoginName && x.LoginPassword == loginModel.LoginPassword).FirstOrDefault();
            if (loginDetails != null) {
                if (loginModel.NewPassword1 != null) {
                    if (loginModel.LoginPassword != loginModel.NewPassword1) {
                        if (loginModel.NewPassword1 == loginModel.NewPassword2) {
                            LoginInfo LoginInfo_to_update = db.LoginInfoes.SingleOrDefault(u => u.LoginName == LoginName);
                            LoginInfo_to_update.LoginPassword = loginModel.NewPassword1;
                            db.SaveChanges();
                            loginModel.ModifyMessage = "Password Changed";
                            return View("ChangePassword", loginModel);
                        } else {
                            loginModel.ModifyMessage = "New Password doesn't match";
                        }
                    } else {
                        loginModel.ModifyMessage = "Please enter a different password";
                    }
                } else {
                    loginModel.ModifyMessage = "New Password is required";
                }
            } else {
                loginModel.ModifyMessage = "Wrong password.";
            }

            return View("ChangePassword", loginModel);
        }

        public string LoginUser()
        {
            string LoginName = (string)Session["UserName"];
            return LoginName;
        }
    }


}