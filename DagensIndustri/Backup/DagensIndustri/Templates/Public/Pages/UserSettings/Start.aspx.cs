using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes;
using DIClassLib.EPiJobs.SyncSubs;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using System.Text;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class Start : DiTemplatePage
    {
        
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!User.Identity.IsAuthenticated)
            {
                HandleNotLoggedIn();
                return;
            }
        }


        private void HandleNotLoggedIn()
        {
            MySettingsMenu1.Visible = false;
            ShowMessage("/mysettings2/notloggedin", true, true);
        }

    }
}