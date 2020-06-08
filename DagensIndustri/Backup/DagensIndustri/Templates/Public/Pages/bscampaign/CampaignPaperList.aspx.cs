using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.WebControls;
using EPiServer;

namespace DagensIndustri.Templates.Public.Pages.bscampaign
{
    public partial class CampaignPaperList : System.Web.UI.Page
    {

        private PageData _currentPage;
        public PageData CurrentPage
        {
            get
            {
                if (_currentPage == null)
                {
                    var pageIdRaw = Request.QueryString["id"];
                    int pageId;

                    if (int.TryParse(pageIdRaw, out pageId))
                    {
                        _currentPage = EPiServer.DataFactory.Instance.GetPage(new PageReference(pageId));
                        return _currentPage;
                    }
                }

                return _currentPage;
            }
        }

        public string Campaign {
            get
            {
                var campaign = Request.QueryString["c"];

                if (!string.IsNullOrEmpty(campaign))
                    return campaign;

                return null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Campaign == null)
                return;

            if (CurrentPage[Campaign + "IncludedProducts"] != null)
            {
                var pdc = new PageDataCollection();
                var productPages = CurrentPage[Campaign + "IncludedProducts"].ToString().Split(',');
                foreach (var product in productPages)
                {
                    try
                    {
                        pdc.Add(DataFactory.Instance.GetPage(new PageReference(int.Parse(product))));
                    }
                    catch 
                    {
                        //OK, editor deleted page (and ignored warning from EpiServer)
                        //GetPage will explode, no other way to check if page exist
                    }

                }
                PlPaperList.DataSource = pdc;
                PlSideBar.DataSource = pdc;
            }

            PlPaperList.DataBind();
            PlSideBar.DataBind();
        }

    }
}