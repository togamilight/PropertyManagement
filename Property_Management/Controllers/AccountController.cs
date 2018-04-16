using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取所有页面的头部导航菜单
        /// </summary>
        /// <returns></returns>
        public ActionResult GetHeader() {
            return PartialView("Header");
        }
    }
}