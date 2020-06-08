using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using System.Globalization;
using System.Data;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Units.Placeable.Subscription
{
    public partial class SubscriptionWelcome : UserControlBase
    {
        #region Properties
        protected bool IsDiWeekend
        {
            get
            {
                return Session["NewSubscriptionType"] != null && 
                    ((DIClassLib.Subscriptions.SubscriptionType.TypeOfSubscription)Session["NewSubscriptionType"] == DIClassLib.Subscriptions.SubscriptionType.TypeOfSubscription.DiWeekend);
            }
        }
        #endregion

        #region Events
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);           
            DataBind();
        }

        protected void BecomeDiGoldMemberLinkButton_Click(object sender, EventArgs e)
        {
            EPiFunctions.RedirectToPage(Page, EPiFunctions.GetDiGoldFlowPage(CurrentPage).PageLink, EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage));
        }
        #endregion

        #region Methods
        /// <summary>
        /// If the new subscription's start date was stored in session, get the formatted date from session.
        /// Otherwise, get it from the querystring.
        /// </summary>
        /// <returns></returns>
        protected string GetSubscriptionStartDate()
        {
            string subscriptionStartDateFormat = string.Empty;
            if (Session["NewSubscriptionStartDate"] != null)
            {
                subscriptionStartDateFormat = GetSubscriptionStartDateBySession();
            }
            else
            {
                subscriptionStartDateFormat = GetSubscriptionStartDateByQueryString();
            }

            return subscriptionStartDateFormat;
        }

        /// <summary>
        /// Get the new subscription's start date stored in session.
        /// </summary>
        /// <returns></returns>
        private string GetSubscriptionStartDateBySession()
        {
            DateTime subscriptionStartDate = Convert.ToDateTime(Session["NewSubscriptionStartDate"]);
            return FormatSubscriptionStartDate(subscriptionStartDate);
        }

        /// <summary>
        /// Get the new subscription's start date stored in QueryString.
        /// </summary>
        /// <returns></returns>
        protected string GetSubscriptionStartDateByQueryString()
        {
            string subscriptionStartDateFormat = string.Empty;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
            {
                DateTime subscriptionStartDate = DateTime.MinValue;
                string date = Request.QueryString["std"] as string;
                if (date != null && date.Length >= 8)
                {
                    date = string.Format("{0}-{1}-{2}", date.Substring(0, 4), date.Substring(4, 2), date.Substring(6,2));
                    subscriptionStartDateFormat = FormatSubscriptionStartDate(Convert.ToDateTime(date));
                }
            }
            return subscriptionStartDateFormat;
        }

        /// <summary>
        /// Format subscription start date to be of format "Thursday 15 april"
        /// </summary>
        /// <param name="startDate"></param>
        /// <returns></returns>
        private string FormatSubscriptionStartDate(DateTime startDate)
        {
            return startDate != DateTime.MinValue ? startDate.ToString("dddd d MMMM", new CultureInfo("sv-SE")) : string.Empty;
        }
        #endregion
    }
}