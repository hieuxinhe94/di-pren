using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiCode.ImageList.SpecializedProperties;
using EPiCode.ImageList.Utils;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.SideBarBoxes
{
    public partial class ImageListBox : EPiServer.UserControlBase
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
                if (EPiFunctions.HasValue(pd, "Heading") && EPiFunctions.HasValue(pd, "ImageList"))
                {
                    HeadingLiteral.Text = pd["Heading"].ToString();

                    PropertyImageList imgList = pd.Property["ImageList"] as PropertyImageList;
                    ImageListRepeater.DataSource = imgList.SelectedLinkItems;
                    ImageListRepeater.DataBind();
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


        protected string GetItem(object item)
        {
            LinkItem linkItem = item as LinkItem;
            StringBuilder returnString = new StringBuilder();
            bool isPageLink = !string.IsNullOrEmpty(linkItem.PageLink.URL);

            //Add link start
            if (isPageLink)
            {
                if (String.IsNullOrEmpty(linkItem.PageLink.Target))
                {
                    returnString.Append("<a href=\"" + linkItem.PageLink.URL + "\">");
                }
                else
                {
                    returnString.Append("<a href=\"" + linkItem.PageLink.URL + "\" target=\"" + linkItem.PageLink.Target + "\">");
                }

            }
            

            //Add image
            returnString.Append("<img src=\"" + linkItem.ImageLink.URL + "\" class=\"logo\" />");
            //if (!string.IsNullOrEmpty(linkItem.ImageLink.AltText))
            //    returnString.Append("<p>" + linkItem.ImageLink.AltText + "</p>");

            //Add link end
            if (isPageLink)
                returnString.Append("</a>");

            return returnString.ToString();
        }
    }
}