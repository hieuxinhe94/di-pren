using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using Di.Common.Logging;
using Di.Common.Utils.Context;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.Session;
using Pren.Web.Business.Subscription;

namespace Pren.Web.Controllers.ApiControllers.MySettings
{
    public class LoggingController : ExtendedApiController
    {
        private readonly ILogger _logger;
        private readonly ISessionData _sessionData;

        public LoggingController(IApiReferrerCheck apiReferrerCheck, ISessionData sessionData) 
            : base(apiReferrerCheck)
        {
            _sessionData = sessionData;
            _logger = new Log4NetLogger();
        }

        [HttpGet]
        public HttpResponseMessage Log(string error, string url, string line, string col, string extra)
        {
            var subscriber = (Subscriber)_sessionData.Get(SessionConstants.SubscriberSessionKey);

            var logString = new StringBuilder();

            if (subscriber != null)
            {
                logString.Append("S+ Email : " + subscriber.ServicePlusUser.Email + " ");
            }
            else
            {
                logString.Append("No subscriber in session ");
            }

            logString.Append(new UserContext().ToLogString() + " ");

            logString.Append("Error: " + error + " Url: " + url + " line: " + line + " col: " + col);

            _logger.Log(logString.ToString(), LogLevel.Error, typeof(LoggingController));
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
