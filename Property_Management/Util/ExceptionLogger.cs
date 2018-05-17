using Property_Management.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;

namespace Property_Management.Util {
    public static class ExceptionLogger {
        public static Queue<ExceptionInfo> ExceptionQueue = new Queue<ExceptionInfo>();

        public static void WriteExceptionLog(object filePath) {
            var strfilePath = filePath as string;
            if (strfilePath == null) return;

            while (true) {
                if(ExceptionQueue.Count > 0) {
                    var exceptionInfo = ExceptionQueue.Dequeue();
                    if(exceptionInfo != null) {
                        //将异常信息写到日志文件中
                        string fileName = exceptionInfo.DateTime.ToString("yyyy-MM-dd");
                        string fullFileName = strfilePath + fileName + ".txt";
                        //作为醒目标识，便于查看
                        string strHead =
                            Environment.NewLine + $"---------------------------{exceptionInfo.DateTime.ToString("yyyy-MM-dd hh:MM:ss")} 于 {exceptionInfo.ControllerName} 控制器的 {exceptionInfo.ActionName} 方法中产生异常 Start---------------------------"  + Environment.NewLine;

                        string fullInfo = strHead + exceptionInfo.Exception.ToString() + strHead.Replace("Start", "End");

                        File.AppendAllText(fullFileName, fullInfo, System.Text.Encoding.UTF8);
                    }
                }else {
                    //队列中没有异常，休息10秒
                    Thread.Sleep(10000);
                }
            }
        }
    }
}