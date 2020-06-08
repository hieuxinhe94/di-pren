using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business.CustomProperties;

namespace Pren.Web.Models.Blocks
{
    [ContentType(GUID = "dcecc392-4b14-42de-92b7-d41f80b66812",
        DisplayName = "CampaignRangeBlock", 
        Description = "",
        AvailableInEditMode = false)]
    public class CampaignPeriodBlock : SiteBlockData
    {
        [Display(
                Name = "Periodrubrik (Iframe)",
                GroupName = SystemTabNames.Content,
                Order = 80)]
        public virtual string PeriodHeading { get; set; }

        [Display(
            Name = "Uppgraderingskampanj (Iframe)",
            GroupName = SystemTabNames.Content,
            Order = 90)]
        public virtual bool IsUpgradeCampaign { get; set; }

        [Display(
            Name = "Period (Redirect)",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Period { get; set; }

        [Display(
            Name = "Pris (Redirect)",
            GroupName = SystemTabNames.Content,
            Order = 200)]
        public virtual string Price { get; set; }

        [Display(
            Name = "Totalkostnadstext",
            GroupName = SystemTabNames.Content,
            Order = 300)]
        public virtual string TotalPrice { get; set; }

        [Display(
            Name = "Totalkostnadstext ex moms",
            GroupName = SystemTabNames.Content,
            Order = 310)]
        public virtual string TotalPriceExVat { get; set; }

        [ClientEditor(ClientEditingClass = "pren.editors.CampaignSelector")]
        [Display(
            Name = "Kampanj-ID (Kort & Faktura)",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        public virtual string CampaignCardAndInvoice { get; set; }

        [ClientEditor(ClientEditingClass = "pren.editors.CampaignSelector")]
        [Display(
            Name = "Kampanj-ID (Autogiro)",
            GroupName = SystemTabNames.Content,
            Order = 500)]
        public virtual string CampaignAutogiro { get; set; }

        [Display(
            Name = "SubsKind",
            GroupName = SystemTabNames.Content,
            Order = 600)]
        [SelectOne(SelectionFactoryType = typeof(SubsKindSelectionFactory))]
        public virtual string SubsKind { get; set; }

        [Display(
            Name = "Prova på kampanj (prisgrupp=42)",
            GroupName = SystemTabNames.Content,
            Order = 700)]
        public virtual bool IsTrial { get; set; }

        [Display(
            Name = "Prova på kampanj (prisgrupp=43)",
            GroupName = SystemTabNames.Content,
            Order = 750)]
        public virtual bool IsTrialFree { get; set; }

        [Display(
            Name = "Första summeringstext",
            GroupName = SystemTabNames.Content,
            Order = 800)]
        public virtual string FirstSummaryText { get; set; }

        [Display(
            Name = "Första summeringspris",
            GroupName = SystemTabNames.Content,
            Order = 900)]
        public virtual string FirstSummaryPrice { get; set; }

        [Display(
            Name = "Andra summeringstext",
            GroupName = SystemTabNames.Content,
            Order = 1000)]
        public virtual string SecondSummaryText { get; set; }

        [Display(
            Name = "Andra summeringspris",
            GroupName = SystemTabNames.Content,
            Order = 1100)]
        public virtual string SecondSummaryPrice { get; set; }

        [Display(
            Name = "Tredje summeringstext",
            GroupName = SystemTabNames.Content,
            Order = 1200)]
        public virtual string ThirdSummaryText { get; set; }

        [Display(
            Name = "Tredje summeringspris",
            GroupName = SystemTabNames.Content,
            Order = 1300)]
        public virtual string ThirdSummaryPrice { get; set; }

        [Display(
            Name = "Dölj kortbetalning",
            GroupName = SystemTabNames.Content,
            Order = 1400)]
        public virtual bool HideCardPayment { get; set; }

        [Display(
            Name = "Dölj fakturabetalning (Klarna, DIBS/Recurring)",
            GroupName = SystemTabNames.Content,
            Order = 1500)]
        public virtual bool HideInvoicePayment { get; set; }
    }
}