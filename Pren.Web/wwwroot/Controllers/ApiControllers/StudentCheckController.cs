using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.Student;

namespace Pren.Web.Controllers.ApiControllers
{
    public class StudentCheckController : ExtendedApiController
    {
        private readonly IStudentHandler _studentHandler;

        public StudentCheckController(IApiReferrerCheck apiReferrerCheck, IStudentHandler studentHandler)
            : base(apiReferrerCheck)
        {
            _studentHandler = studentHandler;
        }

        public HttpResponseMessage Get(string socialSecurityNumber)
        {
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var response = new HttpResponseMessage
            {
                Content = new ObjectContent<bool>(_studentHandler.IsStudent(socialSecurityNumber), new JsonMediaTypeFormatter())
            };

            return response;
        }
    }
}
