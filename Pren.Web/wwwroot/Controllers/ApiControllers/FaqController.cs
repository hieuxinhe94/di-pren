using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.Faq;
using Pren.Web.Business.Faq.Models.Items;
using Pren.Web.Business.Faq.Models.Topics;

namespace Pren.Web.Controllers.ApiControllers
{
    public class FaqController : ExtendedApiController
    {
        private readonly IFaqApi _faqApi;

        public FaqController(IApiReferrerCheck apiReferrerCheck, ISiteSettings siteSettings)
            : base(apiReferrerCheck)
        {
            _faqApi = new FaqApi(siteSettings.FaqApiBaseUrl);
        }

        public HttpResponseMessage GetItems(int limit, string sortorder)
        {
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {       
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new ObjectContent<List<Item>>(_faqApi.Items.GetItems(limit, sortorder), new JsonMediaTypeFormatter())
                };

                response.Headers.CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = new TimeSpan(0, 0, 10, 0) };

                return response;
            }
            catch 
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }           
        }

        public HttpResponseMessage GetTopics(int limit, string sortorder)
        {
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new ObjectContent<List<Topic>>(_faqApi.Topics.GetTopics(limit, sortorder), new JsonMediaTypeFormatter())
                };

                response.Headers.CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = new TimeSpan(0, 0, 10, 0) };

                return response;
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        public HttpResponseMessage GetItemsByTopics(string topic, int limit, string sortorder)
        {
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            try
            {
                var response = new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new ObjectContent<List<Item>>(_faqApi.Items.GetItemsByTopic(topic, limit, sortorder), new JsonMediaTypeFormatter())
                };

                response.Headers.CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = new TimeSpan(0, 0, 10, 0) };

                return response;
            }
            catch
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

    }
}
