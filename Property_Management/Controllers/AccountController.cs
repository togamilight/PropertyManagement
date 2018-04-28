using Property_Management.BLL.Service;
using Property_Management.DAL.Entities;
using Property_Management.Filters;
using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Controllers {
    public class AccountController : Controller {
        // GET: Account
        public ActionResult Index() {
            return View();
        }

        /// <summary>
        /// 获取所有页面的头部导航菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetHeader() {
            return PartialView("Header");
        }

        public ActionResult AdminLogin() {
            //TODO 如果已登录，跳转到首页

            return View();
        }

        public ActionResult OwnerLogin() {
            //TODO 如果已登录，跳转到首页

            return View();
        }

        [JsonExceptionFilter]
        public ActionResult DoAdminLogin(Admin admin) {
            var adminService = new AdminService();
            admin.Id = adminService.CheckAccount(admin);
            if (admin.Id == 0) {
                return Json(new ResultInfo(false, "用户名或密码错误", null));
            }

            Session["Account"] = new AccountInfo() {
                Id = admin.Id,
                Name = admin.Name,
                IsAdmin = true
            };
            return Json(new ResultInfo(true, "登录成功", null));

        }

        [JsonExceptionFilter]
        public ActionResult DoOwnerLogin(Owner owner) {
            var ownerService = new OwnerService();
            owner.Id = ownerService.CheckAccount(owner);
            if (owner.Id == 0) {
                return Json(new ResultInfo(false, "用户名或密码错误", null));
            }

            Session["Account"] = new AccountInfo() {
                Id = owner.Id,
                Name = owner.Name,
                IsAdmin = false
            };
            return Json(new ResultInfo(true, "登录成功", null));

        }
    }
}