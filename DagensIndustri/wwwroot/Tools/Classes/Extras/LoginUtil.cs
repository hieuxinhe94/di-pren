using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using EPiServer;
using EPiServer.Core;
using DagensIndustri.Tools.Classes.Membership;
using DIClassLib.Misc;
using System.Web.UI.WebControls;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Tools.Classes.Extras
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
            MembershipProvider mp = System.Web.Security.Membership.Providers["SqlServerMembershipProvider"];
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
                                                                                       MembershipDbHandler.GetExpireDate(userid));
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
            FormsAuthentication.SignOut();
            HttpContext.Current.Session.Abandon();
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
            FormsAuthentication.SetAuthCookie(userName, false);
        }
    }
}