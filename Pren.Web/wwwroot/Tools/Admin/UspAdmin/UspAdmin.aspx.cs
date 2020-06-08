using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.PlugIn;
using EPiServer.ServiceLocation;
using Pren.Web.Business;
using Pren.Web.Business.DataAccess;
using Pren.Web.Business.DataAccess.Usp;
using Pren.Web.Business.DataAccess.Usp.Entities;
using Pren.Web.Models.Blocks;
using Pren.Web.Models.Pages;

namespace Pren.Web.Tools.Admin.UspAdmin
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Administrera USP-texter", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "USP, admin", UrlFromUi = "/Tools/Admin/UspAdmin/UspAdmin.aspx", SortIndex = 1100)]
    public partial class UspAdmin : System.Web.UI.Page
    {
        private Injected<ContentLocator> ContentLocator { get; set; }

        protected override void OnLoad(EventArgs e)
        {
            if (IsPostBack)
            {
                AddUsedOn(int.Parse(DdlProducts.SelectedValue));
            }
            else
            {
                AddUsedOn(new UspRepository().GetUspProducts().First().Id);
            }

            base.OnLoad(e);
        }

        protected void BtnAddProduct(object sender, EventArgs e)
        {
            var repos = new UspRepository();
            repos.AddUspProduct(TxtUspProduct.Text);
        }

        protected void BtnAddProductItem(object sender, EventArgs e)
        {
            var repos = new UspRepository();
            repos.AddUspText(int.Parse(DdlProducts.SelectedValue), TxtUspProductItem.Text);
            TxtUspProductItem.Text = string.Empty;
        }

        protected void DdlProducts_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            //AddUsedOn(int.Parse(DdlProducts.SelectedValue));
        }

        private void AddUsedOn(int uspId)
        {
            var usedOn = new List<UsedOn>();

            var campaignPages = FindPagesByPageType<CampaignPage>()
                .Union(FindPagesByPageType<CampaignPageIframe>())
                .Union(FindPagesByPageType<CampaignPageSplus>())
                .ToList();


            foreach (var campaignPage in campaignPages)
            {
                var blocksOnPage = campaignPage.CampaignContentArea != null ? campaignPage.CampaignContentArea.Items.Select(block => block.GetContent()).OfType<CampaignBlock>().ToList() : new List<CampaignBlock>();

                foreach (var campaignBlock in blocksOnPage)
                {
                    if (campaignBlock.UspProduct == uspId)
                    {
                        usedOn.Add(new UsedOn
                        {
                            BlockName = campaignBlock.Heading,
                            PageName = campaignPage.Name,
                            PageUrl = campaignPage.LinkURL
                        });
                    }
                }
            }

            repUsedOn.DataSource = usedOn;
            repUsedOn.DataBind();
        }

        private IEnumerable<T> FindPagesByPageType<T>() where T : PageData
        {
            var contentTypeRepository = ServiceLocator.Current.GetInstance<IContentTypeRepository>();

            return ContentLocator.Service.FindPagesByPageType<T>(ContentReference.StartPage, true,
                contentTypeRepository.Load<T>().ID);
        }
    }

    public class UspRepository
    {
        private readonly IDataAccess _dataAccess;

        public UspRepository()
        {
            _dataAccess = ServiceLocator.Current.GetInstance<IDataAccess>();
        }

        public void AddUspText(int productId, string uspText)
        {
            _dataAccess.UspHandler.AddUspText(productId, uspText);
        }

        public void AddUspProduct(string text)
        {
            _dataAccess.UspHandler.AddUspProduct(text);
        }

        public IEnumerable<UspProductEntity> GetUspProducts()
        {
            return _dataAccess.UspHandler.GetUspProducts();
        }

        public IEnumerable<UspTextEntity> GetUspTexts(int productId)
        {
            return _dataAccess.UspHandler.GetUspTexts(productId);
        }

        public object UpdateUspText(int id, string text)
        {
            _dataAccess.UspHandler.UpdateUspText(id, text);

            return null;
        }

        public object DeleteUspText(int id)
        {
            _dataAccess.UspHandler.DeleteUspText(id);

            return null;
        }
    }

    public class UsedOn
    {
        public string PageName { get; set; }
        public string BlockName { get; set; }
        public string PageUrl { get; set; }
    }
}