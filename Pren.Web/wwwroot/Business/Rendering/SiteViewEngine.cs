using System.Linq;
using System.Web.Mvc;

namespace Pren.Web.Business.Rendering
{
    /// <summary>
    /// Extends the Razor view engine to include the folders ~/Views/Shared/Blocks/ and ~/Views/Shared/PagePartials/
    /// when looking for partial views.
    /// </summary>
    public class SiteViewEngine : RazorViewEngine
    {
        private static readonly string[] AdditionalPartialViewFormats = new[] 
            { 
                TemplateCoordinator.BlockFolder + "{0}.cshtml",
                TemplateCoordinator.BlockFolderDi + "{0}.cshtml",
                TemplateCoordinator.PagePartialsFolder + "{0}.cshtml",
                TemplateCoordinator.MySettingsFolder + "{1}/{0}.cshtml",
                TemplateCoordinator.MySettingsFolderDi + "{1}/{0}.cshtml"
            };

        public SiteViewEngine()
        {
            PartialViewLocationFormats = PartialViewLocationFormats.Union(AdditionalPartialViewFormats).ToArray();
            ViewLocationFormats = ViewLocationFormats.Union(AdditionalPartialViewFormats).ToArray();
        }
    }
}
