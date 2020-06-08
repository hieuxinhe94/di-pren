using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Wine;
using System.Data;
using DIClassLib.Subscriptions;
using DIClassLib.EPiJobs.Apsis;
using System.Configuration;
using EPiServer.Core;
using DagensIndustri.Tools.Classes;
using DIClassLib.Membership;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.EPiJobs.SyncSubs;
using DIClassLib.Misc;

namespace DagensIndustri.Templates.Public.Pages
{
    public partial class DiGoldWine : DiTemplatePage
    {
        protected List<Wine> Wines { get; set; }
        protected List<WineCharacter> WineCharacters { get; set; }
        private bool? _isWineSubscriber { get; set; }

        protected bool HasAccess
        {
            get
            {
                if (User.Identity.IsAuthenticated && User.IsInRole(DiRoleHandler.RoleDiGold))
                {
                    return true;
                }
                else
                {
                    return false;
                }

                
            }
        }

        public string UrlCode
        {
            get
            {
                if (Request.QueryString["code"] == null)
                    return string.Empty;

                return Request.QueryString["code"].ToString();
            }
        }

        private String SubscriberPhoneNumber
        {
            get
            {
                return (string)ViewState["SubscriberPhoneNumber"];
            }
            set
            {
                ViewState["SubscriberPhoneNumber"] = value;
            }
        }

        SubscriptionUser2 SubUser { get; set; }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (UrlCode != String.Empty)
            {
                TryPopulateSubUserByUrlCode();
            }
            if(SubUser == null)
                SubUser = new SubscriptionUser2();
            bool b = UserIsWineSubscriber;
            if (!IsPostBack)
            {
                DataBind();
            }

            if (Session["SuccessMessage"] != null) 
            {
                // Message from the page that starts the subscription
                ShowMessage((String)Session["SuccessMessage"], false, false);
                Session["SuccessMessage"] = null;
            }

            
            

        }

        protected override void OnDataBinding(EventArgs e)
        {
            base.OnDataBinding(e);
            Wines = WineHandler.GetWinesWithCharacters().Where(x => x.Date <= DateTime.Now).ToList();
            WineCharacters = new List<WineCharacter>();
            DataTable dtCharacters = WineHandler.GetWineCharacters();
            if (dtCharacters != null && dtCharacters.Rows != null)
            {
                foreach (DataRow dr in dtCharacters.Rows)
                {
                    WineCharacter wc = new WineCharacter()
                    {
                        Id = (int)dr["Id"],
                        Name = (string)dr["Name"]
                    };
                    WineCharacters.Add(wc);
                }
            }
        }

        protected String WinesJson
        {
            get
            {
                System.Web.Script.Serialization.JavaScriptSerializer oSerializer =
                    new System.Web.Script.Serialization.JavaScriptSerializer();
                string sJSON = oSerializer.Serialize(Wines);
                return sJSON;
            }
        }

        protected bool UserIsWineSubscriber
        {
            get
            {
                if (!_isWineSubscriber.HasValue)
                {

                    if (SubUser == null || SubUser.Cusno == 0)
                        return false;
                    int apsisListId = 0;
                    if (CurrentPage.Property["ApsisListId"] != null)
                    {
                        apsisListId = (int)CurrentPage.Property["ApsisListId"].Value;
                        
                    }

                   /*
                    List<String> phonenumbers = new List<string>();
                    if (SubUser.OPhone != null)
                        phonenumbers.Add(MiscFunctions.FormatPhoneNumber(SubUser.OPhone, Settings.PhoneMaxNoOfDigits, true));
                    if (SubUser.HPhone != null)
                        phonenumbers.Add(MiscFunctions.FormatPhoneNumber(SubUser.HPhone, Settings.PhoneMaxNoOfDigits, true));
                    if (SubUser.WPhone != null)
                        phonenumbers.Add(MiscFunctions.FormatPhoneNumber(SubUser.WPhone, Settings.PhoneMaxNoOfDigits, true));
                    */
                    

                    ApsisWsHandler awh = new ApsisWsHandler();
                    //ApsisListSubscriber sub = new ApsisListSubscriber(apsisListId.ToString(), "", "", "", phonenumbers);
                    //sub = awh.TryGetApsisListSubscriber(sub);
                    //ApsisListSubscriber sub = awh.TryGetApsisListSubscriberByPhonenumbers(apsisListId.ToString(), phonenumbers.ToArray());
                    var sub = awh.TryGetApsisListSubscriberByCusno(apsisListId.ToString(), SubUser.Cusno.ToString());
                    if (sub != null && !string.IsNullOrEmpty(sub.PhoneMob))
                    {
                        _isWineSubscriber = true;
                        SubscriberPhoneNumber = sub.PhoneMob;
                    }
                    else
                    {
                        _isWineSubscriber = false;
                    }
                }
                return _isWineSubscriber.Value;
            }
        }

        protected void btnStartSubscription_Click(object sender, EventArgs e)
        {
            SendToStartSubscriptionPage();
        }

        private void SendToStartSubscriptionPage()
        {
            if (CurrentPage.Property["StartSubscriptionPage"] != null)
            {
                String startSubscriptionUrl = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage["StartSubscriptionPage"] as PageReference);
                startSubscriptionUrl += "?ReturnUrl=" + Server.UrlEncode(Request.Url.PathAndQuery);
                if (!User.Identity.IsAuthenticated)
                {
                    String url = "/sso-login/?ReturnUrl=" + Server.UrlEncode(startSubscriptionUrl);
                    Response.Redirect(url);
                    return;
                }
                else
                {

                    Response.Redirect(startSubscriptionUrl);
                }
            }
            

        }

        protected void btnSendSms_Click(object sender, EventArgs e)
        {
            SendWineSms();
            DataBind();
            ShowMessage("Ett sms med vinernas artikelnummer har skickats till dig.<br/> Vill du göra flera val?", false, false);
        }

        protected void SendWineSms()
        {
            ApsisWsHandler awh = new ApsisWsHandler();
            String msg = "Kom ihåg dessa viner:\n";

            String selectedWines = Request.Params["wine-notify"];
            if (!String.IsNullOrEmpty(selectedWines))
            {
                string[] phone = MiscFunctions.GetSeparatedCountryCodePhoneNumber(SubscriberPhoneNumber);
                string[] selectedVarunummer = selectedWines.Split(new char[]{','});
                foreach (string varunummer in selectedVarunummer)
                {
                    msg += "- " + varunummer + "\n";
                }
                String s = awh.ApsisSendSms(msg, phone[0], phone[1]);
            }
        }

        private bool TryPopulateSubUserByUrlCode()
        {
            if (string.IsNullOrEmpty(UrlCode))
                return false;

            try
            {
                Guid g = new Guid(UrlCode);
                DataSet ds = MsSqlHandler.GetPersonalUrlCusno(g,CurrentPage.PageLink.ID);
                if (DbHelpMethods.DataSetHasRows(ds))
                {
                    DataRow dr = ds.Tables[0].Rows[0];
                    long cusno = long.Parse(dr["cusno"].ToString());

                    //sync cust to mssql login tables: 1=ok, -1=no active subs, -2=no cust info in cirix
                    if (cusno > 0 && SyncSubsHandler.SyncCustToMssqlLoginTables((int)cusno) == 1)
                    {
                        //return LoginUtil.TryLoginUserToDagensIndstri(cusno);
                        //string userId = MembershipDbHandler.GetUserid(cusno);
                        SubUser = new SubscriptionUser2(cusno);
                        if (DIClassLib.Misc.LoginUtil.ReLoginUserRefreshCookie(SubUser.UserName, SubUser.Password))
                            Response.Redirect(EPiFunctions.GetFriendlyUrl(CurrentPage));
                        
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("TryPopSubUserFromUrlCode() failed for urlCode: " + UrlCode, ex.ToString());
            }

            return false;
        }

        protected void btnNoAccessSubscribe_Click(object sender, EventArgs e) 
        {
            SendToStartSubscriptionPage();
            /*
            if (CurrentPage.Property["StartSubscriptionPage"] != null)
            {
                String url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage["StartSubscriptionPage"] as PageReference);
                Response.Redirect(url + "?return=" + Server.UrlEncode(this.Request.Url.PathAndQuery));
            }
             */
        }

        protected void btnNoAccessLogin_Click(object sender, EventArgs e)
        {
            if (!User.Identity.IsAuthenticated)
            {
                String url = "/sso-login/?ReturnUrl=" + Server.UrlEncode("http://" + Request.Url.Host + Request.Url.PathAndQuery);
                Response.Redirect(url);
                return;
            }
        }

        //public void ShowError(string errorMsg)
        //{
        //    LblError.Text = errorMsg;
        //    ShowModalWindow("error", "350", "auto");
        //}

        //public void ShowMessage(string msg)
        //{
        //    LblMessage.Text = msg;
        //    ShowModalWindow("message", "350", "auto");
        //}

        //public void ShowModalWindow(string id, string width, string height)
        //{
        //    Page.ClientScript.RegisterClientScriptBlock(
        //        GetType(),
        //        id,
        //        "<script type='text/javascript'>$(function() {showmodal('#" + id + "','" + width + "px', '" + height + "', true);});</script>");
        //}

    }
}