using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Blocks.Di
{
    [ContentType(DisplayName = "Kontakt", GUID = "3bd66e73-66be-48ea-813c-bb4bdc1073cd", Description = "Mitt Di - Kontakt", GroupName = GroupNameConstants.MySettings)]
    public class ContactBlock : AnchorBlockData
    {
        public ContactBlock(): base("contact")
        {
        }

        [Display(
            Name = "Visa formulär från",
            Description = "Om formuläret ska visas från 08:00 ange 8",
            GroupName = SystemTabNames.Content,
            Order = 300)]
        public virtual int FormShowFrom { get; set; }

        [Display(
            Name = "Visa formulär till",
            Description = "Om formuläret ska visas till 17:00 ange 17",
            GroupName = SystemTabNames.Content,
            Order = 400)]
        public virtual int FormShowTo { get; set; }

        [Display(
            Name = "Innehåll vid dolt formulär",
            GroupName = SystemTabNames.Content,
            Order = 500)]
        public virtual XhtmlString FormHiddenBody { get; set; }

        [Display(
            Name = "Visa alltid formulär",
            GroupName = SystemTabNames.Content,
            Order = 600)]
        public virtual bool ShowForm { get; set; }

        [Display(
            Name = "Skicka e-post till kundservice efter att chatten stängt (chatten är öppen vardagar 08-18, helger 09-13)",
            GroupName = SystemTabNames.Content,
            Order = 700)]
        public virtual bool SendMailOnSpecificTimes { get; set; }

        [Display(
            Name = "Tacktext efter inskickat ärende (när chatten är stängd)",
            GroupName = SystemTabNames.Content,
            Order = 800)]
        public virtual XhtmlString FormThankYouText { get; set; }

        [Display(
            Name = "HELGER - chatt öppen från",
            Description = "08:00 ange 8",
            GroupName = SystemTabNames.Content,
            Order = 900)]
        public virtual int WeekendChatOpenFrom { get; set; }

        [Display(
            Name = "HELGER - chatt öppen till",
            Description = "17:00 ange 17",
            GroupName = SystemTabNames.Content,
            Order = 1000)]
        public virtual int WeekendChatOpenTo { get; set; }

        [Display(
            Name = "VARDAGAR - chatt öppen från",
            Description = "08:00 ange 8",
            GroupName = SystemTabNames.Content,
            Order = 1100)]
        public virtual int VardagChatOpenFrom { get; set; }

        [Display(
            Name = "VARDAGAR - chatt öppen till",
            Description = "17:00 ange 17",
            GroupName = SystemTabNames.Content,
            Order = 1200)]
        public virtual int VardagChatOpenTo { get; set; }
    }
}