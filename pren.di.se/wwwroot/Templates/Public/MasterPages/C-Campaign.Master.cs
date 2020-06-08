using System;
using System.Collections.Generic;
using System.Linq;

using EPiServer;

namespace PrenDiSe.Templates.Public.MasterPages
{
    public partial class C_Campaign : System.Web.UI.MasterPage
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            plhMaintenanceScript.DataBind();

            var pageBase = Page as PageBase;

            LitPageTitle.Text = (pageBase.IsValue("CampaignTitle") ? pageBase.CurrentPage["CampaignTitle"] : pageBase.CurrentPage.PageName) + " - Dagens industri";

            plhMaintenanceScript.DataBind();
        }
    }
}