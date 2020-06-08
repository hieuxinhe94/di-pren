
using System.Collections.Generic;
using System.IO;

namespace Pren.Web.Business.Mail
{
    public interface IMailHandler
    {
        string ReplaceMailPlaceHolders(Dictionary<string, string> placeholderDictionary, string bodyText);

        void SendMail(string mailFrom, string mailTo, string subject, string body, bool isHtml,
            string attachmentFileName = null, Stream attachment = null);
    }
}