using System.Globalization;
using System.Web.Mvc;
using Di.Common.Security.Encryption;
using EPiServer.Editor;
using EPiServer.Web.Mvc;
using Pren.Web.Business.CodePortal;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Subscription;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers
{
    public class GiveAwayOfferBlockController : BlockController<GiveAwayOfferBlock>
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly ISiteSettings _siteSettings;
        private readonly ISubscriberFacade _subscriberFacade;
        private readonly ICodePortalService _codePortalService;

        public GiveAwayOfferBlockController(
            ICryptographyService cryptographyService, 
            ISiteSettings siteSettings, 
            ISubscriberFacade subscriberFacade, 
            ICodePortalService codePortalService)
        {
            _cryptographyService = cryptographyService;
            _siteSettings = siteSettings;
            _subscriberFacade = subscriberFacade;
            _codePortalService = codePortalService;
        }

        public override ActionResult Index(GiveAwayOfferBlock currentBlock)
        {
            var model = new GiveAwayOfferBlockViewModel(currentBlock);

            var token = _subscriberFacade.GetSubscriberFromSession().ServicePlusToken;

            var hasAccess = PageEditing.PageIsInEditMode || _codePortalService.HasAccessToCode(token, currentBlock.CodeList);

            // User dont have access to codes and block marked as dont show if no access
            if (!hasAccess && currentBlock.DontShowIfNotAvailable)
            {
                return PartialView("NoAccess", model);
            }

            // User sont have access to codes and block has fallback
            if (!hasAccess && !currentBlock.DontShowIfNotAvailable)
            {
                return PartialView("NoAccessWithFallback", model);
            }

            // User has access
            var encryptedCodeListId = _cryptographyService.EncryptString(currentBlock.CodeList.ToString(CultureInfo.InvariantCulture), _siteSettings.CryptoKeyCodeListId, _siteSettings.CryptoIvCodeListId);

            model.CodeListId = encryptedCodeListId;

            return PartialView(model);
        }
    }
}
