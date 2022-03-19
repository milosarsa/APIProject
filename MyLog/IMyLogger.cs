namespace MyLog
{
    public interface IMyLogger
    {
        public void Info(Log log);
        public void Info(string? message) => Info(new Log(message));
        public void Info(string? message, int? StatusCode) => Info(new Log(message, StatusCode));
        public void Info(string? message, Exception? exception) => Info(new Log(message, exception));
        public void Info(int? StatusCode, Exception? exception) => Info(new Log(StatusCode, exception));
        public void Info(string? message, int? StatusCode, Exception? exception) => Info(new Log(message, StatusCode, exception));

        public void Warning(Log log);
        public void Warning(string? message) => Warning(new Log(message));
        public void Warning(string? message, int? StatusCode) => Warning(new Log(message, StatusCode));
        public void Warning(string? message, Exception? exception) => Warning(new Log(message, exception));
        public void Warning(int? StatusCode, Exception? exception) => Warning(new Log(StatusCode, exception));
        public void Warning(string? message, int? StatusCode, Exception? exception) => Warning(new Log(message, StatusCode, exception));


        public void Error(Log log);
        public void Error(string? message, int? StatusCode) => Error(new Log(message, StatusCode));
        public void Error(string? message, Exception? exception) => Error(new Log(message, exception));
        public void Error(int? StatusCode, Exception? exception) => Error(new Log(StatusCode, exception));
        public void Error(string? message, int? StatusCode, Exception? exception) => Error(new Log(message, StatusCode, exception));
    }
}
