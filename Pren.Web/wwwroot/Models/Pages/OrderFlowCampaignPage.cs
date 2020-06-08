using System.ComponentModel.DataAnnotations;
using EPiServer;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business;
using Pren.Web.Business.CustomProperties;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "Orderflöde för privat och företag", GUID = "87a2ddd6-c0f9-47a3-90e2-3131acdecfc1", Description = "Orderflöde för privat och företag", GroupName = "Kampanj")]
    public class OrderFlowCampaignPage : SitePageData
    {
        [Display(
            Name = "Rubrik",
            Order = 10)]
        public virtual string Heading { get; set; }

        [Display(
            Name = "Underrubrik",
            Order = 20)]
        public virtual string SubHeading { get; set; }

        [Display(
            Name = "Url till företagsportalen",
            Order = 30)]
        public virtual Url CompanyPortalUrl { get; set; }

        [Required]
        [Display(
            Name = "Försäljningskanal",
            GroupName = SystemTabNames.Content,
            Order = 35)]
        [SelectOne(SelectionFactoryType = typeof(ReceiveTypeListSelectionFactory))]
        public virtual string SalesChannel { get; set; }

        [Required]
        [UIHint(EPiServer.Web.UIHint.LongString)]
        [Display(
            Name = "Kampanjer",
            Description = "Kampanjer, JSON",
            Order = 40)]
        public virtual string Packages { get; set; }

        [UIHint(EPiServer.Web.UIHint.LongString)]
        [Display(
            Name = "Jämförelsetext",
            Order = 50)]
        public virtual string CompareText { get; set; }

        [UIHint(EPiServer.Web.UIHint.LongString)]
        [Display(
            Name = "Script på load i head",
            GroupName = CustomTabs.Script,
            Order = 450)]
        public virtual string ScriptLoadInHeader { get; set; }

        [UIHint(EPiServer.Web.UIHint.LongString)]
        [Display(
            Name = "Script på load",
            GroupName = CustomTabs.Script,
            Order = 500)]
        public virtual string ScriptLoad { get; set; }

        [UIHint(EPiServer.Web.UIHint.LongString)]
        [Display(
            Name = "Script på tacksidan",
            GroupName = CustomTabs.Script,
            Order = 550)]
        public virtual string ScriptThankyou { get; set; }

        [UIHint(EPiServer.Web.UIHint.LongString)]
        [Display(
            Name = "Script på tacksidan i head",
            GroupName = CustomTabs.Script,
            Order = 560)]
        public virtual string ScriptThankyouInHeader { get; set; }
    }
}