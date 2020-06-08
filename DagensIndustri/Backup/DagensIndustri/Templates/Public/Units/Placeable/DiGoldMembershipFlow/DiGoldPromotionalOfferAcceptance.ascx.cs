using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;

namespace DagensIndustri.Templates.Public.Units.Placeable.DiGoldMembershipFlow
{
    public partial class DiGoldPromotionalOfferAcceptance : UserControlBase
    {
        #region Properties
        /// <summary>
        /// Get/set visiblity of promotional offer section
        /// </summary>
        public bool ShowPromotionalOffer 
        {
            get
            {
                return ViewState["ShowPromotionalOffer"] != null && (bool)ViewState["ShowPromotionalOffer"];
            }
            set
            {
                ViewState["ShowPromotionalOffer"] = value;
            }
        }

        protected bool HasPromotionalOffer
        {
            get
            {
                return ShowPromotionalOffer && EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldPromotionalOfferPage") != null;
            }
        }

        /// <summary>
        /// Gets whether user accepted the promotional offer
        /// </summary>
        public bool AcceptedPromotionalOffer 
        {
            get
            {
                return HasPromotionalOffer && AcceptPromotionalOfferCheckBox.Checked;
            }
            set
            {
                AcceptPromotionalOfferCheckBox.Checked = value;
            }
        }
        
        #endregion 

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            DataBind();
        }
        #endregion

        #region Methods        
        /// <summary>
        ///  Get text for terms and conditions from promotional page
        /// </summary>
        /// <returns></returns>
        protected string GetTermsAndConditions()
        {
            string termsAndConditions = string.Empty;
            PageReference termsAndConditionsPageLink = EPiFunctions.SettingsPageSetting(CurrentPage, "DiGoldTermsPage") as PageReference;
            if (termsAndConditionsPageLink != null)
            {
                PageData termsPageData = EPiServer.DataFactory.Instance.GetPage(termsAndConditionsPageLink);
                
                termsAndConditions = string.Format(Translate("/digold/readacceptedterms"),
                                                    EPiFunctions.GetFriendlyAbsoluteUrl(termsPageData.PageLink),
                                                    !string.IsNullOrEmpty((string)termsPageData["Heading"]) ? (string)termsPageData["Heading"] : termsPageData.PageName
                                                    );
            }
            return termsAndConditions;
        }        
        #endregion
    }
}