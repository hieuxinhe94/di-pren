using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.Blocks.Di;

namespace Pren.Web.Models.Pages.MySettings.Di
{
    [ContentType(DisplayName = "Min sida POC",
        GUID = "fd506bff-19bc-4405-b0c0-238ced16aff5", 
        Description = "Min sida Di POC",
        GroupName = GroupNameConstants.MySettings)]
    public class MyStartPage : SitePageData
    {

        [Display(
            Name = "Innehållsyta",
            GroupName = SystemTabNames.Content,
            Order = 4000)]
        [AllowedTypes(new[] { typeof(AnchorBlockData), typeof(BackgroundBlock) })]
        public virtual ContentArea ContentArea { get; set; }

        [Display(
            Name = "Innehållsyta, ej inloggad",
            GroupName = SystemTabNames.Content,
            Order = 5000)]
        [AllowedTypes(new[] { typeof(AnchorBlockData), typeof(BackgroundBlock) })]
        public virtual ContentArea ContentAreaNotLoggedIn { get; set; }

        [Display(
            Name = "Innehållsyta, sidtot",
            GroupName = SystemTabNames.Content,
            Order = 6000)]
        [AllowedTypes(new[] { typeof(TextBlock)})]
        public virtual ContentArea ContentAreaFooter { get; set; }

        [Display(
            Name = "Copyrighttext",
            GroupName = SystemTabNames.Content,
            Order = 6050)]
        public virtual string Copyright { get; set; }

        [Display(
            Name = "Kontakta oss mail frånadress (lämna tom för att amvända kundens adress)",
            GroupName = SystemTabNames.Settings,
            Order = 1010)]
        public virtual string ContactEmailFrom { get; set; }

        [Display(
            Name = "Kontakta oss mail tilladress",
            GroupName = SystemTabNames.Settings,
            Order = 1020)]
        public virtual string ContactEmailTo { get; set; }

        [Display(
            Name = "Kvitto på dragningar, mail tilladress",
            GroupName = SystemTabNames.Settings,
            Order = 1030)]
        public virtual string ReceiptEmailTo { get; set; }

        [Display(
            Name = "Uppsägningar, mail tilladress",
            GroupName = SystemTabNames.Settings,
            Order = 1035)]
        public virtual string CancelSubscriptionEmailTo { get; set; }

        [Display(
            Name = "BN API - Uppehåll - Skapa",
            GroupName = SystemTabNames.Settings,
            Order = 105)]
        public virtual bool BnApiCreateHolidayStop { get; set; }

        [Display(
            Name = "BN API - Uppehåll - Ta bort",
            GroupName = SystemTabNames.Settings,
            Order = 110)]
        public virtual bool BnApiDeleteHolidayStop { get; set; }

        [Display(
            Name = "BN API - Uppehåll - Ändra",
            GroupName = SystemTabNames.Settings,
            Order = 120)]
        public virtual bool BnApiChangeHolidayStop { get; set; }

        [Display(
            Name = "BN API - Uppehåll - Hämta",
            GroupName = SystemTabNames.Settings,
            Order = 130)]
        public virtual bool BnApiGetHolidayStop { get; set; }

        [Display(
            Name = "BN API - Reklamationstyper - Hämta",
            GroupName = SystemTabNames.Settings,
            Order = 140)]
        public virtual bool BnApiGetReclaimTypes { get; set; }

        [Display(
            Name = "BN API - Reklamationer - Hämta",
            GroupName = SystemTabNames.Settings,
            Order = 150)]
        public virtual bool BnApiGetReclaims { get; set; }

        [Display(
            Name = "BN API - Reklamationer - Skapa",
            GroupName = SystemTabNames.Settings,
            Order = 160)]
        public virtual bool BnApiCreateReclaims { get; set; }

        [Display(
            Name = "BN API - Kund - Uppdatera e-post",
            GroupName = SystemTabNames.Settings,
            Order = 170)]
        public virtual bool BnApiUpdateCustomerEmail { get; set; }

        [Display(
            Name = "BN API - Kund - Uppdatera telefonnummer",
            GroupName = SystemTabNames.Settings,
            Order = 180)]
        public virtual bool BnApiUpdateCustomerPhone { get; set; }

        [Display(
            Name = "BN API - Permanent adress - Skapa",
            GroupName = SystemTabNames.Settings,
            Order = 190)]
        public virtual bool BnApiCreatePermanentAddress { get; set; }

        [Display(
            Name = "BN API - Permanent adress - Hämta",
            GroupName = SystemTabNames.Settings,
            Order = 200)]
        public virtual bool BnApiGetPermanentAddress { get; set; }

        [Display(
            Name = "BN API - Permanent adress - Ta bort",
            GroupName = SystemTabNames.Settings,
            Order = 210)]
        public virtual bool BnApiDeletePermanentAddress { get; set; }

        [Display(
            Name = "BN API - Temporär adress - Hämta",
            GroupName = SystemTabNames.Settings,
            Order = 220)]
        public virtual bool BnApiGetTemporaryAddress { get; set; }

        [Display(
            Name = "BN API - Temporär adress lista - Hämta",
            GroupName = SystemTabNames.Settings,
            Order = 230)]
        public virtual bool BnApiGetTemporaryAddressList { get; set; }

        [Display(
            Name = "BN API - Temporär adress - Skapa",
            GroupName = SystemTabNames.Settings,
            Order = 240)]
        public virtual bool BnApiCreateTemporaryAddress { get; set; }

        [Display(
            Name = "BN API - Temporär adress - Ta bort",
            GroupName = SystemTabNames.Settings,
            Order = 250)]
        public virtual bool BnApiDeleteTemporaryAddress { get; set; }

        [Display(
            Name = "BN API - Temporär adress - Ändra",
            GroupName = SystemTabNames.Settings,
            Order = 260)]
        public virtual bool BnApiChangeTemporaryAddress { get; set; }

        [Display(
            Name = "BN API - Passerade utgåvedatum - Hämta",
            GroupName = SystemTabNames.Settings,
            Order = 270)]
        public virtual bool BnApiGetPastIssueDates{ get; set; }

        [Display(
            Name = "BN API - Prenumerationer - Hämta (Beroende av att Reklamationer och Temp adress är påslagna pga PaperCode och ProductNumber)",
            GroupName = SystemTabNames.Settings,
            Order = 500)]
        public virtual bool BnApiGetSubscriptions { get; set; }

        [Display(
            Name = "BN API - Kund - Hämta",
            GroupName = SystemTabNames.Settings,
            Order = 510)]
        public virtual bool BnApiGetCustomer { get; set; }

        [Display(
            Name = "BN API - Fakturor - Hämta",
            GroupName = SystemTabNames.Settings,
            Order = 520)]
        public virtual bool BnApiGetInvoices { get; set; }

        [Display(
            Name = "PREN WEB - Ta avgift för TAÄ",
            GroupName = SystemTabNames.Settings,
            Order = 600)]
        public virtual bool TakeTempAddrChangeFee { get; set; }
    }
}