using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Pren.Web.Models.Pages.MySettings;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers.MySettings
{
    public class GoldMembershipPageController : PageController<GoldMembershipPage>
    {
        public ActionResult Index(GoldMembershipPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);

            // ReSharper disable once Mvc.ViewNotResolved
            return View(model);
        }
    }
}