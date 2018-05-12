using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Property_Management.Util {
    public static class ExceptionLogger {
        public static Queue<Exception> ExceptionQueue = new Queue<Exception>();

        public static void WriteExceptionLog(object filePath) {
            var strfilePath = filePath as string;
            if (strfilePath == null) return;

            while (true) {
                if(ExceptionQueue.Count > 0) {
                    var exception = ExceptionQueue.Dequeue();
                    if(exception != null) {
                        //将异常信息写到日志文件中
                        string fileName = DateTime.Now.ToString("yyyy-MM-dd");
                        File.AppendAllText(strfilePath + fileName + ".txt", exception.ToString(), System.Text.Encoding.UTF8);
                    }
                }else {
                    //队列中没有异常，休息10秒
                    Thread.Sleep(10000);
                }
            }
        }
    }
}