using Microsoft.Extensions.Logging;

namespace MyLogManager
{
    public class MyLogger
    {
        //Needs to be implemented
        //Goal: provide an independent library for capturing, processing, printing and storing logs. (Azure Table, Console, File)

        //purlic void LogKind(string message, Exception? ex)

        //Logs -> Queue -> Table (myLogs)
        public void Info()
        {

        }
        public void Warning()
        {

        }

        public void Error()
        {

        }

    }
}