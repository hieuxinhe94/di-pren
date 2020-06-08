using System;
using System.Net;
using System.Net.Http;
using Bn.SelfService.ContentApi;
using Di.Common.Logging;

namespace Pren.Web.Business.EpiSelfServiceContentApi.ContentApi
{
    public class CodeController : ContentControllerBase
    {
        private readonly ISelfServiceContentService _selfServiceContentService;
        private readonly ILogger _logger;

        public CodeController(
            ISelfServiceContentService selfServiceContentService,
            ILogger logger)
        {
            _selfServiceContentService = selfServiceContentService;
            _logger = logger;
        }

        public HttpResponseMessage GetCodes(string brand)
        {
            try
            {
                var codes = _selfServiceContentService.GetCodes(brand);

                return CreateResponse(codes);
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "GetCodes failed", LogLevel.Error, typeof(TeaserController));
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}