using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.WebControls;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Static.DiCampaign
{
    public partial class CampaignFooter : EPiServer.UserControlBase
    {

        public bool IsDigitalCampaign
        {
            get { return EPiFunctions.HasValue(CurrentPage, "IsDigitalCampaign"); }
        }

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            PageData footerLinksSource = new PageData();

            if (IsValue("CampaignFooterLinks"))
            {
                footerLinksSource = CurrentPage;
            }
            else
            {
                footerLinksSource = EPiServer.DataFactory.Instance.GetPage(CurrentPage.ParentLink);
            }

            if (IsValue("CampaignFooterText"))
            {
                CampaignFooterTextLiteral.Text = CurrentPage["CampaignFooterText"].ToString();
            }
            else
            {
                if (IsDigitalCampaign)
                    CampaignFooterTextLiteral.Text = EPiServer.DataFactory.Instance.GetPage(CurrentPage.ParentLink)["CampaignFooterTextDigital"].ToString();
                else
                    CampaignFooterTextLiteral.Text = EPiServer.DataFactory.Instance.GetPage(CurrentPage.ParentLink)["CampaignFooterText"].ToString();
            }

            DiLinkCollection linkCollection = new DiLinkCollection(footerLinksSource, "CampaignFooterLinks");
            CampaignFooterLinksPageList.DataSource = linkCollection.SelectedPages(false);
            CampaignFooterLinksPageList.DataBind();
        }
    }
}