using System.Web.Mvc;
using Di.Common.Security.Encryption;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Session;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    // ReSharper disable Mvc.ViewNotResolved
    [NoCache]
    public class CodePortalPageController : MySettingsControllerBase<CodePortalPage>
    {
        private readonly ICryptographyService _cryptographyService;
        private readonly ISiteSettings _siteSettings;

        public CodePortalPageController(
            ISessionData sessionData, 
            ICryptographyService cryptographyService, 
            ISiteSettings siteSettings)
            : base(sessionData)
        {
            _cryptographyService = cryptographyService;
            _siteSettings = siteSettings;
        }

        [AuthorizeUser]
        public ActionResult Index(CodePortalPage currentPage, string msgc = "")
        {
            var user = GetSubscriberFromSession();

            var model = new CodePortalPageViewModel(currentPage)
            {
                UserId = _cryptographyService.EncryptString(user.ServicePlusUser.Id, _siteSettings.CryptoKeyUserId, _siteSettings.CryptoIvUserId),
                Token = _cryptographyService.EncryptString(user.ServicePlusToken, _siteSettings.CryptoKeyToken, _siteSettings.CryptoIvToken)
            };

            return View(model);
        }
    }
}