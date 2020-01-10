using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class LoginController : Controller
    {

        //LoginDataBaseEntities db = new LoginDataBaseEntities();
        SystemDataBaseEntities1 db = new SystemDataBaseEntities1();
        // GET: Login
        public ActionResult Index()
        {
            HttpCookie cookie = Request.Cookies["LoginInfo"];
            if (cookie != null) {
                ViewBag.LoginName = cookie["LoginName"].ToString();
                ViewBag.LoginPassword = cookie["LoginPassword"].ToString();
            }
            return View();
        }

        

        [HttpPost]
        public ActionResult Autherize(WebApplication4.Models.LoginInfo loginModel)
        {
            HttpCookie cookie = new HttpCookie("LoginInfo");
            
                if (ModelState.IsValid == true)
                {
                    if (loginModel.Remember == true)
                    {
                        cookie["LoginName"] = loginModel.LoginName;
                        cookie["LoginPassword"] = loginModel.LoginPassword;
                        cookie.Expires = DateTime.Now.AddDays(2);
                        HttpContext.Response.Cookies.Add(cookie);
                    }
                    else {
                        cookie.Expires = DateTime.Now.AddDays(-1);
                        HttpContext.Response.Cookies.Add(cookie);
                    }

                var loginDetails = db.LoginInfoes.Where(x => x.LoginName == loginModel.LoginName && x.LoginPassword == loginModel.LoginPassword).FirstOrDefault();
                if (loginDetails != null)
                {
                    Session["LoginID"] = loginDetails.LoginID;
                    Session["LoginName"] = loginDetails.LoginName;
                    int important = loginDetails.UserID; 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    loginModel.LoginErrorMessage = "Wrong username or password.";
                }
            }
            return View("Index", loginModel);
        }
    }
}