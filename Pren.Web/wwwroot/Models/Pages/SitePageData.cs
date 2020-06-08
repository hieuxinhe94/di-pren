using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using Pren.Web.Business;
using Pren.Web.Business.Rendering;

namespace Pren.Web.Models.Pages
{
    public abstract class SitePageData : PageData, ICustomCssInContentArea
    {
        public string ContentAreaCssClass { get; private set; }

        [Display(
            GroupName = CustomTabs.Advanced,
            Order = 100)] 
        public virtual string MetaTitle
        {
            get
            {
                var metaTitle = this.GetPropertyValue(p => p.MetaTitle);

                // Use explicitly set meta title, otherwise fall back to page name
                return !string.IsNullOrWhiteSpace(metaTitle)
                       ? metaTitle
                       : "Dagens industri";
            }
            set { this.SetPropertyValue(p => p.MetaTitle, value); }
        }
    }
}
