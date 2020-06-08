using System;
using System.Net;
using System.Net.Http;
using Bn.CompanyService.ContentApi;
using Di.Common.Logging;
using EPiServer;
using Pren.Web.Business.EpiSelfServiceContentApi.ContentApi;

namespace Pren.Web.Business.EpiCompanyServiceContentApi.ContentApi
{
    public class CompanyServiceContentApiController : ContentControllerBase
    {
        private readonly IContentRepository _contentRepository;
        private readonly ICompanyServiceContentService _companyServiceContentService;
        private readonly ILogger _logger;

        public CompanyServiceContentApiController(
            IContentRepository contentRepository,
            ICompanyServiceContentService companyServiceContentService,
            ILogger logger)
        {
            _contentRepository = contentRepository;
            _companyServiceContentService = companyServiceContentService;
            _logger = logger;
        }

        public HttpResponseMessage GetMessages(string brand)
        {
            try
            {
                var messages = _companyServiceContentService.GetMessages(brand);

                return CreateResponse(messages);
            }
            catch (Exception exception)
            {
                _logger.Log(exception, "GetMessages failed", LogLevel.Error, typeof(CompanyServiceContentApiController));
                return new HttpResponseMessage(HttpStatusCode.InternalServerError);
            }
        }
    }
}