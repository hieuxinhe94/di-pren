using System;
using log4net;

namespace Di.Common.Logging
{
    public class Log4NetLogger : ILogger
    {
        public void Log(string message)
        {
            var logManager = LogManager.GetLogger("Di.ServicePlus");
            logManager.Error(message);
        }
        
        public void Log(string message, LogLevel level, Type type)
        {
            Log(null, message, level, type);
        }

        public void Log(Exception exception, string message, LogLevel level, Type type)
        {
            var logger = type == null ? LogManager.GetLogger("Di.ServicePlus") : LogManager.GetLogger(type);

            switch (level)
            {
                case LogLevel.Fatal:
                    logger.Fatal(message, exception);
                    break;
                case LogLevel.Error:
                    logger.Error(message, exception);
                    break;
                case LogLevel.Warn:
                    logger.Warn(message, exception);
                    break;
                case LogLevel.Info:
                    logger.Info(message, exception);
                    break;
                case LogLevel.Debug:
                    logger.Debug(message, exception);
                    break;
            }
        }
    }
}
