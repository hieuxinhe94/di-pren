using System;
using System.Web.Security;
using System.Collections.Specialized;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using DIClassLib.DbHelpers;
//using DagensIndustri.Tools.Classes.Membership;


namespace DIClassLib.Membership
{
    public class DiMembershipProvider : MembershipProvider
    {
        //All users
        private string _smsUserName = DiRoleHandler.RoleDiSms24Hour;
        private string _ipLoginUserName = DiRoleHandler.RoleDiIp;

        
        public String ConnectionStringName { get; set; }

        
        public override string ApplicationName
        {
            get;
            set;
        }

        // Initialize the methods
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");
            if (String.IsNullOrEmpty(name))
                name = MembershipSettings.DiMembershipProviderName;
            if (string.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Membership $safeprojectname$ Provider");
            }
            if (string.IsNullOrEmpty(config["connectionStringName"]))
                throw new System.Configuration.Provider.ProviderException("Connection name not specified");
            else
                ConnectionStringName = config["connectionStringName"];

            base.Initialize(name, config);
        }


        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            MembershipUserCollection users = new MembershipUserCollection { GetUser("diAdmin", false) };
            totalRecords = 1;
            return users;
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            DateTime now = DateTime.Now;
            string comment = string.Empty;

            if (username.Equals(_smsUserName) || username.StartsWith(_smsUserName + "__"))
                comment = "Alla SMS-kunder får detta namn som prefix i sitt användarnamn. Sök i DISE för användarinformation";
            else if (username.Equals(_ipLoginUserName))
                comment = "Alla ip-inloggade loggas in som denna användare";
            else
                comment = "Användare vars inloggningsuppgifter finns i DISEPren databasen.";

            return new MembershipUser(MembershipSettings.DiMembershipProviderName,
                                        username,
                                        username,
                                        "",
                                        "",
                                        comment,
                                        true,
                                        false,
                                        now,
                                        now,
                                        now,
                                        now,
                                        DateTime.MinValue);
        }

        public override bool ValidateUser(string username, string password)
        {
            SqlDataReader DR = null;
           
            try
            {

                DR = SqlHelper.ExecuteReader("DisePren", 
                                                "DoLoginV3", 
                                                new SqlParameter[]
                                                {
                                                    new SqlParameter("@userid", username), 
                                                    new SqlParameter("@passwd", password),
                                                    new SqlParameter("@ipNumber", System.Web.HttpContext.Current.Request.UserHostAddress),
                                                    new SqlParameter("@browser", System.Web.HttpContext.Current.Request.UserAgent),
                                                    new SqlParameter("@fileName", System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.FilePath))
                                                }
                                            );
                if (DR.Read())
                {
                    //Set session with Expire date, used in Login.aspx.cs.OnLoggedIn
                    //if (DR["expiredate"] != System.DBNull.Value)
                    //    System.Web.HttpContext.Current.Session["Expires"] = DR["expireDate"]; //set session, this session is used on login page

                    return true;
                
                }
                
                return false;
            }
            catch (Exception ex)
            {
                new Logger(ConnectionStringName + " - ValidateUser(string username, string password) - failed", ex.ToString());
                return false;
            }
            finally
            {
                if (DR != null)
                    DR.Close();
            }
        }



        //public override bool ValidateUser(string username, string password)
        //{
        //    SqlDataReader DR = null;

        //    try
        //    {

        //        SqlParameter[] arr = new SqlParameter[]
        //        {
        //            new SqlParameter("@userid", DBNull.Value), 
        //            new SqlParameter("@email", DBNull.Value),
        //            new SqlParameter("@passwd", password),
        //            new SqlParameter("@ipNumber", "123.123.123")
        //        };

        //        if (!Functions.IsValidEmail(username))
        //            arr[0] = new SqlParameter("@userid", username);
        //        else
        //            arr[1] = new SqlParameter("@email", username);


        //        DR = SqlHelper.ExecuteReader(ConnectionStringName, "DoLoginV2", arr);
        //        if (DR.Read())
        //        {
        //            //if (DR["result"] != System.DBNull.Value && DR["result"].ToString() == "0")
        //            //{
        //            //Set session with Expire date, used in Login.aspx.cs.OnLoggedIn
        //            if (DR["expiredate"] != System.DBNull.Value)
        //                System.Web.HttpContext.Current.Session["Expires"] = DR["expireDate"]; //set session, this session is used on login page

        //            return true;
        //            //}
        //            //else
        //            //{
        //            //    return false;
        //            //}
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("ValidateUser(string username, string password) - failed", ex.ToString());
        //        return false;
        //    }
        //    finally
        //    {
        //        if (DR != null)
        //            DR.Close();
        //    }
        //}


        #region Not Implemented Operations

        public override bool EnablePasswordRetrieval
        {
            get { throw new Exception("The method or operation is not implemented."); }
        }
        public override bool EnablePasswordReset { get { throw new Exception("The method or operation is not implemented."); } }
        public override bool RequiresQuestionAndAnswer { get { throw new Exception("The method or operation is not implemented."); } }
        public override bool RequiresUniqueEmail { get { throw new Exception("The method or operation is not implemented."); } }
        public override MembershipPasswordFormat PasswordFormat { get { throw new Exception("The method or operation is not implemented."); } }
        public override int MaxInvalidPasswordAttempts { get { throw new Exception("The method or operation is not implemented."); } }
        public override int PasswordAttemptWindow { get { throw new Exception("The method or operation is not implemented."); } }
        public override int MinRequiredPasswordLength { get { throw new Exception("The method or operation is not implemented."); } }
        public override int MinRequiredNonAlphanumericCharacters { get { throw new Exception("The method or operation is not implemented."); } }
        public override string PasswordStrengthRegularExpression { get { throw new Exception("The method or operation is not implemented."); } }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool UnlockUser(string userName)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override string GetPassword(string username, string answer)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new Exception("The method or operation is not implemented.");
        }


        #endregion

    }
}