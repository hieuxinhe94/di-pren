using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;

namespace Pren.Web.Models.Pages.MySettings
{
    public class MyPageData : SitePageData
    {
        [Display(
            Name = "Rubrik på sidan",
            Description = "Visas som rubrik, istället för sidans namn",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual string Heading { get; set; }

        [Display(
            Name = "Innehåll",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual XhtmlString MainBody { get; set; }
    }
}