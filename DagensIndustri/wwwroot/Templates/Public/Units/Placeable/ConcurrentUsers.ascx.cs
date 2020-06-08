using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.MobileControls;
using System.Web.UI.WebControls;

using DIClassLib.EPiJobs.SyncSubs;
using DIClassLib.Membership;

using EPiServer;
using System.Web.Script.Serialization;
using System.Configuration;
using System.Net;
using System.IO;
using DIClassLib.DbHelpers;
using DagensIndustri.Tools.Classes;
using DIClassLib.Misc;
using DIClassLib.BonnierDigital;
using System.Web.Security;
using DIClassLib.SingleSignOn;


namespace DagensIndustri.Templates.Public.Units.Placeable
{
    public partial class ConcurrentUsers : UserControlBase
    {
        public string LoginPageUrl
        {
            get
            {
                return EPiFunctions.GetFriendlyAbsoluteUrl(EPiFunctions.GetLoginPage());
            }
        }

        public string UrlToThisPage       
        { 
            get 
            {
                //_urlToThisPage = HttpUtility.UrlEncode(Request.Url.GetLeftPart(UriPartial.Path));
                Uri adr = new Uri(EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage));
                return adr.GetLeftPart(UriPartial.Path); 
            } 
        }
        
        public string LogoutLoginRedirUrl { get { return BonDigMisc.GetLogoutUrl(BonDigMisc.GetLoginUrl(UrlToThisPage, "")); } }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            string show = ConfigurationManager.AppSettings["BonDigUseVerifyEntitlement"];
            if (string.IsNullOrEmpty(show) || show.ToUpper() == "FALSE" || MembershipFunctions.UserIsLoggedInWithProvider(MembershipSettings.EpiMembershipProviderName))
            {
                this.Visible = false;
                return;
            }

            string token = TryGetUserToken();
            HyperLinkButton.NavigateUrl = !IsValue("ConCurrLoginUrl") ? BonDigMisc.GetLogoutUrl(BonDigMisc.GetLoginUrl(BonDigMisc.BonDigLoginRetHandlerPath, UrlToThisPage)) : CurrentPage["ConCurrLoginUrl"] as string;
            HyperLinkCancel.NavigateUrl = !IsValue("ConCurrCancelUrl") ? EPiFunctions.GetFriendlyUrl(EPiFunctions.StartPage()) : CurrentPage["ConCurrCancelUrl"] as string;
            
            //Sometimes this usercontrol is used on a page that is placed in an iframe on e.g. di.se, so need to braek out of that iframe:
            HyperLinkButton.Target = "_top";
            HyperLinkCancel.Target = "_top";

            //not logged in to S+
            if (token == "")
            {
                LiteralMessage.Text = "Du tycks inte vara korrekt inloggad med ditt Di-konto. Var god logga in.";
                LoginUtil.LogoutUser();
            }

            //logged in to S+
            if (!string.IsNullOrEmpty(token))
            {
                #region handle login with DiMembershipProvider
                if (!MembershipFunctions.UserIsLoggedInWithProvider(MembershipSettings.DiMembershipProviderName))
                {
                    string qstMess = "?mess=";
                    string qstToken = "&token=" + token;
                    string qstReturnUrl = "&ReturnUrl=" + Server.UrlEncode(UrlToThisPage);

                    //try get external cusnos from bon dig
                    long cusno = 0;
                    List<long> extCusnos = RequestHandler.TryGetCirixCusnosFromBonDig(token);

                    //ERR: no external cusno in S+
                    if (extCusnos.Count == 0)
                        Response.Redirect(LoginPageUrl + qstMess + "2" + qstToken + qstReturnUrl);

                    //ERR: >1 external cusno in S+
                    if (extCusnos.Count > 1)
                        Response.Redirect(LoginPageUrl + qstMess + "3" + qstToken + qstReturnUrl);

                    //SUCCESS: 1 external cusno in S+
                    if (extCusnos.Count == 1)
                        cusno = extCusnos[0];

                    //try sync cust to dagensindustri.se login tables: 1=ok, -1=no active subs, -2=no cust info in cirix
                    int sync = SyncSubsHandler.SyncCustToMssqlLoginTables((int)cusno);
                    if (sync == -1) sync = 2;
                    if (sync == -2) sync = 3;

                    //ERR: failed login user to dagensindustri.se. Ret values: 41=syncOk, 42=noActiveSubs, 43=noCustInfoInCirix
                    if (!LoginUtil.TryLoginUserToDagensIndstri(cusno))
                        Response.Redirect(LoginPageUrl + qstMess + "4" + sync.ToString() + qstToken + qstReturnUrl);
                    else
                        Response.Redirect(UrlToThisPage + "?token=" + token);
                }
                #endregion



                Dictionary<String, Object> sPlusDic = RequestHandler.GetVerifyEntitlement(Settings.BonDigExtRescIdPdfPaper, token);

                //no valid return from S+
                if (sPlusDic == null)
                {
                    //LiteralMessage.Text = "Ett tekniskt fel uppstod. Vi kunde tyvärr inte verifiera att du har rätt att läsa PDF-tidningen.";
                    this.Visible = false;
                    return;
                }

                string respCode = RequestHandler.TryGetDicValByKey(sPlusDic, "@httpResponseCode");
                    
                //entitled
                if (respCode == "200")
                {
                    this.Visible = false;
                    return;
                }
                    
                //not entitled
                if (respCode == "401")
                {
                    string error = RequestHandler.TryGetDicValByKey(sPlusDic, "error");
                    //string errorMsg = RequestHandler.TryGetDicValByKey(sPlusDic, "errorMsg");

                    //no valid entitlement - let old logic on page handle this
                    if (error == "")
                    {
                        this.Visible = false;
                        return;
                    }

                    //too many concurrent users
                    if (!string.IsNullOrEmpty(error))
                    {
                        LiteralMessage.Text = "För många samtidiga användare på kontot. " +
                                              "Återta kontrollen genom att logga in igen.";
                    }
                }
            }
        }

        private string TryGetUserToken()
        {
            //get from session
            SsoLoginHandler lgh = new SsoLoginHandler();
            if (lgh.IsLoggedInToBonDig)
                return lgh.Token;

            //try get token from url
            string token = Request["token"];

            //no token in url - get it from checkLoginPage
            if (token == null)
            {
                Response.Redirect(ConfigurationManager.AppSettings["BonDigUrlAccount"] +
                                    ConfigurationManager.AppSettings["BonDigCheckLoginPage"] +
                                    "?appId=dagensindustri.se&lc=sv&" + 
                                    "callback=" + HttpUtility.UrlEncode(UrlToThisPage), true);
            }

            return token;
        }



        #region old code

        //LoginUtil.LogoutUser();
        //Response.Redirect(BonDigMisc.GetLogoutUrl(EPiFunctions.GetFriendlyAbsoluteUrl(EPiFunctions.StartPage())));
        //string s = BonDigMisc.GetLogoutUrl(BonDigMisc.GetLoginUrl(EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage), ""));   //EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage)
        //http://localhost/sso-login/?ReturnUrl=http%3a%2f%2flocalhost%2fvara-tidningar-2%2flas-dagens-industri-online%2f

        //string _urlToThisPage = Request.Url.GetLeftPart(UriPartial.Path);

        //                    string script = string.Format(@"$(document).ready(function() {{
        //                                                    alert('{0}');
        //                                                    window.location = '{1}';
        //                                                }});",
        //                                            UserMessage,
        //                                            LogoutLoginRedirUrl
        //                                        );
        //StringBuilder sb = new StringBuilder();
        //sb.Append("<script type=\"text/javascript\">");
        //sb.Append("$(document).ready(function () {");
        //sb.Append("$(\"#dialog-message\").dialog({");
        //sb.Append("modal: true, ");
        //sb.Append("dialogClass: \"no-close\");");
        //sb.Append("});");
        ////sb.Append("alert('test');");
        //sb.Append("});");
        //sb.Append("</script>");

        //RegisterClientScriptFile("http://code.jquery.com/jquery-1.9.1.js");
        //RegisterClientScriptFile("http://code.jquery.com/ui/1.10.3/jquery-ui.js");
        //LiteralJs.Text = sb.ToString();

        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowUserMessage", sb.ToString(), true);
        //Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "ShowUserMessage", "$(document).ready(function () { alert('apa'); });", true);

        #endregion

    }
}