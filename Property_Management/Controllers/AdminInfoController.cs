using Property_Management.BLL.Service;
using Property_Management.DAL.Entities;
using Property_Management.Filters;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Controllers
{
    [AccountFilter("Admin")]
    public class AdminInfoController : Controller
    {
        // GET: AdminInfo
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword() {
            return View();
        }

        public ActionResult AdminSignUp() {
            return View();
        }

        [JsonExceptionFilter]
        public ActionResult DoChangePassword(string oldPassword, string newPassword ) {
            var service = new AdminService();

            var account = Session["Account"] as AccountInfo;
            var admin = new Admin() {
                Name = account.Name,
                Password = oldPassword
            };
            return Json(service.ChangePassword(admin, newPassword));
        }

        [JsonExceptionFilter]
        public ActionResult DoAdminSignUp(Admin admin) {
            var service = new AdminService();

            return Json(service.Add(admin));
        }

        public ActionResult LogOut() {
            Session["Account"] = null;
            return RedirectToAction("AdminLogin", "Account");
        }
    }
}