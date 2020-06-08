using System.Web.Mvc;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Pren.Web.Business.Configuration;
using Pren.Web.Business.Helpers;
using Pren.Web.Models.Blocks.Di;
using Pren.Web.Models.ViewModels.MySettings.Di;

namespace Pren.Web.Controllers.Blocks.Di
{
    // ReSharper disable Mvc.PartialViewNotResolved
    public class ProfileBlockController : BlockController<ProfileBlock>
    {
        private readonly IUrlHelper _urlHelper;
        private readonly ISiteSettings _siteSettings;

        public ProfileBlockController(IUrlHelper urlHelper, ISiteSettings siteSettings)
        {
            _urlHelper = urlHelper;
            _siteSettings = siteSettings;
        }

        public override ActionResult Index(ProfileBlock currentBlock)
        {
            var model = new ProfileBlockViewModel(currentBlock);

            var callbackUrl = _urlHelper.GetContentUrlWithHost(CurrentPage.ContentLink);

            model.ChangePasswordUrl = _siteSettings.ServicePlusChangePasswordUrl + "?appId=" + _siteSettings.ServicePlusAppId + "&callback=" + callbackUrl;

            return PartialView(model);
        }

        public PageData CurrentPage
        {
            get
            {
                var pageRouteHelper = EPiServer.ServiceLocation.ServiceLocator.Current.GetInstance<EPiServer.Web.Routing.PageRouteHelper>();
                return pageRouteHelper.Page;
            }
        }
    }
}
