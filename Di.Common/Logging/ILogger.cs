using System;

namespace Di.Common.Logging
{
    public interface ILogger
    {
        void Log(string message, LogLevel level, Type type);
        void Log(Exception exception, string message, LogLevel level, Type type);        
    }
}
