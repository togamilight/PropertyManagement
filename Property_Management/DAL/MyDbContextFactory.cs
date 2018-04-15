using System;
using System.Web;

namespace Property_Management.DAL {
    public static class MyDbContextFactory {
        /// <summary>
        /// 创建并获取MyDbContext实例，保存在HttpContext.Current.Items中，保证线程中唯一
        /// </summary>
        /// <returns></returns>
        public static MyDbContext GetMyDbContext() {
            MyDbContext dbContext = HttpContext.Current.Items["MyDbContext"] as MyDbContext;
            if(dbContext == null) {
                dbContext = new MyDbContext();
                HttpContext.Current.Items["MyDbContext"] = dbContext;
            }

            return dbContext;
        }

        /// <summary>
        /// 释放HttpContext.Current.Items中的MyDbContext实例
        /// </summary>
        public static void DisposeMyDbContext() {
            MyDbContext dbContext = HttpContext.Current.Items["MyDbContext"] as MyDbContext;
            if (dbContext != null) {
                dbContext.Dispose();
                HttpContext.Current.Items["MyDbContext"] = null;
            }
        }
    }
}