using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.WebControls;

namespace DagensIndustri.Templates.Public.Units.Placeable.DiCampaign
{
    public partial class CampaignSidebar : EPiServer.UserControlBase
    {
       
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if(IsValue("CampaignSideBarStartNode"))
            {
                PageDataCollection pdc = EPiServer.DataFactory.Instance.GetChildren(CurrentPage["CampaignSideBarStartNode"] as PageReference);
                CampaignSideBarPageList.DataSource = pdc;
            }
            else if(IsValue("CampaignSideBarCollection"))
            {
                DiLinkCollection CampaignSideBarCollection = new DiLinkCollection(CurrentPage, "CampaignSideBarCollection");
                CampaignSideBarPageList.DataSource = CampaignSideBarCollection.SelectedPages();
            }
            else
            {
                this.Visible = false;
            }

            CampaignSideBarPageList.DataBind();

            if (CampaignSideBarPageList.DataCount <= 0)
                this.Visible = false;
        }
    }
}