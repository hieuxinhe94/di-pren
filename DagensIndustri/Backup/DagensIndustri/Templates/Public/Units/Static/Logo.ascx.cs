using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Static
{
    public partial class Logo : EPiServer.UserControlBase
    {
        public string logoUrl { get; set; }
        public string altText { get; set; }
        public string logoLinkUrl { get; set; }

        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (IsValue("MasterPageSelector"))
            {
                if (CurrentPage["MasterPageSelector"].ToString() == "/Templates/Public/Styles/dagensindustri.css")
                {
                    logoUrl = "/templates/public/images/logo.png";
                    altText = "Dagens industri";
                    logoLinkUrl = EPiFunctions.StartPage().LinkURL;

                    if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "CampaignPageType"))
                    { 
                        PlaceHolderLink.Visible = false;
                        PlaceHolderNoLink.Visible = true;
                    }
                }
                else
                {
                    logoUrl = "/templates/public/images/gold-logo.png";
                    altText = "Di Guld";
                    PageData DIGoldStartPage = EPiServer.DataFactory.Instance.GetPage(EPiFunctions.SettingsPage(CurrentPage)["DIGoldStartPage"] as PageReference);
                    logoLinkUrl = DIGoldStartPage.LinkURL;
                }
            }
            else
            { 
                logoUrl = "/templates/public/images/logo.png";
                altText = "Dagens industri";
                logoLinkUrl = EPiFunctions.StartPage().LinkURL;
            }  
        }

    }
}