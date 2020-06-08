using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using DIClassLib.Misc;
using DIClassLib.EPiJobs.SyncSubs;
using DIClassLib.DbHelpers;
using DIClassLib.BonnierDigital;
using DagensIndustri.Tools.Classes;
using DIClassLib.SingleSignOn;
using System.Web.SessionState;


namespace DagensIndustri.Handlers.SingleSignOn
{

    /// <summary>
    /// User will load to this handler after S+ page for login/create new account. 
    /// Will set a SsoLoginHandler object, wich stores session variables for logged in S+ user.
    /// </summary>
    public class BonDigReturnHandler : IHttpHandler, IRequiresSessionState 
    {
        //return from S+:
        //-http://localhost/Handlers/SingleSignOn/BonDigReturnHandler.ashx
        //?ReturnUrl=http%3a%2f%2flocalhost%2ftemplates%2fpublic%2fpages%2fMySettings.aspx%3ftest%3d111%26id%3d95%26epslanguage%3dsv
        //&token=
        //&remembered=false
        //&firstName=           //131010 removed
        //&lastName=            //131010 removed

        string _token;
        string _remembered;
        string _returnUrl;

        //todo: improve check?
        public bool LoggedInToBonDig 
        {
            get
            {
                if (!string.IsNullOrEmpty(_token))
                    return true;

                return false;
            }
        }

        public string LoginPageUrl 
        { 
            get 
            {
                return EPiFunctions.GetFriendlyAbsoluteUrl(EPiFunctions.GetLoginPage());
            } 
        }


        public void ProcessRequest(HttpContext ctx)
        {
            ctx.Response.ContentType = "text/plain";
            SetMembersFromUrl(ctx);
            SetSsoLoginHandler();

            string qstMess = "?mess=";
            
            string qstToken = "";
            if (!string.IsNullOrEmpty(_token)) 
                qstToken = "&token=" + _token;
            
            string qstReturnUrl = "&ReturnUrl=" + ctx.Server.UrlEncode(_returnUrl);

            #region redirs to loginPage if not logged in / some error
            //not logged in to S+
            if (!LoggedInToBonDig)
                ctx.Response.Redirect(LoginPageUrl + qstMess + "1" + qstReturnUrl);
            
            //try get external cusnos from bon dig
            long cusno = 0;
            List<long> extCusnos = RequestHandler.TryGetCirixCusnosFromBonDig(_token);

            //ERR: no external cusno in S+
            if (extCusnos.Count == 0)
                ctx.Response.Redirect(LoginPageUrl + qstMess + "2" + qstToken + qstReturnUrl);
                
            //ERR: >1 external cusno in S+
            if (extCusnos.Count > 1)
                ctx.Response.Redirect(LoginPageUrl + qstMess + "3" + qstToken + qstReturnUrl);

            //SUCCESS: 1 external cusno in S+
            if (extCusnos.Count == 1)
                cusno = extCusnos[0];

            //try sync cust to dagensindustri.se login tables: 1=ok, -1=no active subs, -2=no cust info in cirix
            int sync = SyncSubsHandler.SyncCustToMssqlLoginTables((int)cusno);
            if (sync == -1) sync = 2;
            if (sync == -2) sync = 3;

            //ERR: failed login user to dagensindustri.se. Ret values: 41=syncOk, 42=noActiveSubs, 43=noCustInfoInCirix
            var bWasBonDigLoggedIn = LoggedInToBonDig; //Keep state from BonDig as TryLoginUserToDagensIndstri() will call LogOutUser() and clear session, otherwise result in infinite loop
            if (!LoginUtil.TryLoginUserToDagensIndstri(cusno))
                ctx.Response.Redirect(LoginPageUrl + qstMess + "4" + sync.ToString() + qstToken + qstReturnUrl);

            if (bWasBonDigLoggedIn) //Reset state from BonDig as TryLoginUserToDagensIndstri() will call LogOutUser() and clear session
            {
                var loginHandler = new SsoLoginHandler();
                loginHandler.IsLoggedInToBonDig = true;
                loginHandler.Token = _token;
            }
            #endregion

            //go to returnUrl (if not login page)
            if (!string.IsNullOrEmpty(_returnUrl) && !_returnUrl.ToLower().Contains("ssologin.aspx"))
                ctx.Response.Redirect(_returnUrl);
            else  //no relevant returnUrl - go to start page
                ctx.Response.Redirect(EPiFunctions.StartPage().LinkURL);
        }


        private void SetMembersFromUrl(HttpContext ctx)
        {
            _token = ctx.Request.QueryString["token"];
            if (!string.IsNullOrEmpty(_token))
            {
                string[] arr = _token.Split(',');
                _token = arr[0];
            }

            _remembered = ctx.Request.QueryString["remembered"];
            _returnUrl  = ctx.Request.QueryString["ReturnUrl"];
            _returnUrl = MiscFunctions.RemoveWwwFromUrl(_returnUrl);
        }

        
        private void SetSsoLoginHandler()
        {
            SsoLoginHandler lgh = new SsoLoginHandler();
            lgh.IsLoggedInToBonDig = LoggedInToBonDig;
            lgh.Token = _token;
        }

        public bool IsReusable
        {
            get { return false;}
        }

    }
}