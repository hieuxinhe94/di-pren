using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;

namespace DagensIndustri.Templates.Public.Pages.SideBar
{
    public partial class SideBarItem : DiTemplatePage
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "HtmlBoxPageType"))
                HtmlBox.Visible = true;
            
            if(EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ImageListBoxPageType"))
                ImageListBox.Visible = true;

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "PageListBoxPageType"))
                TextListBox.Visible = true;

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ImageTextListBoxPageType"))
                ImageTextListBox.Visible = false;

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "GasellListBoxPageType"))
                GasellListBox.Visible = true;

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "PageBoxPageType"))
                PageBox.Visible = true;

            if(EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "ImageBoxPageType"))
                ImageBox.Visible = true;

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "WineListPageType"))
                WineList.Visible = true;

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "TwitterBoxPageType"))
                TwitterBox.Visible = true;

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "GoogleTranslateBoxPageType"))
                GoogleTranslateBox.Visible = true;

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "AddThisPageType"))
                AddThis.Visible = true;

            if (EPiFunctions.IsMatchingPageType(CurrentPage, CurrentPage.PageTypeID, "OnlineSupportBoxPageType"))
                OnlineSupportBox.Visible = true;
        }
    }
}