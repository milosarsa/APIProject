using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLog
{
    public record class Log
    {
        public string? Message { get; set; }
        public int? StatusCode { get; set; }
        public Exception? InnerException { get; set; }

        public Log()
        {
        }

        public Log(string? message)
        {
            Message = message;
        }
        public Log(string? message, int? statusCode)
        {
            Message = message;
            StatusCode = statusCode;
        }
        public Log(string? message, Exception? exception)
        {
            Message = message;
            InnerException = exception;
        }
        public Log(int? statusCode, Exception? exception)
        {
            StatusCode = statusCode;
            InnerException = exception;
        }
        public Log(string? message, int? statusCode, Exception? exception)
        {
            Message = message;
            StatusCode = statusCode;
            InnerException = exception;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if(Message != null)
                builder.Append(Message + "\n");
            if (StatusCode != null)
                builder.Append(StatusCode + "\n");
            if (InnerException != null)
                builder.Append(InnerException + "\n");

            return builder.ToString();
        }

    }

    public enum LogType
    {
        Info,
        Warning,
        Error
    }
}
