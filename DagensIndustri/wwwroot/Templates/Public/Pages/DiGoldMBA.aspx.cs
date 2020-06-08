using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using System.Text;
using DIClassLib.EPiJobs.SyncSubs;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class DiGoldMBA : DiTemplatePage
    {
        string _urlCode = "code";

        private SubscriptionUser2 _subUser = null;
        public SubscriptionUser2 SubUser
        {
            get
            {
                if (_subUser != null)
                    return _subUser;

                if (HttpContext.Current.User.Identity.IsAuthenticated)
                {
                    _subUser = new SubscriptionUser2();
                    return _subUser;
                }

                return null;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;

            if (!IsPostBack)
            {
                //populate by code in url
                if (!string.IsNullOrEmpty(Request.QueryString[_urlCode]))
                {
                    MssqlCustomer mssqlCust = new MssqlCustomer(Request.QueryString[_urlCode]);
                    if (mssqlCust != null)
                    {
                        long cusno = mssqlCust.Cusno;

                        //sync cust to mssql login tables: 1=ok, -1=no active subs, -2=no cust info in cirix
                        if (cusno > 0 && SyncSubsHandler.SyncCustToMssqlLoginTables((int)cusno) == 1)
                        {
                            SubscriptionUser2 user = new SubscriptionUser2(cusno);
                            if (DIClassLib.Misc.LoginUtil.ReLoginUserRefreshCookie(user.UserName, user.Password))
                                Response.Redirect(EPiFunctions.GetFriendlyUrl(CurrentPage));
                        }
                    }
                }
            }

            if (!HttpContext.Current.User.Identity.IsAuthenticated)
            {
                string s = "<a href='" + EPiFunctions.GetLoginPageUrl(CurrentPage) + "'>Klicka här</a> för att logga in och skicka din ansökan.";
                UserMessageControl.ShowMessage(s, false, false);
                DiMBA.Visible = false;
                return;

                #region old code
                //ShowMessage(string.Format(Translate("/promofferjoingold/loginforpromotionaloffer"), OfferName, EPiFunctions.GetLoginPageUrl(CurrentPage)), false, true);
                //UserMessageControl.ShowMessage("/common/message/loginforservice", true, true);
                //MainBody.Visible = false;
                //MainIntro.Visible = false;
                //DataBind();
                // LabelThankYou.Text = "<p>" + Translate("/common/message/loginforservice") + "</p>";
                // LabelThankYou.Visible = true;
                #endregion
            }
        }

    }
}