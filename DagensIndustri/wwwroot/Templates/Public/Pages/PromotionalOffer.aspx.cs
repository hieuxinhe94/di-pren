using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.DbHandlers;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class PromotionalOffer : DiTemplatePage
    {
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            //Only when user is a subscriber and has not already received that promotional offer, is he/she allowed to get a promotional offer.
            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                HideControls();
                ShowMessage(string.Format(Translate("/common/message/loginforpromotionaloffer"), EPiFunctions.GetLoginPageUrl(CurrentPage), PromotionalOfferControl.PromotionalOfferName.ToLower()), false, true);
            }
            //else if(!System.Web.HttpContext.Current.User.IsInRole("DiGold") || !System.Web.HttpContext.Current.User.IsInRole("DiRegular"))
            //{
            //    HideControls();
            //    ShowMessage(string.Format(Translate("/common/message/noteligiblebleforoffer"), PromotionalOfferControl.PromotionalOfferName.ToLower()), false, false);
            //}
            else if (!PromotionalOfferControl.CanGetPromotionalOffer())
            {
                HideControls();
                ShowMessage(string.Format(Translate("/common/message/tempreceiveoffer"), PromotionalOfferControl.PromotionalOfferName.ToLower()), false, false);
            }
        }
      
        protected void OrderButton_Click(object sender, EventArgs e)
        {
            bool saved = PromotionalOfferControl.SaveData();
            if (saved)
            {
                OrderButton.Visible = false;
                PromotionalOfferPlaceHolder.Visible = false;
                ShowMessage("/common/message/thankyouorder", true, false);
            }
        }
        #endregion

        #region Methods

        /// <summary>
        /// Hide controls
        /// </summary>
        public void HideControls()
        {
            PromotionalOfferPlaceHolder.Visible = false;
            HeadingControl.Visible = false;
        }
        #endregion
    }
}