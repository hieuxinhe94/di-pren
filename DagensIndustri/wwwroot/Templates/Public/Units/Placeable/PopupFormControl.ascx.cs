using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer.Core;
using EPiServer;
using EPiServer.Web;
using DagensIndustri.Tools.Classes.WebControls;

namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class PopupFormControl : EPiServer.UserControlBase
    {
        public PageData SettingsPage { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            
            if (CurrentPage["PopUpSettingsPage"] != null)
            {
                try
                {
                    SettingsPage = DataFactory.Instance.GetPage(CurrentPage["PopUpSettingsPage"] as PageReference);                    
                    //If linkcollection has items, check if popup should be used on this page
                    var applyOnPages = new DiLinkCollection(SettingsPage, "PopUpPages").SelectedPages();
                    if (applyOnPages.Count > 0)
                    {
                        this.Visible = applyOnPages.Any(t => t.PageLink == CurrentPage.PageLink);
                    }

                    if (this.Visible)
                    {
                        PropBody.PageLink = SettingsPage.PageLink;
                        //If an form is used, connect it to settingspage
                        if (SettingsPage.GetValue("XForm") != null)
                        {
                            Xform1.XFormPage = SettingsPage;
                            Xform1.SetupForm();
                            XFormPanel.Visible = true;
                        }
                    }
                }
                catch (Exception)
                {
                    //Popup page has been moved
                    this.Visible = false;
                }

            }
            else {
                this.Visible = false;
            }
        }
    }
}