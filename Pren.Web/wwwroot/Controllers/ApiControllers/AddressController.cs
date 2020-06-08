using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Pren.Web.Business.Address;
using Pren.Web.Business.Controllers;

namespace Pren.Web.Controllers.ApiControllers
{
    public class AddressController : ExtendedApiController
    {       
        //9697608330 KJTH, Apelgatan 5 A
        //5560163429 SSAB, Klarabergsviadukten 70 Uppg D
        //5563687184 Swedia, SKINNARVIKSRINGEN 16 LGH 2
        //5567762686 Supplement company, ELISETORPSVÄGEN 15 C LGH 1602
        //5564128980 Bjersbo, Flintlåsvägen 18 3tr Lgh1302 

        private readonly IAddressService _addressService;

        public AddressController(IApiReferrerCheck apiReferrerCheck, IAddressService addressService) : base(apiReferrerCheck)
        {
            _addressService = addressService;
        }
        public HttpResponseMessage Get(string input)
        {            
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var addressResult = _addressService.GetAddress(input);

            return new HttpResponseMessage
            {
                Content = new ObjectContent<AddressResult>(addressResult, new JsonMediaTypeFormatter())
            };
        }
    }
}