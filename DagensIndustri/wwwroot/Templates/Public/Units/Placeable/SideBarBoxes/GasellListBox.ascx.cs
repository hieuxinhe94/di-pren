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
using EPiServer.Filters;

namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class GasellListBox : EPiServer.UserControlBase
    {
        private int _pageID;

        public int PageID
        {
            get { return _pageID; }
            set { _pageID = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);


            PageData pd = new PageData();

            if (PageID > 0)
                pd = EPiServer.DataFactory.Instance.GetPage(new PageReference(PageID));
            else
                pd = CurrentPage;

            if (pd != null)
            {
                if (EPiFunctions.HasValue(pd, "Heading"))
                {
                    HeadingLiteral.Text = pd["Heading"].ToString();
                }

                if (EPiFunctions.HasValue(pd, "MainBody"))
                {
                    MainBodyLiteral.Text = EPiFunctions.GetXHTMLWithoutEpiDiv(pd, "MainBody");
                }

                if (EPiFunctions.HasValue(pd, "StartNode"))
                {
                    PageDataCollection pdc = new PageDataCollection();
                    
                    foreach(PageData page in EPiServer.DataFactory.Instance.GetChildren(pd["StartNode"] as PageReference))
                    {
                        if (EPiFunctions.IsMatchingPageType(page, page.PageTypeID, "GasellMeetingPageType") && Convert.ToDateTime(page["Date"].ToString()) > DateTime.Now)
                        {
                            pdc.Add(page);
                        }
                    }

                    GasellListBoxPageList.DataSource = pdc;
                }
                else if (EPiFunctions.HasValue(pd, "PagesCollection"))
                {
                    DiLinkCollection ImageTextPageListCollection = new DiLinkCollection(pd, "PagesCollection");
                    GasellListBoxPageList.DataSource = ImageTextPageListCollection.SelectedPages();
                }
                else
                {
                    this.Visible = false;
                }

                GasellListBoxPageList.DataBind();
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}