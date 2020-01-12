using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication4.Models;

namespace WebApplication4.Controllers
{
    public class RoleMaintenanceController : Controller
    {
        SystemDataBaseEntities1 db = new SystemDataBaseEntities1();
        HomeController hc = new HomeController();
        // GET: RoleMaintenance
        public ActionResult Index()
        {
            var roles = db.Roles.ToList();
            var roles2 = db.Roles.ToList();
            List<Role> list = new List<Role>();
            var viewModel = new Role();
            viewModel.ListA = roles;
            viewModel.ListB = list;
            //System.Diagnostics.Debug.WriteLine(roles);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult function(FormCollection form)
        {
            var button = form["button"];
            var roles = db.Roles.ToList();
            var roles2 = db.Roles.ToList();
            var viewModel = new Role();
            var selectRole = form["SelectRole"];
            int loginID = (int)Session["LoginID"];
            var userInfo = db.Users.Where(l => l.UserID == loginID).FirstOrDefault();
            string userName = userInfo.UserName;

            ViewBag.UserNmae = userName;

            if (button == "Query")
            {

                if (selectRole == "All")
                {
                    viewModel.ListA = roles;
                    viewModel.ListB = roles2;
                }
                else
                {
                    roles2 = db.Roles.Where(r => r.RoleName == selectRole).ToList();
                    viewModel.ListA = roles;
                    viewModel.ListB = roles2;
                }
                return View("Index", viewModel);
            }
            else if (button == "Add")
            {
                return View("Add");
            }
            else if (button == "Modify")
            {
                viewModel.ListA = roles;
                ArrayList checkList = new ArrayList();
                roles2.Clear();
                var checkRoleAll = form["checkAll"];
                foreach (var role in roles)
                {
                    var checkRole = form[role.RoleID.ToString()];
                    if (checkRole == "on")
                    {
                        roles2.Add(db.Roles.Where(r => r.RoleID == role.RoleID).FirstOrDefault());
                    }
                }
                System.Diagnostics.Debug.WriteLine(roles2.Count());
                if (roles2.Count() == 1)
                {
                    viewModel.ListB = roles2;
                    return View("Modify", viewModel);
                }
                else if (roles2.Count() == 0)
                {
                    viewModel.ListB = roles2;
                    TempData["SelectionMessage"] = "Plese select atleast one role to modify.";
                    return View("Index", viewModel);
                }
                else
                {
                    roles2 = db.Roles.ToList();
                    viewModel.ListB = roles2;
                    TempData["SelectionMessage"] = "Plese modify one role at a time.";
                    return View("Index", viewModel);
                }
            }
            else if (button == "Menu")
            {
                List<Menu> MenuList = new List<Menu>();
                MenuList = db.Menus.ToList();

                viewModel.ListA = roles;
                roles2.Clear();
                var checkRoleAll = form["checkAll"];
                foreach (var role in roles)
                {
                    var RoleTest = viewModel.Menus.ToList();

                    
                    var checkRole = form[role.RoleID.ToString()];
                    if (checkRole == "on")
                    {
                        roles2.Add(db.Roles.Where(r => r.RoleID == role.RoleID).FirstOrDefault());
                    }
                }
                if (roles2.Count() == 1)
                {
                    var RoleTest = roles2.First().Menus.ToList();
                    viewModel.ListB = roles2;
                    viewModel.ListMenu = RoleTest;
                    viewModel.AllMenu = MenuList; 

                    return View("Menu", viewModel);
                }
                else if (roles2.Count() == 0)
                {
                    viewModel.ListB = roles2;
                    TempData["SelectionMessage"] = "Plese atleast one role to modify its menu list.";
                    return View("Index", viewModel);
                }
                else
                {
                    roles2 = db.Roles.ToList();
                    viewModel.ListB = roles2;
                    TempData["SelectionMessage"] = "Plese modify one role's menu list at a time.";
                    return View("Index", viewModel);
                }
            }
            else if (button == "Delete")
            {
                viewModel.ListA = roles;
                ArrayList checkList = new ArrayList();
                roles2.Clear();
                var checkRoleAll = form["checkAll"];
                Role dropRole = new Role();
                foreach (var role in roles)
                {
                    var checkRole = form[role.RoleID.ToString()];
                    if (checkRole == "on")
                    {
                        dropRole = db.Roles.Where(r => r.RoleID == role.RoleID).FirstOrDefault();
                        db.Roles.Remove(dropRole);
                        db.SaveChanges();
                    }               
                }
                //roles2 = db.Roles.ToList();
                if (roles2.Count() == 0)
                {
                    viewModel.ListB = roles2;
                    return View("Index", viewModel);
                }
                else
                {
                    db.SaveChanges();
                    roles = db.Roles.ToList();
                    roles2 = db.Roles.ToList();
                    viewModel.ListA = roles;
                    viewModel.ListB = roles2;
                    return View("Index", viewModel);
                }
            }
            return View("Index", viewModel);
        }

        public ActionResult Add(FormCollection form, Role role)
        {
            var roles = db.Roles.ToList();
            var roles2 = db.Roles.ToList();
            var viewModel = new Role();
            viewModel.ListA = roles;
            viewModel.ListB = roles2;

            int loginID = (int)Session["LoginID"];
            var userInfo = db.Users.Where(l => l.UserID == loginID).FirstOrDefault();
            string userName = userInfo.UserName;

            var selectStatus = form["SelectStatus"];
            if (selectStatus == "Y")
            {
                role.Status = 1;
            }
            else { role.Status = 0; }
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.Date.ToString("yyyy-MM-dd HH:mm:ss");
            role.CreateDate = Convert.ToDateTime(sqlFormattedDate);
            role.CreateUser = userName;
            role.ModifyDate = Convert.ToDateTime(sqlFormattedDate);
            role.ModifyUser = userName;

            db.Roles.Add(role);
            db.SaveChanges();

            roles = db.Roles.ToList();
            roles2 = db.Roles.ToList();
            viewModel.ListA = roles;
            viewModel.ListB = roles2;

            return View("Index", viewModel);
        }

        public ActionResult Modify(FormCollection form, Role role)
        {
            var roles = db.Roles.ToList();
            var roles2 = db.Roles.ToList();
            var viewModel = new Role();
            viewModel.ListA = roles;
            viewModel.ListB = roles2;
            String roleName = form["Origin-Role-Name"];
            int loginID = (int)Session["LoginID"];

            var userInfo = db.Users.Where(l => l.UserID == loginID).FirstOrDefault();
            string userName = userInfo.UserName;

            Role Role_to_update = db.Roles.SingleOrDefault(r => r.RoleName == roleName);

            Role_to_update.RoleName = roleName;
            Role_to_update.RoleDescription = role.RoleDescription;
            var selectStatus = form["SelectStatus"];
            if (selectStatus == "Y")
            {
                Role_to_update.Status = 1;
            }
            else { Role_to_update.Status = 0; }
            DateTime myDateTime = DateTime.Now;
            string sqlFormattedDate = myDateTime.Date.ToString("yyyy-MM-dd HH:mm:ss");
            Role_to_update.ModifyDate = Convert.ToDateTime(sqlFormattedDate);
            Role_to_update.ModifyUser = userName;
            db.SaveChanges();

            roles = db.Roles.ToList();
            roles2 = db.Roles.ToList();
            viewModel.ListA = roles;
            viewModel.ListB = roles2;

            return View("Index", viewModel);
        }

        public ActionResult Menu(FormCollection form, Role role)
        {
            var roles = db.Roles.ToList();
            var roles2 = db.Roles.ToList();
            var viewModel = new Role();
            viewModel.ListA = roles;
            viewModel.ListB = roles2;
            List<Menu> MenuList = new List<Menu>();
            MenuList = db.Menus.ToList();

            String roleName = form["Origin-Role-Name"];

            Role select_role = db.Roles.SingleOrDefault(r => r.RoleName == roleName);

            var Role_Menu = select_role.Menus.ToList();

            Menu saveRoleMenu = new Menu();
            Role saveMenuRole = new Role();
            foreach (var menu in Role_Menu)
            {
                var checkMenu_delete = form[menu.MenuNo];
                if (checkMenu_delete == null)
                {
                    saveRoleMenu = db.Roles.Where(r => r.RoleName == roleName).FirstOrDefault().Menus.Where(m => m.MenuNo == menu.MenuNo).FirstOrDefault();
                    db.Roles.Where(r => r.RoleName == roleName).FirstOrDefault().Menus.Remove(saveRoleMenu);
                    db.SaveChanges();
                } 
            }
            foreach (var menu in MenuList) {
                var checkMenu_add = form[menu.MenuNo];
                if (checkMenu_add == "on") {
                    System.Diagnostics.Debug.WriteLine(menu.Roles.Count);
                    if (!db.Menus.Where(m => m.MenuNo == menu.MenuNo).FirstOrDefault().Roles.Contains(select_role))
                    {
                        db.Menus.Where(m => m.MenuNo == menu.MenuNo).FirstOrDefault().Roles.Add(select_role);
                        db.SaveChanges();
                        System.Diagnostics.Debug.WriteLine(db.Menus.Where(m => m.MenuNo == menu.MenuNo).FirstOrDefault().LinkName);
                        System.Diagnostics.Debug.WriteLine(select_role.RoleName);
                    }
                }
            }

            return View("Index", viewModel);
        }
    }
}