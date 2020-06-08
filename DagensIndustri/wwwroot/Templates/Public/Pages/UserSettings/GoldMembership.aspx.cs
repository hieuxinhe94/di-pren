using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes;
using DIClassLib.Membership;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.DbHandlers;
using DIClassLib.GoldMember;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class GoldMembership : DiTemplatePage, IUserSettingsPage
    {
        public SubscriptionUser2 Subscriber
        {
            get
            {
                if (ViewState["Subscriber"] == null)
                    ViewState["Subscriber"] = new SubscriptionUser2();

                return (SubscriptionUser2)ViewState["Subscriber"];
            }
            set
            {
                ViewState["Subscriber"] = value;
            }
        }


        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!User.Identity.IsAuthenticated)
            {
                HideControls(false);
                ShowMessage("/mysettings2/notloggedin", true, true);
                return;
            }

            if (User.IsInRole(DiRoleHandler.RoleDiGold))
            {
                HandleButtonVisibility(true);
                return;
            }

            if (!GoldRuleEnforcer.UserPassesGoldRules(Subscriber.Cusno))
            {
                HideControls(true);
                ShowMessage("/digold/missingsubscriptionsdetails2", true, false);
            }
            else
                HandleButtonVisibility(false);
        }

        private void HandleButtonVisibility(bool isMember)
        {
            ActivateDiGoldButton.Visible = !isMember;
            DeactivateDiGoldButton.Visible = isMember;
        }

        private void HideControls(bool showMenu)
        {
            MySettingsMenu1.Visible = showMenu;
            ActivateDiGoldButton.Visible = false;
            DeactivateDiGoldButton.Visible = false;
        }


        protected void DIGoldMembership_Click(object sender, EventArgs e)
        {
            string cmd = ((Button)sender).CommandArgument.ToUpper();

            if (cmd == "START")
            {
                EPiFunctions.RedirectToPage(Page, EPiFunctions.GetDiGoldFlowPage(CurrentPage).PageLink, EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage));
            }
            else if (cmd == "END")
            {
                DiRoleHandler.RemoveUserFromRoles(new string[] { DiRoleHandler.RoleDiGold });
                new CustomerPropertyHandler(Subscriber.Cusno, null, null, null, false, null);
                HandleButtonVisibility(false);
                ShowMessage("Ditt Di Guld medlemskap har avslutats", false, false);
            }
        }

        public void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError)
        {
            ShowMessage(mess, isKey, isError);
        }
    }
}