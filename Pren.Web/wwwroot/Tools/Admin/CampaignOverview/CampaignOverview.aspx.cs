using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using Di.Common.Logging;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Pren.Web.Business;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.Pages;

namespace Pren.Web.Tools.Admin.CampaignOverview
{
    [GuiPlugIn(
        Area = PlugInArea.AdminMenu,
        Description = "Översikt kampanjer",
        RequiredAccess = EPiServer.Security.AccessLevel.Administer,
        DisplayName = "Kampanjöversikt",
        UrlFromUi = "/Tools/Admin/CampaignOverview/CampaignOverview.aspx",
        SortIndex = 2061)]
    public partial class CampaignOverview : System.Web.UI.Page
    {
        private Injected<ContentLocator> ContentLocator { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (!IsPostBack)
            {
                ddlCampaignPageTypes.DataSource = GetCampaignPageTypes();
                ddlCampaignPageTypes.DataBind();

                ddlCampaignPageTypes.Items.Insert(0, new ListItem("Alla kampanjer", "all"));
            }

            base.OnLoad(e);
        }

        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);

            // Use EPiServer UI master page
            MasterPageFile = UriSupport.ResolveUrlFromUIBySettings("MasterPages/EPiServerUI.master");
        }


        private IEnumerable<PageType> GetCampaignPageTypes()
        {
            var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();
            var pageTypeRepos = new PageTypeRepository(contentTypeRepository);

            return pageTypeRepos.List().Where(t => t.Name.StartsWith("CampaignPage"))
                    .OrderByDescending(t => t.ID)
                    .Select(t => t);
        }

        private List<CampaignOverviewData> GetCampaigns(bool onlyPublished, bool onlyNormalRedirectType)
        {

            var campaigPages = ddlCampaignPageTypes.SelectedValue.Equals("all") ?
                FindPagesByPageType<CampaignPage>()
                    .Union(FindPagesByPageType<CampaignPageIframe>())
                    .Union(FindPagesByPageType<CampaignPageSplus>())
                    .ToList()
                    : ContentLocator.Service.FindPagesByPageType<CampaignPage>(ContentReference.RootPage, true, int.Parse(ddlCampaignPageTypes.SelectedValue));

            if (onlyPublished)
            {
                campaigPages = campaigPages.Where(p => p.CheckPublishedStatus(PagePublishedStatus.Published)).ToList();
            }

            if (onlyNormalRedirectType)
            {
                campaigPages = campaigPages.Where(p => (PageShortcutType)Enum.Parse(typeof(PageShortcutType), p["PageShortCutType"].ToString()) == PageShortcutType.Normal).ToList();
            }

            if (!string.IsNullOrEmpty(tbKayakCampaign.Text))
            {
                var filteredPages = new List<CampaignPage>();

                foreach (var campaignPage in campaigPages)
                {
                    var blocksOnPage = campaignPage.CampaignContentArea != null ? campaignPage.CampaignContentArea.Items.Select(block => block.GetContent()).OfType<CampaignBlock>().ToList() : new List<CampaignBlock>();

                    foreach (var campaignBlock in blocksOnPage)
                    {
                        if (
                            (campaignBlock.FirstCampaignPeriod.CampaignCardAndInvoice != null && campaignBlock.FirstCampaignPeriod.CampaignCardAndInvoice.Contains(tbKayakCampaign.Text)) ||
                            (campaignBlock.FirstCampaignPeriod.CampaignAutogiro != null && campaignBlock.FirstCampaignPeriod.CampaignAutogiro.Contains(tbKayakCampaign.Text)) ||
                            (campaignBlock.SecondCampaignPeriod.CampaignCardAndInvoice != null && campaignBlock.SecondCampaignPeriod.CampaignCardAndInvoice.Contains(tbKayakCampaign.Text)) ||
                            (campaignBlock.SecondCampaignPeriod.CampaignAutogiro != null && campaignBlock.SecondCampaignPeriod.CampaignAutogiro.Contains(tbKayakCampaign.Text)) ||
                            (campaignBlock.ThirdCampaignPeriod.CampaignCardAndInvoice != null && campaignBlock.ThirdCampaignPeriod.CampaignCardAndInvoice.Contains(tbKayakCampaign.Text)) ||
                            (campaignBlock.ThirdCampaignPeriod.CampaignAutogiro != null && campaignBlock.ThirdCampaignPeriod.CampaignAutogiro.Contains(tbKayakCampaign.Text)))
                        {
                            filteredPages.Add(campaignPage);
                        }
                    }
                }

                campaigPages = filteredPages;
            }

            return campaigPages.Select(c => new CampaignOverviewData
            {
                CampaignPage = c,
                BlockOverviewDatas = GetBlockOverviewData(c)
            }).ToList();
        }

        private IEnumerable<T> FindPagesByPageType<T>() where T : PageData
        {
            var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();

            return ContentLocator.Service.FindPagesByPageType<T>(ContentReference.StartPage, true,
                contentTypeRepository.Load<T>().ID);
        }

        private List<BlockOverviewData> GetBlockOverviewData(CampaignPage campaignPage)
        {
            var datas = new List<BlockOverviewData>();

            try
            {
                var blocksOnPage = campaignPage.CampaignContentArea != null ? campaignPage.CampaignContentArea.Items.Select(block => block.GetContent()).OfType<CampaignBlock>().ToList() : new List<CampaignBlock>();

                foreach (var campaignBlock in blocksOnPage)
                {
                    var data = new BlockOverviewData();
                    data.CampaignBlock = campaignBlock;

                    var usedOnPages = new List<CampaignPage>();

                    var campaigPages =
                        FindPagesByPageType<CampaignPage>()
                            .Union(FindPagesByPageType<CampaignPageIframe>())
                            .Union(FindPagesByPageType<CampaignPageSplus>());

                    foreach (var page in campaigPages)
                    {
                        if (page.CampaignContentArea == null || page.CampaignContentArea.Items == null ||
                            !page.CampaignContentArea.Items.Any())
                        {
                            continue;
                        }

                        if (page.CampaignContentArea.Items
                                .Any(b => b.ContentLink.ID == ((IContent)campaignBlock).ContentLink.ID) &&
                                page.ContentLink.ID != campaignPage.ContentLink.ID)
                        {
                            usedOnPages.Add(page);
                        }
                    }

                    data.UsedOn = usedOnPages;

                    datas.Add(data);

                }
            }
            catch (Exception exception)
            {
                var logger = ServiceLocator.Current.GetInstance<ILogger>();
                logger.Log(exception, "GetBlockOverviewData failed - " + (campaignPage != null ? campaignPage.ContentLink.ID.ToString() : "null"), LogLevel.Error, typeof(CampaignOverview));
            }
            
            return datas;
        }

        public string GetNicePageTypeName(CampaignPage page)
        {
            if (page.PageTypeName == "CampaignPage")
            {
                return "NETS - Kampanj";
            }
            else if (page.PageTypeName == "CampaignPageIframe")
            {
                return "KLARNA - Kampanj";
            }
            else if (page.PageTypeName == "CampaignPageSplus")
            {
                return "DIBS - Kampanj";
            }

            return string.Empty;
        }

        protected void ShowCampaignOverview(object sender, EventArgs e)
        {
            repCampaignOverview.DataSource = GetCampaigns(cbOnlyPublished.Checked, cbOnlyNormal.Checked);
            repCampaignOverview.DataBind();
        }

    }

    public class CampaignOverviewData
    {
        public CampaignPage CampaignPage { get; set; }
        public List<BlockOverviewData> BlockOverviewDatas { get; set; }
    }

    public class BlockOverviewData
    {
        public CampaignBlock CampaignBlock { get; set; }
        public List<CampaignPage> UsedOn { get; set; }
    }

}