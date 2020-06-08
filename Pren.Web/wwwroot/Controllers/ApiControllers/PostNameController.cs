using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Di.Subscription.Logic.PostName;
using Pren.Web.Business.Controllers;

namespace Pren.Web.Controllers.ApiControllers
{
    public class PostNameController : ExtendedApiController
    {
        private readonly IPostNameHandler _postNameHandler;

        public PostNameController(IApiReferrerCheck apiReferrerCheck, IPostNameHandler postNameHandler)
            : base(apiReferrerCheck)
        {
            _postNameHandler = postNameHandler;
        }

        public HttpResponseMessage Get(string zipCode)
        {
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var postName = _postNameHandler.GetPostName(zipCode);

            var response = Request.CreateResponse(HttpStatusCode.OK, postName);

            if (!string.IsNullOrEmpty(postName))
            {
                response.Headers.CacheControl = new CacheControlHeaderValue { Public = true, MaxAge = new TimeSpan(0, 0, 10, 0) };   
            }            

            return response;
        }
    }
}
