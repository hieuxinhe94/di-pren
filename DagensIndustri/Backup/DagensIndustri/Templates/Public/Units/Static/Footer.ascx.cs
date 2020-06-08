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

namespace DagensIndustri.Templates.Public.Units.Static
{
    public partial class Footer : EPiServer.UserControlBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if (IsValue("MasterPageSelector"))
            {
                if (CurrentPage["MasterPageSelector"].ToString() == "/Templates/Public/Styles/dagensindustri.css")
                {
                    DagensIndustryFooterAd.Visible = true;
                    FooterLinksPlaceHolder.Visible = true;
                }
                else
                {
                    DIGoldFooterFirstColumn.Visible = true;
                    DIGoldFooterLogo.Visible = true;
                    DiGoldFooterLinksPlaceHolder.Visible = true;
                }
            }
            else
            {
                DagensIndustryFooterAd.Visible = true;
            }

            DiLinkCollection linkCollection = new DiLinkCollection(EPiFunctions.SettingsPage(CurrentPage), "FooterLinks");
            FooterLinks.DataSource = linkCollection.SelectedPages(false);
            FooterLinks.DataBind();

            DiLinkCollection diGoldLinkCollection = new DiLinkCollection(EPiFunctions.SettingsPage(CurrentPage), "DiGoldFooterLinks");
            DiGoldFooterLinks.DataSource = diGoldLinkCollection.SelectedPages(false);
            DiGoldFooterLinks.DataBind();

            PageDataCollection pdc = EPiServer.DataFactory.Instance.GetChildren(PageReference.StartPage);
            DagensIndustriMenu.DataSource = EPiFunctions.FilterMenu(pdc);
            DagensIndustriMenu.DataBind();

            CustomerHelp.Text = EPiFunctions.SettingsPage(CurrentPage)["FooterCustomerHelp"].ToString();
            DagensIndustriRightColumn.Text = EPiFunctions.SettingsPage(CurrentPage)["FooterDagensIndustriRightColumn"].ToString();
            DIGoldRightColumn.Text = EPiFunctions.SettingsPage(CurrentPage)["FooterDIGoldRightColumn"].ToString();
        }
    }
}