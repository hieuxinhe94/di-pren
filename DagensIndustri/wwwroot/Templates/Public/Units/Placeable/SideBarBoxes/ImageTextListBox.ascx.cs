﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.WebControls;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class ImageTextListBox : EPiServer.UserControlBase
    {
        private int _pageID;

        public int PageID
        {
            get { return _pageID; }
            set { _pageID = value; }
        }

        public int maxCount = 3;

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

                if (EPiFunctions.HasValue(pd, "MaxCount"))
                    maxCount = Convert.ToInt32(pd["MaxCount"].ToString());

                if (EPiFunctions.HasValue(pd, "StartNode"))
                {
                    PageDataCollection pdc = EPiServer.DataFactory.Instance.GetChildren(pd["StartNode"] as PageReference);
                    ImageTextPageListBox.DataSource = pdc;
                }
                else if (EPiFunctions.HasValue(pd, "PagesCollection"))
                {
                    DiLinkCollection ImageTextPageListCollection = new DiLinkCollection(pd, "PagesCollection");
                    ImageTextPageListBox.DataSource = ImageTextPageListCollection.SelectedPages();
                }
                else
                {
                    this.Visible = false;
                }


                ImageTextPageListBox.MaxCount = maxCount;
                ImageTextPageListBox.DataBind();

                if (ImageTextPageListBox.DataCount <= 0)
                    this.Visible = false;
            }
            else
            {
                this.Visible = false;
            }
        }
    }
}