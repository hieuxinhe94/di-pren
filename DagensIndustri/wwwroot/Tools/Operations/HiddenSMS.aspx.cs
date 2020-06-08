using System;
using System.Web;
using System.Web.Security;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Membership;

namespace DagensIndustri.Tools.Operations
{
    public partial class HiddenSMS : System.Web.UI.Page
    {
        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);

            if(!User.IsInRole(DiRoleHandler.RoleDiSms24Hour))
                CheckSms();
        }

        private void CheckSms()
        {
            string smscode = string.Empty;
            string myUid = string.Empty;
            string payStatus = string.Empty;

            if (!(Session["SmsCode"] == null | Session["myUid"] == null))
            {
                smscode = Session["SmsCode"].ToString();
                myUid = Session["myUid"].ToString();
            }

            //test values (see db)
            //smscode = "1685";
            //myUid = "hrgnru555klenm252uqukn55320462804";

            //localhost (test db)
            //smscode = "8";
            //myUid = "et4a2qi0kqb1ksiepbaiit55353302111";

            payStatus = TryGetPayStatus(smscode, myUid);
            
            if (payStatus.ToLower() == "yes")
            {
                HttpCookie authCookie = TryCreateAuthCookieByEpiProvider(smscode);

                if (authCookie != null)
                {
                    authCookie.Expires = DateTime.Now.AddHours(24);
                    Response.Cookies.Add(authCookie);
                }
                else
                {
                    new Logger("CheckSms() - authCookie is null. SmsCode=" + smscode + ", " + "UID=" + myUid, "");
                }
            }

            Response.Clear();
            Response.Expires = -1;
            Response.Write(payStatus);
            Response.End();
        }

        private string TryGetPayStatus(string smscode, string myUid)
        {
            if (string.IsNullOrEmpty(smscode) || string.IsNullOrEmpty(myUid))
                return "nohit";

            SqlDataReader DR = null;

            try
            {
                SqlParameter[] sqlParameters = new SqlParameter[] 
                { 
                    new System.Data.SqlClient.SqlParameter("@smscode", smscode), 
                    new System.Data.SqlClient.SqlParameter("@myUid", myUid) 
                };

                DR = SqlHelper.ExecuteReader("DisePren", "checkSmsPay", sqlParameters);

                if (DR.Read())
                    return DR["Payed"].ToString().Trim();
                else
                    return "nohit";

                //new EPiServer.Utility.Logger("checkSMS() - checkSmsPay ResultStr equals " + ResultStr, myUid);
            }
            catch (Exception ex)
            {
                //TODO: No idea of logging, the thing starts every 5 seconds, that can be alot of logging if trouble with connection to db
                //new EPiServer.Utility.Logger("checkSMS() - failed", ex.ToString());
                throw ex;
            }
            finally
            {
                if (DR != null)
                    DR.Close();
            }
        }


        public HttpCookie TryCreateAuthCookieByEpiProvider(string smscode)
        {
            try
            {
                HttpCookie authCookie = null;

                string username = string.Format("sms{0}", smscode);
                string password = Guid.NewGuid().ToString().ToLower().Substring(0, 7);
                Session["password"] = password;
                string email = string.Format("sms{0}@.di.se", smscode);

                MembershipProvider memProv = Membership.Providers[MembershipSettings.EpiMembershipProviderName];
                
                MembershipCreateStatus status = new MembershipCreateStatus();
                memProv.CreateUser(username, password, email, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), true, Guid.NewGuid(), out status);

                MembershipUser user = memProv.GetUser(username, false);

                RoleProvider roleProv = Roles.Providers[MembershipSettings.EpiRoleProviderName];
                roleProv.AddUsersToRoles(new string[] { username }, new string[] { DiRoleHandler.RoleDiSms24Hour });

                //login user
                authCookie = FormsAuthentication.GetAuthCookie(user.UserName, false);

                new Logger("TryCreateAuthCookieByEpiProvider() - success for username=" + username + ", " + "password=" + password);

                return authCookie;
            }
            catch (Exception ex)
            {
                new Logger("TryCreateAuthCookieByEpiProvider() - failed for smscode=" + smscode, ex.ToString());
            }

            return null;
        }


        //public HttpCookie TryCreateAuthCookie(string smscode)
        //{
        //    try
        //    {
        //        HttpCookie authCookie = null;

        //        //Create user
        //        string username = string.Format("sms{0}", smscode);
        //        //string password = Membership.GeneratePassword(7, 0);                  //generates non alpha numeric symbols
        //        string password = Guid.NewGuid().ToString().ToLower().Substring(0, 7);
        //        Session["password"] = password;
        //        string email = string.Format("sms{0}@.di.se", smscode);
        //        Membership.CreateUser(username, password, email);

        //        //Get user and set role
        //        MembershipUser user = Membership.GetUser(username);
        //        Roles.AddUserToRole(user.UserName, DiRoleHandler.RoleDiSms24Hour);
                    
        //        //login user                
        //        //FormsAuthentication.SetAuthCookie(user.UserName, false);              //replaced 120605
        //        authCookie = FormsAuthentication.GetAuthCookie(user.UserName, false);

        //        new Logger("TryCreateAuthCookie() - success for username=" + username + ", " + "password=" + password);

        //        return authCookie;

        //        #region old code
        //        //string userName = Functions.GetAppsettingsValue("SmsUser") + "__" + myUid;
        //        //FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddHours(24), true, string.Empty);
        //        //string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
        //        //authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        //will happen if user already exists etc
        //        new Logger("TryCreateAuthCookie() - failed for smscode=" + smscode, ex.ToString());
        //    }

        //    return null;
        //}

    
    }
}