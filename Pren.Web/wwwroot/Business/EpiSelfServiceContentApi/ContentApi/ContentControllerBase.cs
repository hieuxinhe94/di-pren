using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Pren.Web.Business.EpiSelfServiceContentApi.ContentApi
{
    public class ContentControllerBase : ApiController
    {
        public HttpResponseMessage CreateResponse<T>(T responseObject, int maxAgeInMinutes = 1)
        {
                var response = Request.CreateResponse(HttpStatusCode.OK, responseObject, new JsonMediaTypeFormatter
                {
                    SerializerSettings = new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    }
                });

                response.Headers.CacheControl = new CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = new TimeSpan(0, 0, maxAgeInMinutes, 0)
                };

                return response;

        }
    }
}