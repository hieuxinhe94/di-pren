using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.MasterPages
{
    public partial class MasterPage : System.Web.UI.MasterPage
    {
        #region Properties
        public string NavClass { get; set; }
        public string ContentWrapperClass { get; set; }
        #endregion

        #region Events

        protected override void OnInit(System.EventArgs e)
        {
            base.OnInit(e);

            AddBodyClassForCampaign();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            SetClassOnMenu();
            SetClassOnDiv();

            //121116 removed - broken xml received
            //SetBookShelfVisibility();
        }
        #endregion

        #region Methods
        protected void SetClassOnMenu()
        {
            var page = (EPiServer.PageBase)Page;

            NavClass = "";

            if (page.CurrentPage["MasterPageSelector"] != null)
            {
                if(page.CurrentPage["MasterPageSelector"].ToString() != "/Templates/Public/Styles/dagensindustri.css")
                {
                    NavClass = "class=\"gold\"";
                }  
            }            
        }

        protected void SetClassOnDiv()
        {
            var page = (EPiServer.PageBase)Page;
            if (EPiFunctions.IsMatchingPageType(page.CurrentPage, page.CurrentPage.PageTypeID, "AddContactCompanyPageType") ||
                EPiFunctions.IsMatchingPageType(page.CurrentPage, page.CurrentPage.PageTypeID, "UpdateCompanyDetailsPageType") ||
                EPiFunctions.IsMatchingPageType(page.CurrentPage, page.CurrentPage.PageTypeID, "ContactCompanySearchPageType"))
            {
                ContentWrapperClass = "class=\"clearFix\"";
            }
        }

        protected void AddBodyClassForCampaign()
        {
            var page = (EPiServer.PageBase)Page;
            if (EPiFunctions.IsMatchingPageType(page.CurrentPage, page.CurrentPage.PageTypeID, "CampaignPageType"))
            {
                Body.Attributes.Add("class", "campaign-tool");
            }
        }

        //121116 removed - broken xml received
        /// <summary>
        /// Set the visibility of the book shelf
        /// </summary>
        //private void SetBookShelfVisibility()
        //{
        //    if (BookShelfControl == null)
        //        return;

        //    bool visible = false;

        //    PageData pd = ((EPiServer.PageBase)Page).CurrentPage;
        //    PropertyBoolean showBookshelfProperty = pd.Property["ShowBookshelf"] as PropertyBoolean;

        //    if (showBookshelfProperty != null)
        //    {
        //        //If the value of the property is true, check if it is a dynamic property. If so, set the visibility of the book shelf with that value 
        //        // and not with the parent's value. This is done because an EPiServer "Selected/Not selected" property is either null or true. If this property 
        //        //is null in a dynamic property on a page, the parent page's value is used which is not correct in this case.
        //        if ((bool)showBookshelfProperty.Value && showBookshelfProperty.IsDynamicProperty)
        //        {
        //            DynamicProperty showBookShelfDynamicProp = DynamicProperty.Load(pd.PageLink, "ShowBookshelf");
        //            if (showBookShelfDynamicProp != null)
        //            {
        //                visible = !showBookShelfDynamicProp.PropertyValue.IsNull;
        //            }
        //        }
        //        else
        //        {
        //            visible = (bool)showBookshelfProperty.Value;
        //        }
        //    }

        //    BookShelfControl.Visible = visible;
        //}

        public void ShowSideBarBoxes(bool show)
        {
            SidebarBoxes.Visible = show;
        }

        public void ShowHeader(bool show)
        {
            phHeader.Visible = show;
        }

        public void ShowFooter(bool show)
        {
            ucFooter.Visible = show;
        }
        #endregion
    }
}