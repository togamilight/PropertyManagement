using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Property_Management.Filters {
    /// <summary>
    /// 处理获取Json数据的Action所产生的异常并返回Json结果
    /// </summary>
    public class JsonExceptionFilter : HandleErrorAttribute {
        public override void OnException(ExceptionContext filterContext) {
            //base.OnException(filterContext);
            //表示已处理异常，MVC引擎不需要再次处理，不会显示默认错误页面
            //filterContext.ExceptionHandled = true;

            var result = new ResultInfo(false, filterContext.Exception.Message, null);
            filterContext.Result = new JsonResult() { Data = result };
        }
    }
}