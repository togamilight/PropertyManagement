using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

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
            }else {
                switch (CheckType) {
                    case "Admin":
                        if (!account.IsAdmin) {
                            flag = true;
                        }
                        break;
                    case "Owner":
                        if (account.IsAdmin) {
                            flag = true;
                        }
                        break;
                    case "Both":
                        flag = false;
                        break;
                }
            }

            //验证不通过，跳转到登录页面
            if (flag) {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "OwnerLogin", message = "请先登录" }));
            }
        }
    }
}