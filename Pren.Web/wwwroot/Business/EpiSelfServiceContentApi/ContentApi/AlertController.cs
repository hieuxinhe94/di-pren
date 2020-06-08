using System;
using System.Net;
using System.Net.Http;
using Bn.SelfService.ContentApi;
using Di.Common.Logging;

namespace Pren.Web.Business.EpiSelfServiceContentApi.ContentApi
{
    public class AlertController : ContentControllerBase
    {
        private readonly ISelfServiceContentService _selfServiceContentService;
        private readonly ILogger _logger;

        public AlertController(
            ISelfServiceContentService selfServiceContentService,
            ILogger logger)
        {
            _selfServiceContentService = selfServiceContentService;
            _logger = logger;
        }

        public HttpResponseMessage GetAlerts(string brand)
        {
            try
            {
                var alerts = _selfServiceContentService.GetAlerts(brand);

                return CreateResponse(alerts);
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "GetAlerts failed", LogLevel.Error, typeof(TeaserController));
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}