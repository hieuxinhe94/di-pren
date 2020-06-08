using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.Web.WebControls;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class SidebarSubMenu : EPiServer.UserControlBase
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            SubMenuList.PageLink = GetMainMenuContainer() ?? PageReference.StartPage;
            SubMenuList.PageLoader.GetChildrenCallback = new HierarchicalPageLoader.GetChildrenMethod(LoadChildren);
            SubMenuList.DataBind();

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ConferencePageType")
                || EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ConferenceStartPageType")
                || EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ConferenceStartEnPageType"))
            {
                if (LoadChildren(CurrentPage.PageLink).Count <= 1)
                    this.Visible = false;
            }
            else
            {
                if (LoadChildren(CurrentPage.ParentLink).Count <= 1)
                    this.Visible = false;
            }
        }

        /// <summary>
        /// Creates the collection for the main menu, adding the startpage
        /// </summary>
        private PageDataCollection LoadChildren(PageReference pageLink)
        {
            PageDataCollection pages = new PageDataCollection();
            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ConferencePageType")
                || EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ConferenceStartPageType")
                || EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ConferenceStartEnPageType"))
            {
                pages = DataFactory.Instance.GetChildren(pageLink);
                pages.Insert(0, DataFactory.Instance.GetPage(pageLink));
            }
            else
            {
                pages = DataFactory.Instance.GetChildren(CurrentPage.ParentLink);
                pages.Insert(0, DataFactory.Instance.GetPage(CurrentPage.ParentLink));
            }
            return pages;
        }

        /// <summary>
        /// Gets the root page for the main menu.
        /// </summary>
        private PageReference GetMainMenuContainer()
        {
            PageBase page = Page as PageBase;
            PageReference SubMenuStart = CurrentPage.PageLink;

            if (!EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ConferencePageType")
                && !EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ConferenceStartPageType")
                && !EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ConferenceStartEnPageType"))
            {
                SubMenuStart = CurrentPage.ParentLink;
            }

            return page == null ? null : SubMenuStart; //page.CurrentPage["MainMenuContainer"] as PageReference;
        }

        /// <summary>
        /// Gets or sets the MenuList for this control
        /// </summary>
        public MenuList MenuList
        {
            get { return SubMenuList; }
            set { SubMenuList = value; }
        }
    }
}