using System;
using System.Collections.Generic;
using Pren.Web.Business.DataAccess.Logging.Entities;

namespace Pren.Web.Business.DataAccess.Logging
{
    interface ILoggerDataHandler
    {
        IEnumerable<LogEntity> GetLogEntites(DateTime fromDate, DateTime toDate, string source, string description, string exception);

        IEnumerable<LogEmailEntity> GetLogEmailEntites(DateTime fromDate, DateTime toDate, string fromAddress,
            string toAddress, string subject, string message);

        IEnumerable<LogInfoEntity> GetLogInfoEntites(DateTime fromDate, DateTime toDate, string source,
            string description);
    }
}
