using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using System.Data.SqlClient;
using System.Web.Security;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;

namespace DIClassLib.Membership
{

    /// <summary>
    /// Class holds rules for what roles a subscriber is part of depending on subscription information.
    /// Also holds all role names.
    /// </summary>
    public static class DiRoleHandler
    {
        
        //available roles (also in db table Roles)
        public static string RoleDiRegular     = "DiRegular";
        public static string RoleDiGold        = "DiGold";
        public static string RoleDiIp          = "DiIp";
        public static string RoleDiPdf         = "DiPdf";
        public static string RoleDiWeekend     = "DiWeekend";
        public static string RoleDiWineClub    = "DiWineClub";
        public static string RoleDiWineClubSms = "DiWineClubSms";
        public static string RoleDiIpad        = "DiIpad";
        public static string RoleDiSms24Hour   = "SMSGroup";  //DiSms24Hour
        public static string RoleDiY           = "DiY";
        public static string RoleAgenda        = "Agenda";
        public static string RoleDiHybrid      = "DiHybrid";
      

        public static string[] GetUsersRolesFromDb(string username)
        {
            HashSet<string> roles = new HashSet<string>();      //using HashSet to aviod duplicate values in list

            DataSet ds = null;
            ds = SqlHelper.ExecuteDataset("DisePren", "GetActiveSubscriptions", new SqlParameter("@userId", username));

            long cusno = 0;
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                long.TryParse(ds.Tables[0].Rows[0]["cusno"].ToString(), out cusno);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    DateTime dt;
                    DateTime expireDate = (DateTime.TryParse(dr["expireDate"].ToString(), out dt) ? dt : DateTime.MinValue);
                    string productNo = dr["productNo"].ToString().ToUpper();
                    string paperCode = dr["paperCode"].ToString().ToUpper();

                    if (expireDate > DateTime.MinValue)
                    {
                        // Not RoleDiRegular for Agenda and DiY!
                        if (productNo == Settings.ProductNo_Regular &&
                            (paperCode == Settings.PaperCode_DI || paperCode == Settings.PaperCode_DISE ||
                             paperCode == Settings.PaperCode_IPAD))
                            roles.Add(RoleDiRegular);

                        
                        if (productNo == Settings.ProductNo_Weekend && paperCode == Settings.PaperCode_DI)
                            roles.Add(RoleDiWeekend);

                        if (productNo == Settings.ProductNo_Regular && paperCode == Settings.PaperCode_DIY)
                            roles.Add(RoleDiY);

                        if (paperCode == Settings.PaperCode_DISE)
                            roles.Add(RoleDiPdf);

                        if (paperCode == Settings.PaperCode_IPAD || productNo == Settings.PaperCode_IPAD)
                            roles.Add(RoleDiIpad);

                        if (paperCode == Settings.PaperCode_AGENDA)
                            roles.Add(RoleAgenda);
                    }
                }
            }

            if (roles.Contains(RoleDiWeekend) && cusno > 0)
            {
                if (SubscriptionController.IsHybridSubscriber(cusno))
                {
                    roles.Add(RoleDiHybrid);
                    roles.Add(RoleDiIpad);      //allow user to read PDF on weekdays
                }
            }

            return roles.ToArray();
        }


        public static string[] GetAllNonWebProductRoles()
        {
            DataSet roles = null;
            roles = SqlHelper.ExecuteDataset("DisePren", "GetAllNonWebProductRoles", null);

            List<string> ret = new List<string>();

            foreach (DataRow dr in roles.Tables[0].Rows)
                ret.Add(dr["role"].ToString());

            return ret.ToArray();
        }


        /// <summary>
        /// Add roles to the current user. 
        /// The user is signed out and in again in order to have the roles to be set properly
        /// </summary>
        public static void AddUserToRoles(string[] roles)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated && roles != null && roles.Length > 0)
            {
                //Delete cookies where role names are cached
                Roles.DeleteCookie();

                //Add user to DiGold role
                RoleProvider rp = Roles.Providers[MembershipSettings.DiRoleProviderName];
                rp.AddUsersToRoles(new string[] { HttpContext.Current.User.Identity.Name }, roles);

                //Sign out and in again in order to have the roles to be set properly on current user
                LoginUtil.ReLoginUser();
            }
        }

        public static void AddUserToRoles(string username, string[] roles)
        {
                //Add user to DiGold role
                RoleProvider rp = Roles.Providers[MembershipSettings.DiRoleProviderName];
                rp.AddUsersToRoles(new string[] { username }, roles);
        }

        //130409 /TKM no longer in use
        //public static void AddUserByCusnoToRoles(string cusno, string[] roles)
        //{
        //    //Add user to DiGold role
        //    DiRoleProvider rp = Roles.Providers[MembershipSettings.DiRoleProviderName] as DiRoleProvider;            
        //    rp.AddUsersByCusnoToRoles(new string[] { cusno }, roles);
        //}

        /// <summary>
        /// Add roles to the current user. 
        /// The user is signed out and in again in order to have the roles to be set properly
        /// </summary>
        public static void RemoveUserFromRoles(string[] roles)
        {
            if (HttpContext.Current.User.Identity.IsAuthenticated && roles != null && roles.Length > 0)
            {
                //Delete cookies where role names are cached
                Roles.DeleteCookie();

                //Add user to DiGold role
                RoleProvider rp = Roles.Providers[MembershipSettings.DiRoleProviderName];
                rp.RemoveUsersFromRoles(new string[] { HttpContext.Current.User.Identity.Name }, roles);
                
                //Sign out and in again in order to have the roles to be set properly on current user
                LoginUtil.ReLoginUser();
            }
        }
    }
}