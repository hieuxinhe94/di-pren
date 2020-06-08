using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class PageBox : EPiServer.UserControlBase
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
                if (EPiFunctions.HasValue(pd, "TargetPage"))
                {
                    PageData targetPage = EPiServer.DataFactory.Instance.GetPage(pd["TargetPage"] as PageReference);

                    HeadingLiteral.Text = targetPage.PageName.ToString();

                    if (EPiFunctions.HasValue(targetPage, "PuffText"))
                    {
                        MainBodyLiteral.Text = EPiFunctions.GetXHTMLWithoutEpiDiv(targetPage, "PuffText");
                    }
                }
                else
                {
                    this.Visible = false;
                }

            }
            else
            {
                this.Visible = false;
            }
        }
    }
}