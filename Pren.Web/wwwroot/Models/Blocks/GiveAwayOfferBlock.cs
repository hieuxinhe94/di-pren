using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Pren.Web.Business;
using Pren.Web.Business.Descriptors.Editor;

namespace Pren.Web.Models.Blocks
{
    [ContentType(
        DisplayName = "Ge bort kampanj", 
        GUID = "08069b0f-35a4-4af4-b72e-f9fd5b91429f", 
        Description = "Block för ge bort kampanj")]
    public class GiveAwayOfferBlock : SiteBlockData
    {
        [EditorDescriptor(EditorDescriptorType = typeof(CodePortalListEditorDescriptor))]
        [Display(
            Name = "Kodlista",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual int CodeList { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Rubrik",
            Description = "Rubrik",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual String Heading { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Instruktionstext",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual XhtmlString Instructions { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Information som visas när man gett bort en kampanj",
            Description = "Visas ovanför e-postadressen man gicit bort till",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual String GiveAwayInfo { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Knapptext för ge bort kampanj",
            Description = "Texten i knappen för att ge bort kampanj",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual String ButtonText { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Text som visas när kunden inte har rätt att ge bort kampanj",
            Description = "Visas när en kund som inte har rätt att ge bort en kanpanj klickar på knappen",
            GroupName = SystemTabNames.Content,
            Order = 60)]
        public virtual XhtmlString GiveAwayNotAvailableText { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(
            Name = "Ebjudandebild",
            Description = "Erbjudandets bild",
            GroupName = SystemTabNames.Content,
            Order = 70)]
        public virtual ContentReference Image { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Villkorstext under bild",
            GroupName = SystemTabNames.Content,
            Order = 80)]
        public virtual XhtmlString TextBelowImage { get; set; }

        [Display(
            Name = "Visa inte erbjudandet för kunder utan access",
            GroupName = CustomTabs.Fallback,
            Order = 10)]
        public virtual bool DontShowIfNotAvailable { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(
            Name = "Fallbackbild för erbjudande",
            Description = "Fallbackbild för erbjudande",
            GroupName = CustomTabs.Fallback,
            Order = 20)]
        public virtual ContentReference FallbackImage { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Fallbacktext under bild",
            GroupName = CustomTabs.Fallback,
            Order = 30)]
        public virtual XhtmlString FallbackTextBelowImage { get; set; }
    }
}