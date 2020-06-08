using System;
using System.ComponentModel.DataAnnotations;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;

namespace Pren.Web.Models.Blocks
{
    [ContentType(DisplayName = "TextBlock", GUID = "a440dc4e-5647-4d07-a616-a545d09b2837", Description = "Ett block med en editor för valfri text.")]
    public class TextBlock : SiteBlockData
    {
        
        [CultureSpecific]
        [Display(
            Name = "Text",
            GroupName = SystemTabNames.Content,
            Order = 25)]
        public virtual XhtmlString Editor { get; set; }

    }
}