using System;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Security;
using EPiServer.Web.WebControls;
using DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipPopup;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Static
{
    public partial class TopMainMenu : EPiServer.UserControlBase
    {
        #region Properties
        /// <summary>
        /// Gets or sets the MenuList for this control
        /// </summary>
        public MenuList MenuList
        {
            get { return TopMenu; }
            set { TopMenu = value; }
        }
        #endregion

        #region Events
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            TopMenu.PageLink = GetMainMenuContainer() ?? PageReference.StartPage;
            TopMenu.PageLoader.GetChildrenCallback = new HierarchicalPageLoader.GetChildrenMethod(LoadChildren);
            TopMenu.DataBind();
        }

        protected void SubMenuRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.SelectedItem)
            {
                PageData pd = e.Item.DataItem as PageData;
                if (!EPiFunctions.UserHasPageAccess(pd, EPiServer.Security.AccessLevel.Read))
                {
                     HyperLink menuItemHyperLink = EPiFunctions.HasValue(pd, "MenuImage")
                                        ? e.Item.FindControl("MenuItemWithImage$MenuItemWithImageHyperLink") as HyperLink
                                        : e.Item.FindControl("MenuItemWithoutImage$MenuItemWithoutImageHyperLink") as HyperLink;

                     if (menuItemHyperLink != null)
                     {
                         menuItemHyperLink.NavigateUrl = "#membership-required";
                         menuItemHyperLink.CssClass = "ajax";

                         DiGoldMembershipPopup diGoldMembershipPopup = EPiFunctions.FindDiGoldMembershipPopup(Page) as DiGoldMembershipPopup;
                         if (diGoldMembershipPopup != null)
                         {
                             string absoluteFriendlyURL = EPiFunctions.GetFriendlyAbsoluteUrl(pd);
                             //diGoldMembershipPopup.RegisterSetReturnURLScript(menuItemHyperLink, absoluteFriendlyURL);
                         }
                     }
                }
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Creates the collection for the main menu, adding the startpage
        /// </summary>
        private PageDataCollection LoadChildren(PageReference pageLink)
        {
            PageDataCollection pages = DataFactory.Instance.GetChildren(pageLink);
            //pages.Insert(0, DataFactory.Instance.GetPage(pageLink));
            return pages;
        }

        /// <summary>
        /// Gets the root page for the main menu.
        /// </summary>
        private PageReference GetMainMenuContainer()
        {
            PageBase page = Page as PageBase;
            return page == null ? null : page.CurrentPage["MainMenuContainer"] as PageReference;
        }

        public PageDataCollection GetChildrenForSecondLevel(PageData pd)
        {
            PageDataCollection pdc = new PageDataCollection();

            if(EPiFunctions.IsMatchingPageType(pd, pd.PageTypeID, "ConferenceStartPageType"))
            {
                PageDataCollection conferencePDC = EPiFunctions.FilterMenu(EPiServer.DataFactory.Instance.GetChildren(pd["ConferenceStartNode"] as PageReference));
                FilterForVisitor.Filter(conferencePDC);
                new FilterPropertySort("Date", FilterSortDirection.Ascending).Filter(conferencePDC);
                new FilterCount(3).Filter(conferencePDC);
                
                foreach(PageData conferencePage in conferencePDC)
                {
                    if(Convert.ToDateTime(conferencePage["Date"].ToString()) >= DateTime.Now)
                        pdc.Add(conferencePage);
                }

                foreach (PageData child in EPiFunctions.FilterMenu(EPiServer.DataFactory.Instance.GetChildren(pd.PageLink)))
                {
                    pdc.Add(child);
                }
            }
            else
            {
                PageDataCollection pageDataColl = EPiFunctions.FilterMenu(EPiServer.DataFactory.Instance.GetChildren(pd.PageLink));

                foreach (PageData pageData in pageDataColl)
                {
                    //If user doesn't have read access rights to the page in menu, but the page is a DiGold page, add the page to the menu
                    if (!EPiFunctions.UserHasPageAccess(pageData, AccessLevel.Read))
                    {
                        foreach (RawACE ace in pageData.ACL.ToRawACEArray())
                        {
                            if (ace.Name == "DiGold")
                            {
                                pdc.Add(pageData);
                                break;
                            }
                        }
                    }
                    else
                    {
                        pdc.Add(pageData);
                    }
                }
            }

            return pdc;
        }
        #endregion
    }
}