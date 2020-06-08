using System.Web.Mvc;
using Pren.Web.Business.Attributes;
using Pren.Web.Business.Session;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels.MySettings;

namespace Pren.Web.Controllers.MySettings
{
    // ReSharper disable once Mvc.ViewNotResolved
    [NoCache]
    public class AutowithdrawalPageController : MySettingsControllerBase<AutowithdrawalPage>
    {

        public AutowithdrawalPageController(ISessionData sessionData) : base(sessionData)
        {
        }

        #region Actions

        [AuthorizeUser (ValidateSubscriptionId = true)] 
        public ActionResult Index(AutowithdrawalPage currentPage, string sid)
        {
            var model = new AutowithdrawalPageViewModel(currentPage);

            return View(model);
        }

        #endregion


    }


}