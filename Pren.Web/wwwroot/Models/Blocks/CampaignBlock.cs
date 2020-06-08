using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Pren.Web.Business.Descriptors.Editor;

namespace Pren.Web.Models.Blocks
{
    [ContentType(DisplayName = "CampaignBlock", GUID = "4b97c8ba-35cb-49aa-9dcb-7147aaec10cf", Description = "Kampanjblock")]
    public class CampaignBlock : SiteBlockData
    {
        
        [CultureSpecific]
        [Display(
            Name = "Rubrik",
            Description = "Kampanjens rubrik",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual String Heading { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Digital kampanj",
            GroupName = SystemTabNames.Content,
            Order = 15)]
        public virtual bool IsDigital { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Studentkampanj",
            GroupName = SystemTabNames.Content,
            Order = 16)]
        public virtual bool IsStudent { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Betalväggskampanj",
            GroupName = SystemTabNames.Content,
            Order = 17)]
        public virtual bool IsPayWall { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text ovanför bild",
            GroupName = SystemTabNames.Content,
            Order = 19)]
        public virtual XhtmlString TextAboveImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(
            Name = "Bild",
            Description = "Kampanjens bild",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Pricetagtext, över",
            GroupName = SystemTabNames.Content,
            Order = 21)]
        public virtual string ImagePriceTag1 { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Pricetagtext, mitten",
            GroupName = SystemTabNames.Content,
            Order = 22)]
        public virtual string ImagePriceTag2 { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Pricetagtext, under",
            GroupName = SystemTabNames.Content,
            Order = 23)]
        public virtual string ImagePriceTag3 { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Dölj pricetag i desktop",
            GroupName = SystemTabNames.Content,
            Order = 24)]
        public virtual bool HidePriceTagInDesktop { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text under bild",
            GroupName = SystemTabNames.Content,
            Order = 25)]
        public virtual XhtmlString TextBelowImage { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Produkttext",
            GroupName = SystemTabNames.Content,
            Order = 26)]
        public virtual XhtmlString ProductCopy { get; set; }

        [EditorDescriptor(EditorDescriptorType = typeof(CampaignUspEditorDescriptor))]
        [Display(
            Name = "Usp Grupp",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual int UspProduct { get; set; }

        [Display(
            Name = "Kampanjperiod 1",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual CampaignPeriodBlock FirstCampaignPeriod { get; set; }

        [Display(
            Name = "Kampanjperiod 2",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual CampaignPeriodBlock SecondCampaignPeriod { get; set; }

        [Display(
            Name = "Kampanjperiod 3",
            GroupName = SystemTabNames.Content,
            Order = 60)]
        public virtual CampaignPeriodBlock ThirdCampaignPeriod { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Kortbetalning text",
            GroupName = SystemTabNames.Content,
            Order = 70)]
        public virtual XhtmlString CardPayText { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Fakturabetalning text",
            GroupName = SystemTabNames.Content,
            Order = 80)]
        public virtual XhtmlString InvoicePayText { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Autogiro text",
            GroupName = SystemTabNames.Content,
            Order = 90)]
        public virtual XhtmlString AutogiroPayText { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            CardPayText = new XhtmlString("Vi samarbetar med DIBS för betalningar med kort. Du kommer därför att skickas till en separat sida för att slutföra betalningen.");
            InvoicePayText = new XhtmlString("Ska fakturan gå till företaget? Då kan du ange annan faktureringsadress nedan.");
            AutogiroPayText = new XhtmlString("Ett autogiromedgivande kommer att skickas till dig och beloppet dras månadsvis från ditt konto. För din första period kommer du att erhålla en faktura.");
        }

        public XhtmlString GetCardPayText()
        {
            try
            {
                if (CardPayText != null && !string.IsNullOrEmpty(CardPayText.ToString()))
                {
                    return new XhtmlString(CardPayText.ToString().Replace("Nets", "DIBS"));
                }
            }
            catch (Exception)
            {
                return CardPayText;
            }

            return CardPayText;
        }
    }
}