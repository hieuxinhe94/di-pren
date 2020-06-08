using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DagensIndustri.Tools.Classes.Extras;
using DIClassLib.Membership;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using DIClassLib.GoldMember;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class DiGoldMembershipFlow : DiTemplatePage
    {
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            if (!IsPostBack)
            {
                if (HttpContext.Current.User.Identity.IsAuthenticated && HttpContext.Current.User.IsInRole(DiRoleHandler.RoleDiGold))
                    Response.Redirect(EPiFunctions.GetFriendlyAbsoluteUrl(EPiFunctions.GetDiGoldStartPage()), true);

                DiGoldFlowMultiView.ActiveViewIndex = 0;
            }
            
            if (TryPopulateSubscriptionUser() && UserPassesGoldRules())
            {
                //Check user is a subscriber and has a personal subscription
                PromotionalOfferControl.CustomerNumber = AdditionalUserDetailsControl.Subscriber.Cusno;
                AdditionalUserDetailsControl.PromotionalOfferName = PromotionalOfferControl.PromotionalOfferName;
            }
        }

        protected void ContinueButton_Click(object sender, EventArgs e)
        {
            UserMessageControl.ClearMessage();

            bool readyToSave = false;

            //If current view is the additional user details' view, check if the data is valid before moving to next step or
            //saving data (if no promotional offer was accepted)
            if (DiGoldFlowMultiView.ActiveViewIndex == 0)
            {
                if (AdditionalUserDetailsControl.IsValid())
                {
                    if (AdditionalUserDetailsControl.AcceptedPromotionalOffer)
                    {
                        DiGoldFlowMultiView.ActiveViewIndex++;
                        BackLinkButton.Visible = DiGoldFlowMultiView.ActiveViewIndex > 0;
                    }
                    else
                    {
                        readyToSave = true;
                    }
                }
            }
            else if (DiGoldFlowMultiView.ActiveViewIndex == 1)
            {
                readyToSave = PromotionalOfferControl.IsValid();
            }

            if (readyToSave)
            {
                //Save user details
                bool saved = AdditionalUserDetailsControl.SaveData();

                //If everything was ok and promotional offer existed and was accepted, save that data as well
                if (saved && AdditionalUserDetailsControl.AcceptedPromotionalOffer)
                    saved = PromotionalOfferControl.SaveData();
                
                //If everything was OK, redirect user to the page he/she was trying to access
                if (saved)
                    EPiFunctions.RedirectToReturnUrlOrGoldStartPage(Page, "ReturnUrl");
            }
        }

        protected void BackLinkButton_Click(object sender, EventArgs e)
        {
            DiGoldFlowMultiView.ActiveViewIndex--;
            BackLinkButton.Visible = DiGoldFlowMultiView.ActiveViewIndex > 0;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get heading of the page, depending on active view
        /// </summary>
        /// <returns></returns>
        protected string GetHeading()
        {
            string heading = string.Empty;
            if (DiGoldFlowMultiView.ActiveViewIndex == 0)
            {
                heading = AdditionalUserDetailsControl.Heading;
            }
            else
            {
                heading = PromotionalOfferControl.Heading;
            }
            return heading;
        }

        /// <summary>
        /// Get the subscription user object for current user
        /// </summary>
        /// <returns></returns>
        private bool TryPopulateSubscriptionUser()
        {
            AdditionalUserDetailsControl.Subscriber = new SubscriptionUser2();

            if (AdditionalUserDetailsControl.Subscriber.Cusno > 0)
                return true;
            
            HideControls();
            ShowMessage("/common/message/digold/nocusno", true, false);
            return false;
        }

        /// <summary>
        /// Check if the subscription is the current user's personal subscription. If not, hide all the controls and show a message to the user.
        /// </summary>
        private bool UserPassesGoldRules()
        {
            if (!GoldRuleEnforcer.UserPassesGoldRules(AdditionalUserDetailsControl.Subscriber.Cusno))
            {
                HideControls();
                ShowMessage("/digold/missingsubscriptionsdetails2", true, false);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Hide all the placeholders
        /// </summary>
        private void HideControls()
        {
            HeadingPlaceHolder.Visible = false;
            MultiViewPlaceHolder.Visible = false;
        }
        #endregion
    }
}