using System;
using Di.Common.Logging;
using Pren.Web.Business.Data;

namespace Pren.Web.Business.DataAccess.Logging
{
    // ReSharper disable EmptyGeneralCatchClause
    public class EntityLogger : ILogger
    {
        public void Log(string description, LogLevel level, Type type)
        {
            if (level.Equals(LogLevel.Info))
            {
                LogInfo(description, level, type);   
            }
            else
            {
                LogError(string.Empty, description, level, type);
            }
        }

        public void Log(Exception exception, string description, LogLevel level, Type type)
        {
            var logInfo = exception.Data["loginfo"] ?? string.Empty;

            LogError(logInfo + exception.ToString(), description, level, type);
        }

        private void LogInfo(string description, LogLevel level, Type type)
        {
            try
            {
                using (var dbContext = new PrenWebMiscEntities())
                {
                    dbContext.LogTableInfo.Add(new LogTableInfo
                    {
                        fileName = level + " - " + type,
                        info = description,
                        date = DateTime.Now
                    });

                    dbContext.SaveChanges();
                }
            }
            catch
            {
                // Do nothing
            }
        }

        private void LogError(string exception, string description, LogLevel level, Type type)
        {
            try
            {
                using (var dbContext = new PrenWebMiscEntities())
                {
                    dbContext.LogTable.Add(new LogTable
                    {
                        description = description,
                        errorFileName = level + " - " + type,
                        exception = exception,
                        date = DateTime.Now
                    });

                    dbContext.SaveChanges();
                }
            }
            catch
            {
                // Do nothing
            }
        }
    }
}