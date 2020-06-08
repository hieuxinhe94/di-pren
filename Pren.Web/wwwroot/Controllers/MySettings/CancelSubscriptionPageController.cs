using System.Web.Mvc;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Session;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers.MySettings
{
    public class CancelSubscriptionPageController : MySettingsControllerBase<CancelSubscriptionPage>
    {
        public CancelSubscriptionPageController(
            ISessionData sessionData) : base(sessionData)
        {
        }

        [AuthorizeUser]
        public ActionResult Index(CancelSubscriptionPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);

            // ReSharper disable once Mvc.ViewNotResolved
            return View(model);
        }
    }
}