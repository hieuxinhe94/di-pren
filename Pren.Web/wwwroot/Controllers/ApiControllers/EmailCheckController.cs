using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web.Http;
using EPiServer;
using EPiServer.Core;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.ServicePlus.Logic;
using Pren.Web.Models.Pages;

namespace Pren.Web.Controllers.ApiControllers
{
    public class EmailCheckController : ExtendedApiController
    {
        private readonly IServicePlusFacade _servicePlusFacade;
        private readonly IContentLoader _contentLoader;

        public EmailCheckController(
            IApiReferrerCheck apiReferrerCheck,
            IServicePlusFacade servicePlusFacade, 
            IContentLoader contentLoader)
            : base(apiReferrerCheck)
        {
            _servicePlusFacade = servicePlusFacade;
            _contentLoader = contentLoader;
        }

        [HttpPost]
        public HttpResponseMessage Get( [FromBody]EmailModel model)
        {
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var response = new HttpResponseMessage
            {
                Content = new ObjectContent<bool>(_servicePlusFacade.GetUserByEmail(model.Email) != null, new JsonMediaTypeFormatter())
            };

            return response;
        }

        [HttpPost]
        public HttpResponseMessage GetEmailMessage(bool isLoggedIn, [FromBody] EmailModel model)
        {
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var startPage = _contentLoader.Get<StartPage>(ContentReference.StartPage);

            if (startPage.EmailBipVerification)
            {
                var orderFlowMessage = _servicePlusFacade.GetOrderFlowMessage(model.Email, isLoggedIn);

                var responseOrder = new
                {
                    ForceLogin = orderFlowMessage.ForceLogin,
                    Message = orderFlowMessage.Message,
                    Header = orderFlowMessage.Header,
                    IsBip = true
                };

                return new HttpResponseMessage
                {
                    Content = new ObjectContent<object>(responseOrder, new JsonMediaTypeFormatter())
                };
            }

            var userResponse = _servicePlusFacade.GetUserByEmail(model.Email);

            var responseUser = new
            {
                ForceLogin = userResponse != null && !isLoggedIn,
                Message = string.Empty,
                Header = string.Empty,
                IsBip = false
            };

            return new HttpResponseMessage
            {
                Content = new ObjectContent<object>(responseUser, new JsonMediaTypeFormatter())                
            };           
        }
    }

    public class EmailModel
    {
        public string Email { get; set; }
    }
}
