using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Http;
using Di.Common.Security.Encryption;
using Pren.Web.Business.CodePortal;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Controllers;
using Pren.Web.Business.Subscription;

namespace Pren.Web.Controllers.ApiControllers.CodePortal
{
    public class CodePortalCodeController : ExtendedApiController
    {
        private readonly ISubscriberFacade _subscriberFacade;
        private readonly ICodePortalService _codePortalService;
        private readonly ICryptographyService _cryptographyService;
        private readonly ISiteSettings _siteSettings;

        public CodePortalCodeController(
            ICodePortalService codePortalService,
            ICryptographyService cryptographyService,
            ISiteSettings siteSettings,
            IApiReferrerCheck apiReferrerCheck, 
            ISubscriberFacade subscriberFacade) : base(apiReferrerCheck)
        {
            _codePortalService = codePortalService;
            _cryptographyService = cryptographyService;
            _siteSettings = siteSettings;
            _subscriberFacade = subscriberFacade;
        }

        [HttpGet]
        public HttpResponseMessage GetExistingCode(string userId, string listId)
        {
            var validationResponse = GetValidationResponse();
            if (validationResponse != null)
            {
                return validationResponse;
            }

            var decryptedListId = GetDecryptedStringAsInteger(listId, _siteSettings.CryptoKeyCodeListId, _siteSettings.CryptoIvCodeListId);
            if (decryptedListId == 0)
            {
                return GetResponseMessage(string.Empty);
            }

            var decryptedUserId = GetDecryptedString(userId, _siteSettings.CryptoKeyUserId, _siteSettings.CryptoIvUserId);

            if (string.IsNullOrEmpty(decryptedUserId))
            {
                return GetResponseMessage(string.Empty);
            }

            return GetResponseMessage(_codePortalService.GetExistingCode(decryptedUserId, decryptedListId));
        }

        [HttpGet]
        public HttpResponseMessage GetExistingGiveAway(string userId, string listId)
        {
            var validationResponse = GetValidationResponse();
            if (validationResponse != null)
            {
                return validationResponse;
            }

            var decryptedListId = GetDecryptedStringAsInteger(listId, _siteSettings.CryptoKeyCodeListId, _siteSettings.CryptoIvCodeListId);
            if (decryptedListId == 0)
            {
                return GetResponseMessage(string.Empty);
            }

            var decryptedUserId = GetDecryptedString(userId, _siteSettings.CryptoKeyUserId, _siteSettings.CryptoIvUserId);

            if (string.IsNullOrEmpty(decryptedUserId))
            {
                return GetResponseMessage(string.Empty);
            }

            return GetResponseMessage(_codePortalService.GetExistingGiveAway(decryptedUserId, decryptedListId));
        }

        [HttpGet]
        public HttpResponseMessage GetNewCode(string token, string listId)
        {
            var validationResponse = GetValidationResponse();
            if (validationResponse != null)
            {
                return validationResponse;
            }

            var decryptedListId = GetDecryptedStringAsInteger(listId, _siteSettings.CryptoKeyCodeListId, _siteSettings.CryptoIvCodeListId);
            if (decryptedListId == 0)
            {
                return GetResponseMessage(string.Empty);
            }

            var decryptedToken = GetDecryptedString(token, _siteSettings.CryptoKeyToken, _siteSettings.CryptoIvToken);

            if (string.IsNullOrEmpty(decryptedToken))
            {
                return GetResponseMessage(string.Empty);
            }

            var hasAccess = _codePortalService.HasAccessToCode(decryptedToken, decryptedListId);

            if (!hasAccess)
            {
                return GetResponseMessage(string.Empty);
            }

            return GetResponseMessage(_codePortalService.GetNewCodeAndSetAsUsed(decryptedToken, decryptedListId));
        }

        [HttpGet]
        public async Task<HttpResponseMessage> CreateNewGiveAway(string token, string listId, string giveToEmail)
        {
            var validationResponse = GetValidationResponse();
            if (validationResponse != null)
            {
                return validationResponse;
            }

            var decryptedListId = GetDecryptedStringAsInteger(listId, _siteSettings.CryptoKeyCodeListId, _siteSettings.CryptoIvCodeListId);
            if (decryptedListId == 0)
            {
                return GetResponseMessage(string.Empty);
            }

            var decryptedToken = GetDecryptedString(token, _siteSettings.CryptoKeyToken, _siteSettings.CryptoIvToken);

            if (string.IsNullOrEmpty(decryptedToken))
            {
                return GetResponseMessage(string.Empty);
            }

            var hasAccess = _codePortalService.HasAccessToCode(decryptedToken, decryptedListId);

            if (!hasAccess)
            {
                return GetResponseMessage(string.Empty);
            }

            var giveAway = await _codePortalService.CreateNewGiveAway(decryptedToken, decryptedListId, giveToEmail);

            return GetResponseMessage(giveAway);
        }

        private string GetDecryptedString(string value, string key, string iv)
        {
            if (string.IsNullOrEmpty(value))
            {
                return "";
            }

            var decrypted = _cryptographyService.DecryptString(value, key, iv);

            return decrypted;
        }

        private int GetDecryptedStringAsInteger(string value, string key, string iv)
        {
            var decryptedInteger = 0;
            var decrypted = GetDecryptedString(value, key, iv);

            if (string.IsNullOrEmpty(decrypted) || !int.TryParse(decrypted, out decryptedInteger))
            {
                return 0;
            }

            return decryptedInteger;
        }

        private HttpResponseMessage GetValidationResponse()
        {
            var subscriber = _subscriberFacade.GetSubscriberFromSession();

            if (!VerifyDomain() || !VerifySubscriber(subscriber))
            {
                return Request.CreateResponse(HttpStatusCode.Forbidden);
            }

            return null;
        }

        private HttpResponseMessage GetResponseMessage<T>(T response)
        {
            return new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new ObjectContent<T>(response, new JsonMediaTypeFormatter())
            };
        }
    }
}
