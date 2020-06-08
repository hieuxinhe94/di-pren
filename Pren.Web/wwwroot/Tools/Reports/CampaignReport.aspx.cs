using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using EPiServer.Shell.WebForms;
using Pren.Web.Business;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.Pages;

namespace Pren.Web.Tools.Reports
{
    [GuiPlugIn(
    Area = PlugInArea.ReportMenu,
    DisplayName = "Campaign report",
    Description = "List of all campaign pages and their connected campaigns",
    Category = "Di reports",
    Url = "~/Tools/Reports/CampaignReport.aspx")]
    public partial class CampaignReport : WebFormsBase 
    {
        private Injected<ContentLocator> ContentLocator { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                CampaignTypeSelector.DataSource = GetCampaignPageTypes();
                CampaignTypeSelector.DataBind();

                CampaignTypeSelector.Items.Insert(0, new ListItem("Alla kampanjer", "all"));
            }
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }

        protected void BtnReportClick(object sender, EventArgs e)
        {
            GvReport.DataSource = GetCampaigns();
            GvReport.DataBind();
        }

        private IEnumerable<PageType> GetCampaignPageTypes()
        {
            var pageTypeRepos = new PageTypeRepository(ContentTypeRepository);

            return pageTypeRepos.List().Where(t => t.Name.StartsWith("CampaignPage"))
                    .OrderByDescending(t => t.ID)
                    .Select(t => t);
        }

        private List<CampaignPageItem> GetCampaigns()
        {
            var campaignPages = CampaignTypeSelector.SelectedValue.Equals("all") 
                ? FindPagesByPageType<CampaignPage>().Union(FindPagesByPageType<CampaignPageIframe>()).Union(FindPagesByPageType<CampaignPageSplus>()).ToList()
                : ContentLocator.Service.FindPagesByPageType<CampaignPage>(ContentReference.RootPage, true, int.Parse(CampaignTypeSelector.SelectedValue));

            return campaignPages.Select(campaign => new CampaignPageItem
            {
                CampaignPage = campaign,
                CampaignBlocks = GetCampaignBlocks(campaign)
            }).ToList();
        }

        private IEnumerable<T> FindPagesByPageType<T>() where T : PageData
        {
            return ContentLocator.Service.FindPagesByPageType<T>(ContentReference.StartPage, true,
                ContentTypeRepository.Load<T>().ID);
        }

        private List<CampaignBlock> GetCampaignBlocks(CampaignPage campaignPage)
        {
            return campaignPage.CampaignContentArea != null ? campaignPage.CampaignContentArea.Items.Select(block => block.GetContent()).OfType<CampaignBlock>().ToList() : new List<CampaignBlock>();
        }
    }

    public class CampaignPageItem
    {
        public CampaignPage CampaignPage { get; set; }
        public List<CampaignBlock> CampaignBlocks{ get; set; }
    }
}