using Property_Management.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Filters {
    [AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
    public class ExceptionFilter : HandleErrorAttribute {
        public override void OnException(ExceptionContext filterContext) {
            //当异常未处理时进行处理
            if (!filterContext.ExceptionHandled) {
                //string controllerName = filterContext.RouteData.Values["controller"].ToString();
                //string actionName = filterContext.RouteData.Values["action"].ToString();
                //将异常插入队列
                ExceptionLogger.ExceptionQueue.Enqueue(filterContext.Exception);
                //当结果是JSON时，不用跳转页面，将其设为已处理
                if(filterContext.Result is JsonResult) {
                    filterContext.ExceptionHandled = true;
                }
            }

            base.OnException(filterContext);
        }
    }
}