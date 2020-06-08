using System;
using Pren.Web.Business.Data;

namespace Pren.Web.Business.DataAccess.Logging
{
    class EntityEmailLogger : IEmailLogger
    {
        public void LogEmail(string result, string mailFrom, string mailTo, string subject, string body, bool isHtml, string ip)
        {
            //Anonymize, 
            mailFrom = "from address";
            body = "body";

            using (var prenWebMisc = new PrenWebMiscEntities())
            {
                prenWebMisc.EmailLog.Add(new EmailLog
                {
                    ToAddress = mailTo,
                    FromAddress = mailFrom,
                    Subject = subject,
                    Body = body,
                    IsHtml = isHtml ? 1 : 0,
                    IP = ip,
                    Result = result,
                    SendTime = DateTime.Now
                });
                prenWebMisc.SaveChanges();
            }
        }
    }
}