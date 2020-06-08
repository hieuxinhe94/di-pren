using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business;
using Pren.Web.Business.CustomProperties;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Models.Pages
{
    [ContentType(DisplayName = "Kampanjsida (gammal)", GUID = "f43981b9-1c2e-4e3a-a155-90cf57a146d8", Description = "Kampanjsida med NETS betalning", GroupName = "Kampanj")]
    [AvailableContentTypes(Availability.None)]
    public class CampaignPage : SitePageData
    {
        [Display(
            Name = "Rubrik",
            Description = "Rubrik",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Heading { get; set; }

        [Required]
        [Display(
            Name = "Målgrupp, vanliga besökare",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual string TargetGroup { get; set; }

        [Required]
        [Display(
            Name = "Målgrupp, mobila besökare",
            GroupName = SystemTabNames.Content,
            Order = 300)]
        public virtual string TargetGroupMobile { get; set; }

        [Required]
        [Display(
            Name = "Försäljningskanal",
            GroupName = SystemTabNames.Content,
            Order = 310)]
        [SelectOne(SelectionFactoryType = typeof(ReceiveTypeListSelectionFactory))]
        public virtual string SalesChannel { get; set; }

        [Display(
            Name = "Extra fält",
            GroupName = SystemTabNames.Content,
            Order = 350)]
        public virtual string ExtraInfoHeading { get; set; }

        [Display(
            Name = "Extra fält, tvingande",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        public virtual bool ExtraInfoMandatory { get; set; }

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

        [Display(
            Name = "Innehållsyta för kampanjer",
            Description = "Innehållsyta för kampanjer",
            GroupName = SystemTabNames.Content,
            Order = 5000)]
        [AllowedTypes(new[] { typeof(CampaignBlock) })]
        public virtual ContentArea CampaignContentArea { get; set; }
    }
}