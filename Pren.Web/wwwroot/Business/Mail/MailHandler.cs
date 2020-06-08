using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using Di.Common.Utils.Context;
using Pren.Web.Business.DataAccess.Logging;
using Di.Common.Utils.Long;
using Pren.Web.Business.Detection;

namespace Pren.Web.Business.Mail
{
    public class MailHandler : IMailHandler
    {
        private const int MaximumAttachementFileSizeInMb = 4;
        private readonly IDetectionHandler _detection;
        private readonly IEmailLogger _emailLogger;

        public MailHandler(IDetectionHandler detection, IEmailLogger emailLogger)
        {
            _detection = detection;
            _emailLogger = emailLogger;
        }

        public void SendMail(string mailFrom, string mailTo, string subject, string body, bool isHtml, string attachmentFileName = null, Stream attachment = null)
        {
            if (!_detection.IsValidEmail(mailFrom) || !_detection.IsValidEmail(mailTo))
                return;

            var result = "OK";

            try
            {
                var message = new MailMessage("no-reply@di.se", mailTo)
                {
                    IsBodyHtml = isHtml,
                    Subject = subject,
                    Body = body
                };

                message.ReplyToList.Add(mailFrom);

                // Max allowed attachment size is 4 MB
                if (attachmentFileName != null && attachment != null && attachment.Length.ConvertBytesToMegabytes() <= MaximumAttachementFileSizeInMb)
                {
                    message.Attachments.Add(new Attachment(attachment, attachmentFileName));
                }

                var smtp = new SmtpClient();
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = ex.ToString();
            }
            finally
            {
                // Result is VarChar(255), so sub it to prevent error if long exception string
                result = result.Length > 250 ? result.Substring(0, 250) : result;
                _emailLogger.LogEmail(result, mailFrom, mailTo, subject, body, isHtml, HttpContextUtils.GetUserIp());
        }
        }

        public string ReplaceMailPlaceHolders(Dictionary<string, string> placeholderDictionary, string bodyText)
        {
            return placeholderDictionary.Aggregate(bodyText,  (current, placeholder) => Regex.Replace(current, "\\" + placeholder.Key, placeholder.Value, RegexOptions.IgnoreCase));
        }
   }
}