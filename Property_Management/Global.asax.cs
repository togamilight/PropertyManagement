using Property_Management.App_Start;
using Property_Management.DAL;
using Property_Management.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Property_Management
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);

            //启用线程来写异常日志
            string filePath = Server.MapPath("/Log/");
            var logThread = new Thread(new ParameterizedThreadStart(ExceptionLogger.WriteExceptionLog));
            logThread.Start(filePath);
        }

        void Application_EndRequest(object sender, EventArgs e) {
            //EndRequest是在响应Request时最后一个触发的事件
            //但在对象被释放或者从新建立以前，适合在这个时候清理代码
            MyDbContextFactory.DisposeMyDbContext();
        }
    }
}
