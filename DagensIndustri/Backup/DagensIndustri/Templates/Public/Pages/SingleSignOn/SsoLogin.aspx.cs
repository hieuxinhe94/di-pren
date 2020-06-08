using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Misc;
using DIClassLib.SingleSignOn;
using System.Data;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using System.Configuration;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using System.Text;
using DIClassLib.BonnierDigital;
using DIClassLib.EPiJobs.SyncSubs;
using EPiServer.UI;
using DIClassLib.GoldMember;

namespace DagensIndustri.Templates.Public.Pages.SingleSignOn
{
    public partial class SsoLogin : DiTemplatePage
    {

        public string ReturnUrl
        {
            get
            {
                if (Request.QueryString["ReturnUrl"] != null)
                {
                    return MiscFunctions.RemoveWwwFromUrl(Request.QueryString["ReturnUrl"].ToString());
                    //return Request.QueryString["ReturnUrl"].ToString();
                }
                else
                {
                    return MiscFunctions.RemoveWwwFromUrl(HttpContext.Current.Request.Url.AbsoluteUri);
                }
            }
        }

        /// <summary>
        /// UrlMess values
        /// --------------
        /// '' redir to S+ (check if user is logged in),
        /// 1=not logged in to S+, 
        /// 2=no external cusno in S+,
        /// 3= >1 external cusno in S+,
        /// 4X=failed login user to dagensindustri.se,
        /// 41=sync ok,
        /// 42=sync failed (no active subs in cirix),
        /// 43=sync failed (no cust info in cirix)
        /// </summary>
        public string UrlMess
        {
            get
            {
                if (Request.QueryString["mess"] != null)
                    return Request.QueryString["mess"].ToString();

                return null;
            }
        }

        /// <summary>
        /// -1=no active subs, 
        /// -2=no cust info in cirix
        /// </summary>
        public string UrlSync
        {
            get
            {
                if (Request.QueryString["sync"] != null)
                    return Request.QueryString["sync"].ToString();

                return null;
            }
        }

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

                return null;
            }
        }

        public bool IsInEpiEditMode
        {
            get
            {
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    PageReference pageVersionReference = PageReference.Parse(Request.QueryString["id"]); 
                    if (pageVersionReference.WorkID > 0)
                        return true;
                }

                return false;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            base.UserMessageControl = UserMessageControl1;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            //Response.Write(EPiFunctions.GetFriendlyUrl(CurrentPage) + "<hr/>");
            //Response.Write(EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) + "<hr/>");

            UserMessageControl1.Visible = false;
            LabelMess.Visible = false;

            if (!IsPostBack)
            {
                Mainintro1.Visible = true;
                HandlePlaceHolders(false, false);

                //no UrlMess - find out if user is logged in
                if (string.IsNullOrEmpty(UrlMess) && !IsInEpiEditMode)
                {
                    Response.Redirect(BonDigMisc.GetCheckLoggedInUrl(BonDigMisc.BonDigLoginRetHandlerPath, ReturnUrl));
                }

                #region show info on page (value in UrlMess)
                if (UrlMess == "1")
                {
                    ShowMessLabel(CurrentPage["NotLoggedInToBonDig"].ToString());
                    HandlePlaceHolders(false, true);
                    return;
                }

                if (UrlMess == "2")
                {
                    ShowMessLabel(CurrentPage["BonDigNotConnectedToCusno"].ToString());
                    HandlePlaceHolders(true, false);
                    Mainintro1.Visible = false;
                    return;
                }

                if (UrlMess == "3")
                {
                    ShowMess(CurrentPage["BonDigConnectedToMultipleCusnos"].ToString(), true);
                    HandlePlaceHolders(false, false);
                    return;
                }

                if (!string.IsNullOrEmpty(UrlMess) && UrlMess.StartsWith("4"))
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append(CurrentPage["LoginToDagIndFailed"].ToString());

                    string intro = "<br><i>Teknisk information: kundinloggning på denna webbplats misslyckades. ";

                    if (UrlMess == "41")
                        sb.Append(intro + "Kundinloggningsuppgifter synkades dock korrekt.</i>");

                    if (UrlMess == "42")
                        sb.Append(intro + "Synkning av kundinloggningsuppgifter misslyckades. Aktiv prenumeration saknas i prenumerationssystemet.</i>");

                    if (UrlMess == "43")
                        sb.Append(intro + "Synkning av kundinloggningsuppgifter misslyckades. Kundinformation kunde inte hittas i prenumerationssystemet.</i>");

                    ShowMess(sb.ToString(), true);
                    HandlePlaceHolders(false, false);
                    return;
                }
                #endregion
            }
        }


        protected void ButtonCode_Click(object sender, EventArgs e)
        {
            SsoConnectRow ssoRow = new SsoConnectRow(TextBoxCode.Text);
            if (!string.IsNullOrEmpty(ssoRow.TryPopulareErr))
            {
                ShowMess(ssoRow.TryPopulareErr, true);
                HandlePlaceHolders(true, false);
                return;
            }

            UserOutput user = RequestHandler.GetUserByToken(UrlToken);
            if (BonDigHandler.TryAddSubsAsBonDigImports(UrlToken, ssoRow.CirixCusno, user))
            {
                StringBuilder sb = new StringBuilder();

                int ret = SyncSubsHandler.SyncCustToMssqlLoginTables((int)ssoRow.CirixCusno);
                if (ret == -1)
                    sb.Append("Du tycks sakna en aktiv prenumeration och kan därför inte loggas in som kund på dagensindustri.se.<br>");

                if (ret == -2)
                    sb.Append("Kunddata kunde inte hämtas för kundnummer: " + ssoRow.CirixCusno.ToString() + "<br>");

                if (!LoginUtil.TryLoginUserToDagensIndstri(ssoRow.CirixCusno))
                    sb.Append("Du kunde tyvärr inte loggas in som kund på dagensindustri.se. Din behörighet på sidan kommer att vara begränsad.");

                UpdateSsoDbRow(ssoRow.CirixCusno, UrlToken, user.user.id);

                if (sb.ToString().Length > 0)
                {
                    ShowMess(sb.ToString(), true);
                }
                else
                {
                    ShowMess(ImportsAddedSuccessMess(), false);
                }
            }
            else
            {
                ShowMess(ImportsAddedFailMess(ssoRow.CirixCusno), true);
            }

            HandlePlaceHolders(false, false);
        }

        protected void ButtonToLogin_Click(object sender, EventArgs e)
        {
            Response.Redirect(BonDigMisc.GetLoginUrl(BonDigMisc.BonDigLoginRetHandlerPath, Server.HtmlDecode(TryGetUrlParam("ReturnUrl"))), true);
        }

        protected void ButtonToNewAccount_Click(object sender, EventArgs e)
        {
            //HttpContext.Current.Request.Url.AbsoluteUri
            Response.Redirect(BonDigMisc.GetCreateAccountUrl(BonDigMisc.BonDigLoginRetHandlerPath, Server.HtmlDecode(TryGetUrlParam("ReturnUrl"))), true);
        }

        private void UpdateSsoDbRow(long cirixCusno, string token, string bonDigUserId)
        {
            string remembered = TryGetUrlParam("remembered");
            MsSqlHandler.SsoUpdateCustRow(cirixCusno, token, bonDigUserId, remembered);
        }

        private string ImportsAddedSuccessMess()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Ditt Di-konto har kopplats till din tidningsprenumeration.<br>");
            sb.Append("Du har nu tillgång till alla våra digitala tjänster.<br>");

            if (!string.IsNullOrEmpty(ReturnUrl) && !ReturnUrl.ToLower().Contains("ssologin.aspx"))
            {
                string path = Server.UrlDecode(ReturnUrl);
                sb.Append("<br><br><a href='" + path + "'>Gå till efterfrågad sida</a>");
            }
            
            return sb.ToString();
        }

        private string ImportsAddedFailMess(long cusno)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Di-kontot kunde tyvärr inte kopplas till någon tidningsprenumeration för kundnummer: " + cusno.ToString() + ". ");
            sb.Append("Var god kontakta kundtjänst. Ha gärna kundnumret till hands.");
            return sb.ToString();
        }

        private void ShowMess(string mess, bool isError)
        {
            ShowMessage(mess, false, isError);
            UserMessageControl1.Visible = true;
        }

        private void ShowMessLabel(string mess)
        {
            LabelMess.Text = "<p>" + mess + "</p>";
            LabelMess.Visible = true;
        }

        private void HandlePlaceHolders(bool showCodeForm, bool showSsoLinks)
        {
            PlaceHolderCode.Visible = showCodeForm;
            PlaceHolderSsoLinks.Visible = showSsoLinks;

            //if (showCodeForm)
            //    Mainbody1.Text = CurrentPage["MainBody"].ToString();

            if (showSsoLinks)
            {
                //if (string.IsNullOrEmpty(UrlCode))
                //    Mainbody1.Text = CurrentPage["MainBodyIdentified"].ToString();
                //else
                //    Mainbody1.Text = CurrentPage["MainBodyIdentifiedByUrl"].ToString();
            }
        }

        private string TryGetUrlParam(string paramName)
        {
            if (Request.QueryString[paramName] != null)
                return Request.QueryString[paramName].ToString();

            return string.Empty;
        }


        #region old code
        //if (!IsPostBack)
        //{
        //    //-http://account.qa.newsplus.se/register?appId=dagensindustri.se&lc=sv&callback=xxx
        //    string urlBonDig = ConfigurationManager.AppSettings["BonDigUrlAccount"];
        //    string pageLogin = ConfigurationManager.AppSettings["BonDigLoginPage"];
        //    string pageCreAcc = ConfigurationManager.AppSettings["BonDigCreateAccountPage"];
        //    string appId = "?appId=" + ConfigurationManager.AppSettings["BonDigAppIdDagInd"];
        //    string lc = "&lc=sv";
        //    string ret = (string.IsNullOrEmpty(ReturnUrl)) ? EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage) : ReturnUrl;
        //    string callBack = "&callback=" + Server.UrlEncode(BonDigMisc.BonDigLoginRetHandlerPath + "?ReturnUrl=" + ret);

        //    //HyperLinkToLogin.NavigateUrl = urlBonDig + pageLogin + appId + lc + callBack;
        //    //HyperLinkToNewAccount.NavigateUrl = urlBonDig + pageCreAcc + appId + lc + callBack;
        //}
        
        //protected void ButtonCheckLogin_Click(object sender, EventArgs e)
        //{
        //    //long cusno = CirixDbHandler.GetCusnoByLogin(user, pass);
        //    long cusno = 0;
        //    if (cusno < 0)
        //    {
        //        ShowMess("Vi kunde tyvärr inte verifiera din identitet.", true);
        //        return;
        //    }

        //    _ssoRow = new SsoConnectRow(cusno);
        //    if (!string.IsNullOrEmpty(_ssoRow.TryPopulareErr))
        //    {
        //        ShowMess(_ssoRow.TryPopulareErr, true);
        //        return;
        //    }

        //    HandlePlaceHolders(false, true);
        //}

        //private string TrySendPassword(string storedProc, string inputText)
        //{
        //    inputText = MiscFunctions.REC(inputText);

        //    if (string.IsNullOrEmpty(inputText))
        //        return "Var god ange mail/användarnamn/kundnummer";

        //    string email = string.Empty;
        //    string userid = string.Empty;
        //    string passwd = string.Empty;
        //    TrySetUserFromDb(storedProc, inputText, out email, out userid, out passwd);

        //    if (!MiscFunctions.IsValidEmail(email) || string.IsNullOrEmpty(userid) || string.IsNullOrEmpty(passwd))
        //        return "Ditt inogg kunde tyvärr inte skickas. Var god kontakta kundtjänst.";

        //    string mailSubject = Translate("/dilogin/forgotpassword/mail/subject");
        //    string mailBody = string.Format(Translate("/dilogin/forgotpassword/mail/body"), email, passwd).Replace("[nl]", Environment.NewLine);
        //    MiscFunctions.SendMail("no-reply@di.se", email, mailSubject, mailBody, false);

        //    return "Inlogget har skickats till " + email;
        //}

        //private void TrySetUserFromDb(string storedProc, string inputText, out string email, out string userid, out string passwd)
        //{
        //    email = string.Empty;
        //    userid = string.Empty;
        //    passwd = string.Empty;
        //    SqlDataReader DR = null;
        //    try
        //    {
        //        DR = SqlHelper.ExecuteReader("DisePren", storedProc, new SqlParameter("@strID", inputText));
        //        if (DR.Read())
        //        {
        //            if (DR["result"].ToString() == "1")
        //            {
        //                if (DR["emailaddress"] != System.DBNull.Value) email = DR["emailaddress"].ToString();
        //                if (DR["userid"] != System.DBNull.Value) userid      = DR["userid"].ToString();
        //                if (DR["passwd"] != System.DBNull.Value) passwd      = DR["passwd"].ToString();
        //            }
        //            //else if (DR["result"].ToString() == "-2")
        //            //    return err;   //multiple users with same emailaddress
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("SendPassword() - failed", ex.ToString());
        //    }
        //    finally
        //    {
        //        if (DR != null)
        //            DR.Close();
        //    }
        //}



        //protected void ButtonSaveAccount_Click(object sender, EventArgs e) {}

        //private void PopulateSsoForm()
        //{
        //    TextBoxEmail.Text = _ssoRow.CirixEmail;

        //    if (!string.IsNullOrEmpty(_ssoRow.PlusCustId))
        //    {
        //        RadioButtonListDiAccount.Items[0].Selected = true;
        //        PlaceHolderPass2.Visible = false;
        //    }
        //    else
        //        RadioButtonListDiAccount.Items[1].Selected = true;

        //}


        //protected void ButtonSendCode_Click(object sender, EventArgs e)
        //{
        //    string cusnoStr = MiscFunctions.REC(TextBoxRemind.Text);

        //    if (string.IsNullOrEmpty(cusnoStr) || !MiscFunctions.IsNumeric(cusnoStr))
        //    {
        //        ShowMess("Var god ange kundnummer", true);
        //        return;
        //    }

        //    long cusno = long.Parse(cusnoStr);
        //    SsoConnectRow ssoRow = new SsoConnectRow(cusno);
        //    if (!string.IsNullOrEmpty(ssoRow.TryPopulareErr))
        //    {
        //        ShowMess(ssoRow.TryPopulareErr, true);
        //        return;
        //    }

        //    string email = ssoRow.CirixEmail;
        //    if (!MiscFunctions.IsValidEmail(email))
        //        email = TryGetEmailFromCirix(cusno);

        //    if (!MiscFunctions.IsValidEmail(email))
        //    {
        //        ShowMess("Din kod kunde tyvärr inte skickas till dig. Vi kunde inte hitta någon giltig mailadress för kundnummer " + cusnoStr + ". Var god kontakta kundtjänst.", true);
        //        return;
        //    }

        //    SendCodeEmail(email, ssoRow.CustomerCode);            

        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("Koden har skickats till den mailadress som är kopplad till angivet kundnummer. ");
        //    sb.Append("När du fått din kod fyller du i den i rutan nedan för att identifiera dig. ");
        //    sb.Append("Var god kontakta kundtjänst om du inte får ett mail inom kort.");

        //    ShowMess(sb.ToString(), false);
        //    PlaceHolderSendCodeBox.Visible = false;
        //    HandlePlaceHolders(true, false);
        //}

        //private void SendCodeEmail(string email, string code)
        //{
        //    string mailSubject = "Kod till dagensindustri.se";
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("Du har via dagensindustri.se begärt att få din kod skickad till dig.<br>");
        //    sb.Append("Din kod är: " + code + "<br><br>");
        //    sb.Append("Med vänlig hälsning<br>");
        //    sb.Append("Dagens industri");
        //    MiscFunctions.SendMail("no-reply@di.se", email, mailSubject, sb.ToString(), true);
        //}
        #endregion

    }
}