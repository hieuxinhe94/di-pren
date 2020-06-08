using System;
using System.Web.Security;
using System.Collections.Specialized;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using System.Data;
using System.Collections.Generic;


namespace DIClassLib.Membership
{
    public class DiRoleProvider : RoleProvider
    {
        //public String ConnectionStringName { get; set; }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (String.IsNullOrEmpty(name))
                name = MembershipSettings.DiRoleProviderName;
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "$safeprojectname$ Role Provider");
            }
            base.Initialize(name, config);
        }

        public override string ApplicationName
        {
            get;
            set;
        }


        public override string[] GetAllRoles()
        {
            DataSet roles = null;
            roles = SqlHelper.ExecuteDataset("DisePren", "GetAllRoles", null);

            List<string> ret = new List<string>();

            foreach (DataRow dr in roles.Tables[0].Rows)
                ret.Add(dr["role"].ToString());

            return ret.ToArray();
        }

        public override string[] GetRolesForUser(string username)
        {
            List<string> roles = new List<string>();

            DataSet RolesForUser = null;
            RolesForUser = SqlHelper.ExecuteDataset("DisePren", "GetRolesForCustomer", new SqlParameter("userId", username));

            foreach (DataRow dr in RolesForUser.Tables[0].Rows)
                roles.Add(dr["role"].ToString());

            return roles.ToArray();
        }

        public override bool IsUserInRole(string username, string roleName)
        {
            int IsInRole = int.Parse(SqlHelper.ExecuteScalarParam("DisePren", 
                                                                     "IsCustomerInRole", 
                                                                     new SqlParameter[] 
                                                                     { 
                                                                         new SqlParameter("@userId", username), 
                                                                         new SqlParameter("@role", roleName) 
                                                                     }
                                                                  ).ToString());
            if (IsInRole > 0)
                return true;

            return false;
        }

        public override bool RoleExists(string roleName)
        {
            int roleExists = int.Parse(SqlHelper.ExecuteScalar("DisePren", "RoleExists", new SqlParameter("@role", roleName)).ToString());

            if (roleExists > 0)
                return true;

            return false;
        }


        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            foreach (string usr in usernames)
            {
                foreach (string role in roleNames)
                    AddUserToRole(usr, role);
            }
        }
        
        private void AddUserToRole(string username, string rolename)
        {
            SqlHelper.ExecuteNonQuery("DisePren", 
                                        "AddCustomerInRole", 
                                        new SqlParameter[] 
                                        { 
                                            new SqlParameter("@userId", username), 
                                            new SqlParameter("@role", rolename) 
                                        });
        }

        //130409 /TKM no longer in use. SP only exist on test
        //public void AddUsersByCusnoToRoles(string[] cusnos, string[] roleNames)
        //{
        //    foreach (string cusno in cusnos)
        //    {
        //        foreach (string role in roleNames)
        //            AddUserByCusnoToRole(cusno, role);
        //    }
        //}
        //private void AddUserByCusnoToRole(string cusno, string rolename)
        //{
        //    SqlHelper.ExecuteNonQuery("DisePren",
        //                                "AddCustomerByCusnoInRole",
        //                                new SqlParameter[] 
        //                                { 
        //                                    new SqlParameter("@cusno", cusno), 
        //                                    new SqlParameter("@role", rolename) 
        //                                });
        //}


        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            foreach (string usr in usernames)
            {
                if (!string.IsNullOrEmpty(usr))
                {
                    foreach (string role in roleNames)
                    {
                        if (!string.IsNullOrEmpty(role))
                        {
                            RemoveUserFromRole(usr, role);
                        }
                    }
                }
            }
        }

        private void RemoveUserFromRole(string username, string rolename)
        {
            try
            {
                SqlHelper.ExecuteNonQuery("DisePren",
                                          "DeleteCustomerInRole",
                                          new SqlParameter[] 
                                          { 
                                              new SqlParameter("@userId", username), 
                                              new SqlParameter("@role", rolename), 
                                              new SqlParameter("@comment", DBNull.Value) 
                                          });
            }
            catch (Exception ex)
            {
                new Logger("RemoveUserFromRole() - failed for username:" + username + ", rolename:" + rolename, ex.ToString());
            }
        }


        #region Not Implemented Operations
        
        public override string[] GetUsersInRole(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void CreateRole(string roleName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion

    }
}