using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

namespace MyLog
{
    class MyLog
    {

        //Needs to be implemented
        //Goal: provide an independent library for capturing, processing, printing and storing logs. (Azure Table, Console, File)

        //public void LogKind(string message, Exception? ex)

        //Logs -> Table(myLogs)

        //Configurable logging level(Ex.full exception messages by default present only in file, table log form).
        //All data output can be configured to output custom data(Ex.Console output does not log inner exception message)
        //MyException - Message, status code, innerException?
        //Calls made using Logger.Level(MyException)
        //Logs output format: StackTrace\t Error message?, Status code?  \n Exception message?

        //consoleTraceListener will always output to console so setting the value here

        static TextWriterTraceListener _consoleTraceListener = new(System.Console.Out);
        static TextWriterTraceListener _fileTraceListener = new();
        public static void ConfigureLogger()
        {

            string getCurrentTimeStamp = DateTime.Now.ToString();
            string logName = "Log-" + getCurrentTimeStamp + ".txt" ;
            File.Create(logName);

            _consoleTraceListener = new TextWriterTraceListener(System.Console.Out);
            _fileTraceListener = new TextWriterTraceListener(logName);
            Trace.Listeners.Add(_consoleTraceListener);
            Trace.Listeners.Add(_fileTraceListener);
        }
    }
}