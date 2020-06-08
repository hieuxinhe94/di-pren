namespace Pren.Web.Business.DataAccess.Logging
{
    public interface IEmailLogger
    {
        void LogEmail(string result, string mailFrom, string mailTo, string subject, string body, bool isHtml, string ip);
    }
}
