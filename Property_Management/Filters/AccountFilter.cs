using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Filters {
    public class AccountFilter: ActionFilterAttribute {
        public AccountFilter(string checkType): base() {
            CheckType = checkType;
        }
        public AccountFilter() : base() { }

        public string CheckType { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext) {
            var account = filterContext.HttpContext.Session["Account"] as AccountInfo;

            bool flag = false;
            if (account == null) {
                flag = true;
                //TODO 跳转到用户登录页面
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "MyAdmin", action = "Login", message = "请先登录" }));
            }
            else if(CheckType == "Admin") {
                if (!account.IsAdmin) {
                    //TODO 跳转到您无权限页面
                }
            }else if(CheckType == "Owner") {
                if (account.IsAdmin) {
                    //TODO 跳转到用户登录页面
                }
            }
        }
    }
}