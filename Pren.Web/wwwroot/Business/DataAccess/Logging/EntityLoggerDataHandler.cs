using System;
using System.Collections.Generic;
using System.Linq;
using Pren.Web.Business.Data;
using Pren.Web.Business.DataAccess.Logging.Entities;

namespace Pren.Web.Business.DataAccess.Logging
{
    public class EntityLoggerDataHandler : EntityDataHandlerBase, ILoggerDataHandler
    {
        public IEnumerable<LogEntity> GetLogEntites(DateTime fromDate, DateTime toDate, string source, string description, string exception)
        {
            return GetEntities<IEnumerable<LogEntity>>(
                dbContext =>
                {
                    var logEntities = dbContext.LogTable.Where(log => log.date >= fromDate && log.date <= toDate);

                    if (!string.IsNullOrEmpty(source))
                    {
                        logEntities = logEntities.Where(log => log.errorFileName.Contains(source));
                    }

                    if (!string.IsNullOrEmpty(description))
                    {
                        logEntities = logEntities.Where(log => log.description.Contains(description));
                    }

                    if (!string.IsNullOrEmpty(exception))
                    {
                        logEntities = logEntities.Where(log => log.exception.Contains(exception));
                    }

                    return logEntities.Select(CreateLogEntity).OrderByDescending(log => log.Date).ToList();

                });
        }

        public IEnumerable<LogEmailEntity> GetLogEmailEntites(DateTime fromDate, DateTime toDate, string fromAddress, string toAddress, string subject, string message)
        {
            return GetEntities<IEnumerable<LogEmailEntity>>(
                dbContext =>
                {
                    var logEntities = dbContext.EmailLog.Where(log => log.SendTime >= fromDate && log.SendTime <= toDate);


                    if (!string.IsNullOrEmpty(toAddress))
                    {
                        logEntities = logEntities.Where(log => log.ToAddress.Contains(toAddress));
                    }

                    if (!string.IsNullOrEmpty(fromAddress))
                    {
                        logEntities = logEntities.Where(log => log.FromAddress.Contains(fromAddress));
                    }

                    if (!string.IsNullOrEmpty(subject))
                    {
                        logEntities = logEntities.Where(log => log.Subject.Contains(subject));
                    }

                    if (!string.IsNullOrEmpty(message))
                    {
                        logEntities = logEntities.Where(log => log.Body.Contains(message));
                    }

                    return logEntities.Select(CreateEmailEntity).OrderByDescending(log => log.SendDate).ToList();

                });
        }

        public IEnumerable<LogInfoEntity> GetLogInfoEntites(DateTime fromDate, DateTime toDate, string source, string description)
        {
            return GetEntities<IEnumerable<LogInfoEntity>>(
                dbContext =>
                {
                    var logEntities = dbContext.LogTableInfo.Where(log => log.date >= fromDate && log.date <= toDate);


                    if (!string.IsNullOrEmpty(source))
                    {
                        logEntities = logEntities.Where(log => log.fileName.Contains(source));
                    }

                    if (!string.IsNullOrEmpty(description))
                    {
                        logEntities = logEntities.Where(log => log.info.Contains(description));
                    }

                    return logEntities.Select(CreateInfoLogEntity).OrderByDescending(log => log.Date).ToList();

                });
        }

        private LogEntity CreateLogEntity(LogTable log)
        {
            return new LogEntity
            {
                Id = log.id,
                Date = log.date,
                Description = log.description,
                Source = log.errorFileName,
                Exception = log.exception
            };
        }

        private LogInfoEntity CreateInfoLogEntity(LogTableInfo log)
        {
            return new LogInfoEntity
            {
                Id = log.id,
                Date = log.date,
                Description = log.info,
                Source = log.fileName,
            };
        }

        private LogEmailEntity CreateEmailEntity(EmailLog log)
        {
            return new LogEmailEntity
            {
                Id = log.Id,
                FromAddress = log.FromAddress,
                ToAddress = log.ToAddress,
                Ip = log.IP,
                Message = log.Body,
                Subject = log.Subject,
                Result = log.Result,
                SendDate = log.SendTime
            };
        }
    }
}