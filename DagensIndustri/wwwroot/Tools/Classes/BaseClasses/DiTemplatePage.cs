using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using EPiServer;
using EPiServer.Core;
using EPiServer.Security;
using DagensIndustri.Templates.Public.Units.Placeable;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.SingleSignOn;
using DIClassLib.BonnierDigital;
using System.Collections.Generic;
//using System.Web.Security;


namespace DagensIndustri.Tools.Classes.BaseClasses
{
    public class DiTemplatePage : EPiServer.TemplatePage
    {

        public UserMessage UserMessageControl { get; set; }

        private bool? _isGoldPage;
        public bool IsGoldPage 
        {
            get
            {
                if (_isGoldPage != null)
                {
                    return (bool)_isGoldPage;
                }
                _isGoldPage = false;
                var pd = DataFactory.Instance.GetPage(this.CurrentPageLink);
                foreach (RawACE ace in pd.ACL.ToRawACEArray())
                {
                    if (ace.Name == DiRoleHandler.RoleDiGold)
                    {
                        _isGoldPage = true;
                        break;
                    }
                }

                return (bool)_isGoldPage;
            }
        }


        protected override void OnPreInit(System.EventArgs e)
        {
            base.OnPreInit(e);

            PageData thisPd = DataFactory.Instance.GetPage(this.CurrentPageLink);

            
            SsoLoginHandler lgh = new SsoLoginHandler();
            if (!IsPublicPage(thisPd) && !lgh.IsLoggedInToBonDig)
            {
                //check if user is logged in to S+
                //1. redir to S+ check-logged-in
                //2. return to BonDigReturnHandler.ashx
                //3. redir to HttpContext.Current.Request.Url.AbsoluteUri &
                Response.Redirect(BonDigMisc.GetCheckLoggedInUrl(BonDigMisc.BonDigLoginRetHandlerPath, HttpContext.Current.Request.Url.AbsoluteUri));
            }

            
            if (!EPiFunctions.UserHasPageAccess(thisPd, AccessLevel.Read))
            {
                #region old bonDig logged in check
                //check if user is logged in to S+
                //1. redir to S+ check-logged-in
                //2. return to BonDigReturnHandler.ashx
                //3. redir to HttpContext.Current.Request.Url.AbsoluteUri &
                //Response.Redirect(BonDigMisc.GetCheckLoggedInUrl(BonDigMisc.BonDigLoginRetHandlerPath, HttpContext.Current.Request.Url.AbsoluteUri), true);
                #endregion

                //!hasPageAccess && isGoldPage
                //- isAuthenticated: redir to di gold flow page 
                //- !isAuthenticated: redir to login page
                PageReference pr = (IsGoldPage && HttpContext.Current.User.Identity.IsAuthenticated) ? 
                                        EPiFunctions.GetDiGoldFlowPage(thisPd).PageLink :
                                        EPiFunctions.GetLoginPage(thisPd).PageLink;

                EPiFunctions.RedirectToPage(Page, pr, EPiFunctions.GetFriendlyAbsoluteUrl(thisPd));

                #region old code
                //bool isDiGoldPage = false;
                //foreach (RawACE ace in thisPd.ACL.ToRawACEArray())
                //{
                //    if (ace.Name == "DiGold")
                //    {
                //        isDiGoldPage = true;
                //        break;
                //    }
                //}
                #endregion
            }


            #region IP-login old code
            //todo - ip-logged in users does not get a 'normal' role in the system
            //if (!User.Identity.IsAuthenticated)
            //{
            //    AutoLoginByIPHTTPModule al = new AutoLoginByIPHTTPModule();
            //    System.Security.Principal.GenericPrincipal principal = al.GetAutoLoginPrincipal(HttpContext.Current);

            //    if (principal != null)
            //    {
            //        HttpContext.Current.User = principal;
            //        //CreateIpAutoLoginCookie();
            //    }
            //}
            #endregion
        }

        private bool IsPublicPage(PageData pd)
        {
            foreach (string key in pd.ACL.Keys)
            {
                if (key.ToLower() == "everyone")
                    return true;
            }

            return false;
        }


        /// <summary>
        /// Show message on page
        /// </summary>
        /// <param name="translateKey"></param>
        /// <param name="isKey"></param>
        /// <param name="isErrorMessage"></param>
        public void ShowMessage(string translateKey, bool isKey, bool isErrorMessage)
        {
            if (UserMessageControl != null)
                UserMessageControl.ShowMessage(translateKey, isKey, isErrorMessage);
        }


        //public void CreateIpAutoLoginCookie()
        //{
        //    try
        //    {
        //        HttpCookie authCookie = null;
        //        string ticks = DateTime.Now.Ticks.ToString();

        //        //Create user
        //        string username = DiRoleHandler.RoleDiIp + ticks;
        //        string password = System.Web.Security.Membership.GeneratePassword(7, 0);
        //        string email = DiRoleHandler.RoleDiIp + ticks + "@di.se";
        //        //System.Web.Security.Membership.CreateUser(username, password, email);

        //        //MembershipProvider mp = System.Web.Security.Membership.Providers[MembershipSettings.DiMembershipProviderName];
        //        //mp.CreateUser(username, password, email);
                
        //        //Get user and set role
        //        System.Web.Security.MembershipUser user = System.Web.Security.Membership.GetUser(username);
        //        Roles.AddUserToRole(user.UserName, "SMSGroup");

        //        //RoleProvider rp = Roles.Providers[MembershipSettings.DiRoleProviderName];
        //        //rp.AddUsersToRoles(new string[] { user.UserName }, new string[] { "DiGold" });

        //        //login user                
        //        //FormsAuthentication.SetAuthCookie(user.UserName, false);
        //        authCookie = FormsAuthentication.GetAuthCookie(user.UserName, false);

        //        if (authCookie != null)
        //        {
        //            authCookie.Expires = DateTime.Now.AddMinutes(1);
        //            Response.Cookies.Add(authCookie);
        //            new Logger("CreateIpAutoLoginCookie() - authCookie successfully set.");
        //        }
        //        else
        //        {
        //            new Logger("CreateIpAutoLoginCookie() - authCookie equals null.", "");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("CreateIpAutoLoginCookie() - failed", ex.ToString());
        //    }
        //}


    }
}