using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;

namespace DagensIndustri.Templates.Public.MasterPages
{
    public partial class C_Campaign : System.Web.UI.MasterPage
    {

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            var pageBase = Page as PageBase;

            LitPageTitle.Text = (pageBase.IsValue("CampaignTitle") ? pageBase.CurrentPage["CampaignTitle"] : pageBase.CurrentPage.PageName) + " - Dagens industri";       
        }
    }
}