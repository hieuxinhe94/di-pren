using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using System.Text;
using DagensIndustri.Templates.Public.Units.Placeable.AddressForm;
using DIClassLib.Misc;
using DIClassLib.DbHandlers;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.Subscriptions.CirixMappers;
using System.Data;
using DIClassLib.EPiJobs.SyncSubs;
using DagensIndustri.Tools.Classes;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class SubsSleeps : DiTemplatePage, IUserSettingsPage
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();
            HandleNotLoggedIn();
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (!IsPostBack && User.Identity.IsAuthenticated)
            {
                long subsno = 0;
                long.TryParse(Request.QueryString["sid"], out subsno);
                if (subsno == 0 || SubsSleep1.Subscriber == null || SubsSleep1.Subscriber.SubsActive.Any(sub => sub.Subsno == subsno) == false)
                {
                    ShowMessage("Inloggad användare saknar behörighet att redigera prenumerations-ID: " + subsno, false, true);
                    SubsSleep1.Visible = false;
                }
            }
        }

        private void HandleNotLoggedIn()
        {
            if (!User.Identity.IsAuthenticated)
            {
                MySettingsMenu1.Visible = false;
                SubsSleep1.Visible = false;
                ShowMessage("/mysettings2/notloggedin", true, true);
            }
        }

        public void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError)
        {
            ShowMessage(mess, isKey, isError);
        }
    }

    [Serializable]
    public class ObjHolderForSubsSleepFormClick
    {
        /// <summary>
        /// values: insert update delete
        /// </summary>
        public string SqlCommand;

        public SubsSleepsCirixMap SubsSleepsOrg;

        public ObjHolderForSubsSleepFormClick() { }

        public ObjHolderForSubsSleepFormClick(string sqlCommand, SubsSleepsCirixMap subsSleepsOrg)
        {
            SqlCommand = sqlCommand;
            SubsSleepsOrg = subsSleepsOrg;
        }
    }

}