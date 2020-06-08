using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Templates.Public.Pages;

namespace DagensIndustri.Templates.Public.Units.Placeable.Subscription
{
    public partial class SubscriptionMatrix : UserControlBase
    {
        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            SetActiveSubscriptionType();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Set which subscription type is active
        /// </summary>
        private void SetActiveSubscriptionType()
        {
            DiPremiumSubscriptionType.IsActive = DiPremiumSubscriptionType.SubscriptionType == ((SubscriptionFlow)Page).SubscriptionType;
            DiPremiumFooterSubscriptionType.IsActive = DiPremiumSubscriptionType.IsActive;            

            DagensIndustriSubscriptionType.IsActive = DagensIndustriSubscriptionType.SubscriptionType == ((SubscriptionFlow)Page).SubscriptionType;
            DagensIndustriFooterSubscriptionType.IsActive = DagensIndustriSubscriptionType.IsActive;

            DiDirectDebitSubscriptionType.IsActive = DiDirectDebitSubscriptionType.SubscriptionType == ((SubscriptionFlow)Page).SubscriptionType;
            DiDirectDebitFooterSubscriptionType.IsActive = DiDirectDebitSubscriptionType.IsActive;

            DiWeekendSubscriptionType.IsActive = DiWeekendSubscriptionType.SubscriptionType == ((SubscriptionFlow)Page).SubscriptionType;
            DiWeekendFooterSubscriptionType.IsActive = DiWeekendSubscriptionType.IsActive;

            DiPlusSubscriptionType.IsActive = DiPlusSubscriptionType.SubscriptionType == ((SubscriptionFlow)Page).SubscriptionType;
            DiPlusFooterSubscriptionType.IsActive = DiPlusSubscriptionType.IsActive;
        }

        protected long ConvertCampaignNo(string campNo)
        {
            long result;
            long.TryParse(campNo, out result);
            return result;
        }

        /// <summary>
        /// Set Di Gold checkbox
        /// </summary>
        public void SetDiGoldMember()
        {
            DiPremiumSubscriptionType.BecomeDiGoldSelected = ((SubscriptionFlow)Page).BecomeDiGoldMember;
            DiPremiumFooterSubscriptionType.BecomeDiGoldSelected = DiPremiumSubscriptionType.BecomeDiGoldSelected;

            DagensIndustriSubscriptionType.BecomeDiGoldSelected = ((SubscriptionFlow)Page).BecomeDiGoldMember;
            DagensIndustriFooterSubscriptionType.BecomeDiGoldSelected = DagensIndustriSubscriptionType.BecomeDiGoldSelected;

            DiDirectDebitSubscriptionType.BecomeDiGoldSelected = ((SubscriptionFlow)Page).BecomeDiGoldMember;
            DiDirectDebitFooterSubscriptionType.BecomeDiGoldSelected = DiDirectDebitSubscriptionType.BecomeDiGoldSelected;

            DiWeekendSubscriptionType.BecomeDiGoldSelected = ((SubscriptionFlow)Page).BecomeDiGoldMember;
            DiWeekendFooterSubscriptionType.BecomeDiGoldSelected = DiWeekendSubscriptionType.BecomeDiGoldSelected;

            DiPlusSubscriptionType.BecomeDiGoldSelected = ((SubscriptionFlow)Page).BecomeDiGoldMember;
            DiPlusFooterSubscriptionType.BecomeDiGoldSelected = DiPlusSubscriptionType.BecomeDiGoldSelected;

        }
        #endregion
    }
}