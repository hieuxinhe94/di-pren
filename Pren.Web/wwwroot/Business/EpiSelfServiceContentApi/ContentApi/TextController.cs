using System;
using System.Net;
using System.Net.Http;
using Bn.SelfService.ContentApi;
using Di.Common.Logging;

namespace Pren.Web.Business.EpiSelfServiceContentApi.ContentApi
{
    public class TextController : ContentControllerBase
    {
        private readonly ISelfServiceContentService _selfServiceContentService;
        private readonly ILogger _logger;

        public TextController(
            ISelfServiceContentService selfServiceContentService,
            ILogger logger)
        {
            _selfServiceContentService = selfServiceContentService;
            _logger = logger;
        }

        public HttpResponseMessage GetText(string brand, string type)
        {
            try
            {
                var text = _selfServiceContentService.GetText(brand, type);

                return CreateResponse(text);
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "GetText failed", LogLevel.Error, typeof(TextController));
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}