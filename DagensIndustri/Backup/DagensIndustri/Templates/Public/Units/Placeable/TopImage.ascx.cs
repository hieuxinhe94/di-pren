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

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    // ********************************
    // Plug in used 
    // https://www.coderesort.com/p/epicode/wiki/ImageListProperty
    // ********************************

    public partial class TopImage : UserControlBase
    {
        #region Constants
        private const string NOT_LOGGED_IN = "ImageListNotLoggedIn";
        private const string LOGGED_IN = "ImageListLoggedIn";
        private const string LOGGED_IN_NOT_GOLD = "ImageListLoggedInNotGold";
        private const string SIGNUPFLOWIMAGE = "ImageFlow";
        #endregion
        
        #region Events
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            //if (!IsPostBack)
            {
                if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "SignUpFlowPageType") && Request.QueryString["eventId"] != null)
                {
                    PageData pd = EPiServer.DataFactory.Instance.GetPage(PageReference.Parse(Request.QueryString["eventId"].ToString()));
                    PropertyImageList imgList = pd.Property[SIGNUPFLOWIMAGE] as PropertyImageList;
                    ImageViewerRepeater.DataSource = imgList.SelectedLinkItems;
                    ImageViewerRepeater.DataBind();
                }
                else
                {
                    string propName = GetImageListPropertyName();

                    if (!(GetNoOfImageListItems(propName) > 0))
                    {
                        propName = NOT_LOGGED_IN;
                    }

                    ImageViewerPlaceHolder.Visible = GetNoOfImageListItems(propName) > 0;

                    if (ImageViewerPlaceHolder.Visible)
                    {
                        PropertyImageList imgList = CurrentPage.Property[propName] as PropertyImageList;
                        ImageViewerRepeater.DataSource = imgList.SelectedLinkItems;
                        ImageViewerRepeater.DataBind();
                    }
                }
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get the ImageList property name. 
        /// There is a fallback to return 'ImageListNotLoggedIn' if user is logged in (as DiGold or other) 
        /// but no ImageList has been provided for that group.
        /// </summary>
        /// <returns></returns>
        private string GetImageListPropertyName()
        {
            string propName = NOT_LOGGED_IN;
            if (System.Web.HttpContext.Current.User.Identity.IsAuthenticated)
            {
                if (System.Web.HttpContext.Current.User.IsInRole("DiGold"))
                    propName = GetNoOfImageListItems(LOGGED_IN) > 0 ? LOGGED_IN : NOT_LOGGED_IN;
                else
                    propName = GetNoOfImageListItems(LOGGED_IN_NOT_GOLD) > 0 ? LOGGED_IN_NOT_GOLD : NOT_LOGGED_IN;
            }

            return propName;
        }

        private int GetNoOfImageListItems(string propName)
        {
            int noOfImageListItems = 0;
            if (IsValue(propName))
            {
                PropertyImageList imgList = CurrentPage.Property[propName] as PropertyImageList;
                noOfImageListItems = imgList.SelectedLinkItems.Count;
            }

            return noOfImageListItems;
        }

        protected string GetItem(object item)
        {
            LinkItem linkItem = item as LinkItem;
            StringBuilder returnString = new StringBuilder();
            bool isPageLink = !string.IsNullOrEmpty(linkItem.PageLink.URL);

            //Add link start
            if (isPageLink)
                returnString.Append("<a href=\"" + linkItem.PageLink.URL + "\">");

            //Add image
            returnString.Append("<img src=\"" + linkItem.ImageLink.URL + "\" />");
            if (!string.IsNullOrEmpty(linkItem.ImageLink.AltText))
                returnString.Append("<p>" + linkItem.ImageLink.AltText + "</p>");

            //Add link end
            if (isPageLink)
                returnString.Append("</a>");

            return returnString.ToString();
        }

        #endregion
    }
}