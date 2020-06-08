using System.Web.Mvc;
using EPiServer.Web.Mvc;
using Pren.Web.Models.Pages;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers 
{
    public class StartPageController : PageController<StartPage>
    {
        public ActionResult Index(StartPage currentPage)
        {
            var model = PageViewModel.Create(currentPage);

            return View(model);
        }
    }
}