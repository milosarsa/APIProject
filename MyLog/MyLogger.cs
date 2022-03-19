using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MyLog;

namespace MyLog
{
    public class MyLogger : IMyLogger
    {
        public MyLogger()
        {
            //Set up and configure for data output
        }

        public void Info(Log log)
        {
            _ = DoLogAsync(log);
        }

        public void Warning(Log log)
        {
            _ = DoLogAsync(log, LogType.Warning);
        }
        public void Error(Log log)
        {
            _ = DoLogAsync(log, LogType.Error);
        }


        //This is ment to be ran without await, need to run this independently and there is no need to wait for a response as it's printing it :)
        async Task DoLogAsync(Log log, LogType logType = 0)
        {
            await Task.Run(() =>
            {
            Trace.WriteLine("Test");
            Trace.Write(log.ToString());
            Trace.Write(log.ToString());
            });
            Thread.Yield();
        }

    }
}
