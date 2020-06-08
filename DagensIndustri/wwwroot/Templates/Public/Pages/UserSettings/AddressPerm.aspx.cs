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
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions.CirixMappers;
using DagensIndustri.Templates.Public.Units.Placeable.AddressForm;
using DIClassLib.Membership;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class AddressPerm : DiTemplatePage, IUserSettingsPage
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
                AddrPerm1.Visible = false;
                ShowMessage("/mysettings2/notloggedin", true, true);
            }
        }

        public void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError)
        {
            ShowMessage(mess, isKey, isError);
        }
    }
}