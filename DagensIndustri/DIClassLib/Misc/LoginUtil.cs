using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using EPiServer;
using EPiServer.Core;
using DIClassLib.Membership;
using DIClassLib.Misc;
using System.Web.UI.WebControls;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using DIClassLib.SingleSignOn;


namespace DIClassLib.Misc
{
    public class LoginUtil
    {
        
        public static bool Authenticate(PageData loginPage, ref string userName, string password, out string emailUserName, out string failureText)
        {
            bool authenticated = false;
            failureText = string.Empty;
            emailUserName = string.Empty;

            //UserName is emailaddress - get username for user
            if (MiscFunctions.IsValidEmail(userName))
            {
                emailUserName = userName;
                string userId = MiscFunctions.GetUserIdByEmail(emailUserName);

                switch (userId)
                {
                    case "-1":  //no user with matching email
                        failureText = loginPage["ForgotPasswordErrorText"] as string;
                        break;
                    case "-2":  //user has more then 1 active subscription
                        failureText = loginPage["ForgotPasswordErrorTextMultiple"] as string;
                        break;
                    case "-3":  //error
                        break;
                    default:    //one active subscriber
                        userName = userId;
                        break;
                }
            }

            //password = password.TrimEnd(' ').TrimStart(' ');

            authenticated = LoginUser(userName, password);
            return authenticated;
        }

        public static bool LoginUser(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return false;

            bool authenticated = TryLoginUsingDiProvider(userName, password);

            if (!authenticated)
                authenticated = TryLoginUsingEpiProvider(userName, password);

            return authenticated;
        }
                
        private static bool TryLoginUsingDiProvider(string userName, string password)
        {
            MembershipProvider mp = System.Web.Security.Membership.Providers[MembershipSettings.DiMembershipProviderName];
            bool authenticated = mp.ValidateUser(userName, password);

            if (authenticated)
            {
                //remove customer from old NON webProduct roles
                RoleProvider rp = Roles.Providers[MembershipSettings.DiRoleProviderName];
                rp.RemoveUsersFromRoles(new string[] { userName }, DiRoleHandler.GetAllNonWebProductRoles());  //rp.GetAllNonWebProductRoles

                //add customer to NON webProduct roles roles depending on subscription info
                rp.AddUsersToRoles(new string[] { userName }, DiRoleHandler.GetUsersRolesFromDb(userName));
            }

            return authenticated;
        }

        private static bool TryLoginUsingEpiProvider(string userName, string password)
        {
            MembershipProvider mp = System.Web.Security.Membership.Providers[MembershipSettings.EpiMembershipProviderName];
            return mp.ValidateUser(userName, password);
        }


        public static void HandleCookieExpireDate(string userid)
        {
            try
            {
                 HttpCookie authCookie = System.Web.HttpContext.Current.Request.Cookies[FormsAuthentication.FormsCookieName];
                 MembershipUser usr = System.Web.Security.Membership.GetUser(userid, true);

                 if (authCookie != null)
                 {
                     if (usr.ProviderName.ToString() == MembershipSettings.DiMembershipProviderName)
                     {
                         long timeOutMins = Microsoft.VisualBasic.DateAndTime.DateDiff(Microsoft.VisualBasic.DateInterval.Minute,
                                                                                       DateTime.Now,
                                                                                       DateTime.Now.AddDays(1));  //MembershipDbHandler.GetExpireDate(userid)
                         if (timeOutMins > 0)
                         {
                             FormsAuthentication.Initialize();
                             FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(userid, true, (int)timeOutMins);
                             string hash = FormsAuthentication.Encrypt(ticket);
                             HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hash);

                             if (ticket.IsPersistent)
                                 cookie.Expires = ticket.Expiration;

                             HttpContext.Current.Response.Cookies.Add(cookie);
                         }
                     }
                 }
            }
            catch (Exception ex)
            {
                new Logger("HandleCookieExpireDate() - failed for userid:" + userid, ex.ToString());
            }
        }

        /// <summary>
        /// Sign out current user
        /// </summary>
        public static void LogoutUser()
        {
            //if (HttpContext.Current == null || HttpContext.Current.Session == null)
            //    return;

            var lgn = new SsoLoginHandler();
            lgn.IsLoggedInToBonDig = false;
            lgn.Token = null;

            FormsAuthentication.SignOut();

            //if (HttpContext.Current.Session != null)
            //    HttpContext.Current.Session.Abandon();


        }

        /// <summary>
        /// Sign the user out and in 
        /// </summary>
        public static void ReLoginUser()
        {
            string userName = HttpContext.Current.User.Identity.Name;
            
            //Sign out current user
            LogoutUser();

            //Create an authentication ticket for user
            if(!string.IsNullOrEmpty(userName))
                FormsAuthentication.SetAuthCookie(userName, false);
        }


        public static bool ReLoginUserRefreshCookie(string userName, string password)
        {
            //if (HttpContext.Current != null && HttpContext.Current.User.Identity.IsAuthenticated)
            LogoutUser();

            Roles.DeleteCookie();

            if (LoginUser(userName, password))
            {
                FormsAuthentication.SetAuthCookie(userName, false);
                return true;
            }

            return false;
        }


        public static bool TryLoginUserToDagensIndstri(long cusno)
        {
            List<string> userAndPass = MsSqlHandler.GetUsernameAndPasswd(cusno);
            string user = userAndPass[0];
            string pass = userAndPass[1];
            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(pass))
                return ReLoginUserRefreshCookie(user, pass);

            return false;
        }
    }
}