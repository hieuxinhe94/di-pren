using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Web.PropertyControls;

using PrenDiSe.Tools.Classes;
using PrenDiSe.Tools.Classes.WebControls;

namespace PrenDiSe.Tools.Properties
{
    /// <summary>
    /// Used in bootstrap campaign
    /// </summary>
    [Serializable]
    [PageDefinitionTypePlugIn(DisplayName = "Kampanjprodukter")]
    public class PropertyCampaignProductList : PropertyMultipleValue
    {
        public override IPropertyControl CreatePropertyControl()
        {
            return new PropertyCampaignProductListControl();
        }
    }

    public class PropertyCampaignProductListControl : PropertySelectMultipleControlBase
    {       
        protected override void SetupEditControls()
        {
            var campaignIncludedList = new DiLinkCollection(EPiFunctions.SettingsPage(CurrentPage), "CampaignProductList");

            foreach (var page in campaignIncludedList.SelectedPages())
            {
                var listItem = new ListItem(page.PageName, page.PageLink.ID.ToString());
                listItem.Selected = ((PropertyMultipleValue)PropertyData).IsValueActive(listItem.Value);
                this.EditControl.Items.Add(listItem);
            }            
        }

    }
}