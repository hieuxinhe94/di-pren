using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business.EpiCompanyServiceContentApi.Models.Blocks;

namespace Pren.Web.Business.EpiCompanyServiceContentApi.Models.Pages
{
    [ContentType(DisplayName = "Startsida för företagsservice innehåll", GUID = "17921121-FFA0-4591-BD5B-2D858DAB8361", Description = "Startsida för företagsservice innehåll", GroupName = "Företagsservice")]
    public class CompanyServiceHomePage : PageData
    {
        [Display(
            Name = "Viktiga meddelanden",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        [AllowedTypes(new[] { typeof(MessageBlock) })]
        public virtual ContentArea MessageContentArea { get; set; }
    }
}