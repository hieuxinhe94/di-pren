using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Pren.Web.Business;

namespace Pren.Web.Models.Blocks.Di
{
    [ContentType(DisplayName = "Profil", GUID = "e85ce797-5790-4df5-b3b0-00b64d545f1e", Description = "Mitt Di - Profil", GroupName = GroupNameConstants.MySettings)]
    public class ProfileBlock : AnchorBlockData
    {

        public ProfileBlock() : base("profile")
        {
            
        }       

        [CultureSpecific]
        [Display(
            Name = "Text, avsluta prenumeration",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual XhtmlString EndSubscriptionText { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text, mina prenumerationer",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual XhtmlString SubscriptionText { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text, informationsflik för prenumerationer",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual XhtmlString SubscriptionInfoText { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text, temporär adressändring",
            GroupName = SystemTabNames.Content,
            Order = 60)]
        public virtual XhtmlString TempAddressChangeInfoText { get; set; }

    }
}