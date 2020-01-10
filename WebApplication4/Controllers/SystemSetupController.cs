using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class SystemSetupController : Controller
    {
        SystemDataBaseEntities1 db = new SystemDataBaseEntities1();
        
        // GET: SystemSetup
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserMaintain()
        {
            return RedirectToAction("Index", "UserMaintenance");
        }

        public ActionResult RoleMaintain()
        {
            return RedirectToAction("Index", "RoleMaintenance");
        }

        public ActionResult MenuMaintain()
        {
            return RedirectToAction("Index", "MenuMaintenance");
        }
    }
}