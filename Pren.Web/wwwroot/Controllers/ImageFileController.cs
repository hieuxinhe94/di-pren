using System.Web.Mvc;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Pren.Web.Models.Media;
using Pren.Web.Models.ViewModels;

namespace Pren.Web.Controllers
{
    public class ImageFileController : PartialContentController<ImageFile>
    {
        private readonly UrlResolver _urlResolver;

        public ImageFileController(UrlResolver urlResolver)
        {
            _urlResolver = urlResolver;
        }

        public override ActionResult Index(ImageFile currentContent)
        {
            var model = new ImageFileViewModel
            {
                Url = _urlResolver.GetUrl(currentContent.ContentLink)
            };

            return PartialView(model);
        }
    }
}
