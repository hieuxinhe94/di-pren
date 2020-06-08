using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using System.Text;
using System.Configuration;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class Complaint : DiTemplatePage, IUserSettingsPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();
            HandleNotLoggedIn();
        }

        private void HandleNotLoggedIn()
        {
            if (!User.Identity.IsAuthenticated)
            {
                MySettingsMenu1.Visible = false;
                Complaint1.Visible = false;
                ShowMessage("/mysettings2/notloggedin", true, true);
            }
        }

        public void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError)
        {
            ShowMessage(mess, isKey, isError);
        }
    }
}