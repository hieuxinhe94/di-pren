using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Kontakta oss",
        GUID = "4c62a13c-40b5-4333-ad6d-8b39cffb0663", 
        Description = "Skicka mail via ett formulär",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.None)]
    public class ContactPage : MyPageData
    {
        [Display(
            Name = "Visa formulär från",
            Description = "Om formuläret ska visas från 08:00 ange 8",
            GroupName = SystemTabNames.Content,
            Order = 300)]
        public virtual int FormShowFrom { get; set; }

        [Display(
            Name = "Visa formulär till",
            Description = "Om formuläret ska döljas 17:00 ange 17",
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
        public virtual bool ShowForm{ get; set; }

        [Display(
            Name = "Tacktext",
            GroupName = SystemTabNames.Content,
            Order = 600)]
        [Required]
        public virtual XhtmlString ThankYouBody { get; set; }

    }
}