using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Pren.Web.Business;
using Pren.Web.Business.CustomProperties;
using Pren.Web.Models.Blocks;

namespace Pren.Web.Models.Pages.MySettings
{
    [ContentType(DisplayName = "Min sida", 
        GUID = "3c3d39b6-dbc0-44bb-8b7a-242e8c58ed44", 
        Description = "Min sida",
        GroupName = GroupNameConstants.MySettings)]
    [AvailableContentTypes(Availability.Specific, Include = new[]
    {
        typeof(AutowithdrawalPage),
        typeof(CancelSubscriptionPage),
        typeof(ComplaintPage),
        typeof(InvoicePage),
        typeof(PermanentAddressChangePage),
        typeof(PersonInfoPage),
        typeof(SubscriptionSleepPage),
        typeof(TemporaryAddressChangePage),
        typeof(ConnectPage),
        typeof(BusinessSubscriptionPage),
        typeof(ContactPage),
        typeof(CodePortalPage)
    })]
    public class MySettingsStartPage : MyPageData
    {
        [Display(
            Name = "Innehåll, logga in",
            GroupName = SystemTabNames.Content,
            Order = 110)]
        public virtual XhtmlString LoginBody { get; set; }

        [Display(
            Name = "Meddelande",
            GroupName = SystemTabNames.Content,
            Order = 120)]
        public virtual XhtmlString MessageBody { get; set; }

        [Display(
            Name = "Meddelande - tema",
            GroupName = SystemTabNames.Content,
            Order = 130)]
        [SelectOne(SelectionFactoryType = typeof(MessageSelectionFactory))]
        public virtual string MessageTheme { get; set; }

        [Display(
            Name = "Innehållsyta under kontakta oss",
            GroupName = SystemTabNames.Content,
            Order = 140)]
        public virtual XhtmlString SecondaryBody { get; set; }

        [Display(
            Name = "Innehållsyta i högerkolumn",
            GroupName = SystemTabNames.Content,
            Order = 5000)]
        [AllowedTypes(new[] { typeof(TextBlock) })]
        public virtual ContentArea RightContentArea { get; set; }

        [Display(
            Name = "Innehållsyta i sidfoten",
            GroupName = SystemTabNames.Content,
            Order = 5100)]
        [AllowedTypes(new[] { typeof(TextBlock) })]
        public virtual ContentArea FooterContentArea { get; set; }
    }
}