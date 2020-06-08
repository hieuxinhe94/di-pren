using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Misc;
using DIClassLib.SingleSignOn;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using System.Text;
using DIClassLib.BonnierDigital;
using DIClassLib.EPiJobs.SyncSubs;


namespace DagensIndustri.Templates.Public.Pages.SingleSignOn
{
    public partial class SsoConnect : DiTemplatePage
    {

        //cust mail with code in link
        public string UrlCode 
        {
            get
            {
                if (Request.QueryString["scode"] != null)
                    return Request.QueryString["scode"].ToString();

                return string.Empty;
            }
        }

        //if user returned from BonDig page token will exist in url
        public string UrlToken
        {
            get
            {
                if (Request.QueryString["token"] != null)
                {
                    string token = Request.QueryString["token"].ToString();
                    string[] arr = token.Split(',');
                    return arr[0];
                }

                return string.Empty;
            }
        }
        
        //'1' returned from bonDig after checkLoggedIn
        public string UrlLoggedInChecked 
        {
            get
            {
                if (Request.QueryString["loggedInCheck"] != null)
                    return Request.QueryString["loggedInCheck"].ToString();

                return string.Empty;
            }
        }

        //pop by code
        public SsoConnectRow SsoRow 
        {
            get
            {
                if (ViewState["sso"] != null)
                    return (SsoConnectRow)ViewState["sso"];

                return null;
            }
            set
            { 
                ViewState["sso"] = value;
            }
        }

        UserOutput _user = null;
        public UserOutput BonDigUser
        {
            get
            {
                if (_user != null && _user.user != null)
                    return _user;
                
                if (!string.IsNullOrEmpty(UrlToken))
                    _user = RequestHandler.GetUserByToken(UrlToken);
                
                return _user;
            }
            set 
            {
                _user = value;
            }
        }


        private string GetCirixCustRows(SsoConnectRow ssoRow)
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("Kundnummer: " + SsoRow.CirixCusno.ToString() + "<br>");

            if (string.IsNullOrEmpty(ssoRow.Cirix_RowText1_RowText2[1]))
                sb.Append("Namn: " + ssoRow.Cirix_RowText1_RowText2[0] + "<br>");
            else
            {
                sb.Append("Företag: " + ssoRow.Cirix_RowText1_RowText2[0] + "<br>");
                sb.Append("Namn: " + ssoRow.Cirix_RowText1_RowText2[1] + "<br>");
            }
            return sb.ToString();
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            HandleMainIntoAndMainBody();
            HandleAccountBoxText();

            #region 'done' in url
            string doneVal = TryGetUrlParam("done");
            if (!string.IsNullOrEmpty(doneVal))
            {
                if (MiscFunctions.IsNumeric(doneVal)) //1 or 2  (doneVal == "1")
                    ShowMess(ImportsAddedSuccessMess(doneVal), false);
                else
                    ShowMess(ImportsAddedFailMess(doneVal), true);

                HandlePlaceHolders(false, false, false);
                return;
            }
            #endregion

            if (!IsPostBack)
            {
                UserMessageControl1.Visible = false;

                //no code in url
                if (string.IsNullOrEmpty(UrlCode))
                {
                    HandlePlaceHolders(true, false, true);  //show 'identify you' form
                    return;
                }
                
                //code in url (customer mail link or is return from BonDig)
                SsoRow = new SsoConnectRow(UrlCode);
                if (!string.IsNullOrEmpty(SsoRow.TryPopulareErr))
                {
                    ShowMess(SsoRow.TryPopulareErr, true);
                    HandlePlaceHolders(true, false, true);        //show 'identify you' form
                    return;
                }
                else //populare ssoRow OK
                {
                    //not return from BonDig - go there
                    if (string.IsNullOrEmpty(UrlLoggedInChecked))
                        Response.Redirect(GetCheckedLoggedInUrl(SsoRow.CustomerCode), true);

                    HandleAccountBoxText();

                    //show BonDig login links
                    HandlePlaceHolders(false, true, true);    
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            HandleSsoPlaceHolderContent();
        }


        protected void ButtonCode_Click(object sender, EventArgs e)
        {
            HandlePlaceHolders(true, false, true);

            SsoRow = new SsoConnectRow(TextBoxCode.Text);
            if (!string.IsNullOrEmpty(SsoRow.TryPopulareErr))
            {
                ShowMess(SsoRow.TryPopulareErr, true);
                return;
            }

            Response.Redirect(GetCheckedLoggedInUrl(SsoRow.CustomerCode), true);
        }

        protected void ButtonSsoLinkToNewAccount_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            Response.Redirect(b.CommandArgument, true);
        }

        protected void ButtonSsoFirst_Click(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            if (b.CommandArgument == "connect")
                TryConnectAccounts(SsoRow);
            else
                Response.Redirect(b.CommandArgument);
        }
        
        private void TryConnectAccounts(SsoConnectRow ssoRow)
        {
            long cusno = ssoRow.CirixCusno;
            
            string diffCusnoInBonDig = BonDigHandler.TryGetCustHasDiffCusnosInBonDigMess(UrlToken, cusno);
            if (diffCusnoInBonDig.Length > 0)
            {
                ShowMess(diffCusnoInBonDig, true);
                return;
            }

            if (BonDigHandler.TryAddSubsAsBonDigImports(UrlToken, cusno, BonDigUser))
            {
                int ret = SyncSubsHandler.SyncCustToMssqlLoginTables((int)cusno);
                if (ret != 1)
                    new Logger("DoSyncToMssqlLoginTables() failed for cusno:" + cusno + ", ret:" + ret, "not an exception: -1 does not have active subs, -2 could not find customer facts in cirix");

                if (!LoginUtil.TryLoginUserToDagensIndstri(cusno))
                    new Logger("TryLoginUserToDagensIndstri() failed for cusno:" + cusno, "not an exception");

                UpdateSsoDbRow(cusno, UrlToken, BonDigUser.user.id);

                bool hasAgendaSub = CustHasAgendaSub(cusno);
                int retId = hasAgendaSub ? 2 : 1;

                Response.Redirect(EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) + "?done=" + retId);
            }
            else
            {
                Response.Redirect(EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) + "?done=" + ssoRow.CustomerCode);
            }
        }

        private bool CustHasAgendaSub(long cusno)
        {
            var subs = SubscriptionController.GetSubscriptions2(cusno);
            foreach (var sub in subs)
            {
                if (sub.PaperCode == Settings.PaperCode_AGENDA)
                    return true;
            }

            return false;
        }

        private string GetCheckedLoggedInUrl(string code)
        {
            string url = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) + "?loggedInCheck=1&scode=" + code;
            return BonDigMisc.GetCheckLoggedInUrl(url, "");
        }
        
        //private string GetCustHasDiffCusnosInBonDigMess(string token, long cirixCusno)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    foreach (long cn in RequestHandler.TryGetCirixCusnosFromBonDig(token))
        //    {
        //        if (cn != cirixCusno)
        //            sb.Append(cn.ToString() + " ");
        //    }

        //    if (sb.ToString().Length > 0)
        //    {
        //        StringBuilder sb2 = new StringBuilder();
        //        sb2.Append("Di-kontot kunde tyvärr inte kopplas till ditt kundnummer " + cirixCusno.ToString() + " ");
        //        sb2.Append("eftersom det redan är kopplat till kundnummer ");
        //        sb2.Append(sb.ToString());
        //        sb2.Append("<br>Var god kontakta kundtjänst. Ha gärna dessa uppgifter till hands.");

        //        new Logger("GetCustHasDiffCusnosInBonDigMess() - found other external cusnos in S+ for cusno: " + cirixCusno, "Other external cusnos in S+ " + sb.ToString());

        //        return sb2.ToString();
        //    }

        //    return string.Empty;
        //}

        private void UpdateSsoDbRow(long cirixCusno, string token, string bonDigUserId)
        {
            string remembered = TryGetUrlParam("remembered");
            MsSqlHandler.SsoUpdateCustRow(cirixCusno, token, bonDigUserId, remembered);
        }

        private string ImportsAddedSuccessMess(string doneVal)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>Tack!</b><br>");
            sb.Append("Ditt Di-konto har kopplats till din prenumeration.<br>");
            sb.Append("Du har nu tillgång till dina digitala tjänster.<br>");

            if (doneVal == "2")
                sb.Append("<a href='http://www.di.se/agenda/'>Klicka här</a> för att komma till Agenda.<br>");
            
            return sb.ToString();
        }

        private string ImportsAddedFailMess(string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Prenumerationen kunde tyvärr inte kopplas till ditt Di-kontot. ");
            sb.Append("Var god kontakta kundtjänst. Ha gärna ditt kundnummer till hands.");
            
            SsoConnectRow row = new SsoConnectRow(code);
            if (string.IsNullOrEmpty(row.TryPopulareErr))
            {
                sb.Append("<br>Kundnummer: " + row.CirixCusno.ToString());
            }
            return sb.ToString();
        }

        private void HandleSsoPlaceHolderContent()
        {
            if (SsoRow != null && PlaceHolderSso.Visible == true)
            {
                string callBack = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) + "?scode=" + SsoRow.CustomerCode;

                if (UrlLoggedInChecked == "1" && BonDigUser != null && BonDigUser.user != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(CurrentPage["MainBodyConf"].ToString());
                    sb.Append("<p>");

                    //sb.Append("<b>Dagens industri prenumeration</b><br>");
                    sb.Append(GetCirixCustRows(SsoRow));
                    sb.Append("<br>");

                    sb.Append("Kopplas till Di-konto");
                    sb.Append("<br><br>");

                    //sb.Append("<b>Di-konto</b><br>");
                    sb.Append("Användarnamn: " + BonDigUser.user.email + "<br>");
                    sb.Append("Namn: " + BonDigUser.user.firstName + " " + BonDigUser.user.lastName);
                    
                    //sb.Append("<br><br>");
                    //sb.Append("<a href='" + BonDigMisc.GetLogoutUrl(callBack) + "'>Klicka här</a> ");
                    //sb.Append("om du vill koppla ihop prenumerationen med ett annat Di-konto.<br><br>");
                    sb.Append("<br><br></p>");
                    LiteralSsoFirst.Text = sb.ToString();

                    ButtonSsoFirst.CommandArgument = "connect";
                    ButtonSsoFirst.Text = "Slutför";
                    PlaceHolderSsoNew.Visible = false;
                }
                else
                {
                    ButtonSsoFirst.CommandArgument = BonDigMisc.GetLoginUrl(callBack, "");
                    ButtonSsoFirst.Text = "Logga in med nuvarande Di-konto";
                    PlaceHolderSsoNew.Visible = true;
                }

                ButtonSsoLinkToNewAccount.CommandArgument = BonDigMisc.GetCreateAccountUrl(callBack, "");
            }
        }

        private void HandleMainIntoAndMainBody()
        {
            Mainintro1.Visible = true;
            Mainbody1.Visible = false;

            if (string.IsNullOrEmpty(UrlCode) && string.IsNullOrEmpty(UrlToken))
                Mainbody1.Visible = true;

            if (UrlLoggedInChecked == "1" && string.IsNullOrEmpty(UrlToken))
            {
                Mainbody1.Text = CurrentPage["MainBodyIdentified"].ToString();
                Mainbody1.Visible = true;
            }

            if (UrlLoggedInChecked == "1" && !string.IsNullOrEmpty(UrlCode) && !string.IsNullOrEmpty(UrlToken))
            {
                Mainintro1.Visible = false;
            }

            if (!string.IsNullOrEmpty(TryGetUrlParam("done")))
            {
                Mainintro1.Visible = false;
                Mainbody1.Visible = false;
            }
        }

        private void HandleAccountBoxText()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>Dagens industri prenumeration</b><br>");
            if (SsoRow != null)
                sb.Append(GetCirixCustRows(SsoRow));
            else
                sb.Append("<i>Ej identifierad</i><br>");

            sb.Append("<br><b>Di-konto</b><br>");
            if (BonDigUser != null && BonDigUser.user != null)
            {
                sb.Append("Användarnamn: " + BonDigUser.user.email + "<br>");
                sb.Append("Namn: " + BonDigUser.user.firstName + " " + BonDigUser.user.lastName);
            }
            else
                sb.Append("<i>Ej inloggad</i>");

            LiteralAccountInfoBox.Text = sb.ToString();
        }

        private void ShowMess(string mess, bool isError)
        {
            ShowMessage(mess, false, isError);
            UserMessageControl1.Visible = true;
        }

        private void HandlePlaceHolders(bool showCodeForm, bool showSsoForm, bool showAccBox)
        {
            PlaceHolderCode.Visible = showCodeForm;
            PlaceHolderSso.Visible = showSsoForm;
            PlaceHolderAccountBox.Visible = showAccBox;
        }

        private string TryGetUrlParam(string paramName)
        {
            if (Request.QueryString[paramName] != null)
                return Request.QueryString[paramName].ToString();

            return string.Empty;
        }
        
    }
}