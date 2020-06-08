using System;
using System.Web;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.BonnierDigital;
using DIClassLib.Misc;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class Start : DiTemplatePage, IUserSettingsPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            //base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();
            HandleNotLoggedIn();
        }

        private void HandleNotLoggedIn()
        {
            if (!User.Identity.IsAuthenticated)
            {
                MySettingsMenu1.Visible = false;
                UserMessageControl.ShowMessage("/mysettings2/notloggedin", true, true);
            }
        }

        public void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError)
        {
            UserMessageControl.ShowMessage(mess, isKey, isError);
        }
    }
}