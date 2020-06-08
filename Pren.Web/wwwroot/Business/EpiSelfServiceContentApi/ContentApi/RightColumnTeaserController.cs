using System;
using System.Net;
using System.Net.Http;
using Bn.SelfService.ContentApi;
using Di.Common.Logging;

namespace Pren.Web.Business.EpiSelfServiceContentApi.ContentApi
{
    public class RightColumnTeaserController : ContentControllerBase
    {
        private readonly ISelfServiceContentService _selfServiceContentService;
        private readonly ILogger _logger;

        public RightColumnTeaserController(
            ISelfServiceContentService selfServiceContentService,
            ILogger logger)
        {
            _selfServiceContentService = selfServiceContentService;
            _logger = logger;
        }

        public HttpResponseMessage GetRightColumnTeasers(string brand)
        {
            try
            {
                var teasers = _selfServiceContentService.GetRightColumnTeasers(brand);

                return CreateResponse(teasers);
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "GetTeasers failed", LogLevel.Error, typeof(TeaserController));
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}