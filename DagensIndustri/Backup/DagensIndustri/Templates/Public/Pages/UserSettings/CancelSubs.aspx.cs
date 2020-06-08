using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using EPiServer;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class CancelSubs : DiTemplatePage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!User.Identity.IsAuthenticated)
                HandleNotLoggedIn();
        }

        private void HandleNotLoggedIn()
        {
            MySettingsMenu1.Visible = false;
            ShowMessage("/mysettings2/notloggedin", true, true);
        }
    }
}