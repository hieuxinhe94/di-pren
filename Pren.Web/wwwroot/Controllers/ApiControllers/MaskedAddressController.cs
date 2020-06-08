using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using Pren.Web.Business.Address;
using Pren.Web.Business.Controllers;

namespace Pren.Web.Controllers.ApiControllers
{
    public class MaskedAddressController : ExtendedApiController
    {       
        //9697608330 KJTH, Apelgatan 5 A
        //5560163429 SSAB, Klarabergsviadukten 70 Uppg D
        //5563687184 Swedia, SKINNARVIKSRINGEN 16 LGH 2
        //5567762686 Supplement company, ELISETORPSVÄGEN 15 C LGH 1602
        //5564128980 Bjersbo, Flintlåsvägen 18 3tr Lgh1302 

        private readonly IAddressService _addressService;
        private readonly IMaskedAddressService _maskedAddressService;

        public MaskedAddressController(IApiReferrerCheck apiReferrerCheck, IAddressService addressService, IMaskedAddressService maskedAddressService) : base(apiReferrerCheck)
        {
            _addressService = addressService;
            _maskedAddressService = maskedAddressService;
        }

        public HttpResponseMessage Get(string input)
        {            
            if (!VerifyDomain())
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            var addressResult = new MaskedAddressResult(_addressService.GetAddress(input));

            if (string.IsNullOrEmpty(addressResult.Error) && string.IsNullOrEmpty(addressResult.CompanyNumber))
            {
                //Do Masking
                addressResult = _maskedAddressService.MaskInfo(addressResult);
            }

            return new HttpResponseMessage
            {
                Content = new ObjectContent<MaskedAddressResult>(addressResult, new JsonMediaTypeFormatter())
            };
        }
    }
}