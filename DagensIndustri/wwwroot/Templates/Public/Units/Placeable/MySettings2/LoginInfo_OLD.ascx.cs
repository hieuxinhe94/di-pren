using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DIClassLib.Subscriptions;
using DIClassLib.DbHelpers;
using System.Text;
using DIClassLib.Misc;
using System.Web.Security;


namespace DagensIndustri.Templates.Public.Units.Placeable.MySettings2
{
    public partial class LoginInfo_OLD : UserControlBase
    {

        private DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2 MySettingsPage
        {
            get
            {
                return (DagensIndustri.Templates.Public.Pages.MySettings2)Page;
            }
        }

        public SubscriptionUser2 Subscriber
        {
            get
            {
                return (SubscriptionUser2)MySettingsPage.Subscriber;
            }
        }

        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                InputUsername.Text = Subscriber.UserName;
        }


        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            string username = InputUsername.Text.ToUpper();
            string passOld = InputPassOld.Text.ToUpper();
            string passNew1 = InputPassNew1.Text.ToUpper().Trim();
            string passNew2 = InputPassNew2.Text.ToUpper().Trim();

            string errVal = GetValidatonErr(username, passOld, passNew1, passNew2);
            if (errVal.Length > 0)
            {
                MySettingsPage.ShowMessage(errVal, false, true);
                return;
            }

            bool saveNewPass = (!string.IsNullOrEmpty(passNew1) && (passNew1 == passNew2) && (passNew1 != passOld));
            bool saveNewUsername = (!string.IsNullOrEmpty(username) && username != Subscriber.UserName.ToUpper());

            if (saveNewPass)
            {
                if (!Subscriber.UpdatePasswd(passNew1))
                {
                    MySettingsPage.ShowMessage(GetSaveNewPassErr(saveNewUsername), false, true);
                    return;
                }
            }
            
            if (saveNewUsername)
            {
                int updUsrNameRet = Subscriber.UpdateUserName(username);
                string err = GetSaveNewUsernameErr(updUsrNameRet, saveNewPass);
                if (err.Length > 0)
                {
                    MySettingsPage.ShowMessage(err, false, true);
                    return;
                }

                if (updUsrNameRet == 1)
                {
                    string pass = saveNewPass ? passNew1 : passOld;
                    FormsAuthentication.SignOut();
                    FormsAuthentication.SetAuthCookie(username, false);
                    //MySettingsPage.UsernameUpdatedSessionProp = true;
                    //Response.Redirect(CurrentPage.LinkURL);
                    Response.Redirect("http://localhost/templates/public/pages/mysettings2.aspx");
                }
            }
                        
            //MySettingsPage.ShowMessageSuccess("Dina inloggningsuppgifter har sparats.", false, false);
        }

        private string GetValidatonErr(string username, string passOld, string passNew1, string passNew2)
        {
            if (!Page.IsValid)
            {
                new Logger("ButtonSave_Click - page not valid", "Cusno: " + Subscriber.Cusno + ", username:" + username + ", passOld:" + passOld + ", passNew1:" + passNew1 + ", passNew2:" + passNew2);
                return Translate("/mysettings2/errorpagenotvalid");
            }

            if (passOld != Subscriber.Password.ToUpper())
                return "Fältet 'nuvarande lösenord' är inte korrekt angivet. Var god försök igen.";

            if (passNew1 != passNew2)
                return "Fälten med 'nytt lösenord' måste vara identiska. Var god försök igen.";
        
            return string.Empty;
        }

        private string GetSaveNewPassErr(bool saveNewUsername)
        {
            StringBuilder err = new StringBuilder();
            err.Append("Ett tekniskt fel uppstod när lösenordet skulle sparas. ");

            if (saveNewUsername)
                err.Append("<br>- Användarnamnet har inte sparats.");

            err.Append("<br>- Lösenordet har inte sparats.");

            return err.ToString();
        }

        private string GetSaveNewUsernameErr(int updUsrNameRet, bool saveNewPass)
        {
            StringBuilder err = new StringBuilder();

            if (updUsrNameRet == -1)
                err.Append("Användarnamnet är upptaget. Var god försök med ett annat användarnamn. ");

            if (updUsrNameRet == -2)
                err.Append("Ett tekniskt fel uppstod när användarnamnet skulle sparas. ");

            if (err.ToString().Length > 0)
            {
                err.Append("<br>- Användarnamnet har inte sparats.");

                if (saveNewPass)
                    err.Append("<br>- Det nya lösenordet har dock sparats.");
            }

            return err.ToString();
        }


        //protected void SaveUserName_Click(object sender, EventArgs e)
        //{
        //    //first check that username doesn't start with reserved name for sms-users
        //    string reservedUserName = DiRoleHandler.RoleDiSms24Hour;

        //    string newUserName = UserNameInput.Text.Trim();

        //    //check if changed
        //    if (Page.IsValid && newUserName != Subscriber.UserName && !newUserName.StartsWith(reservedUserName))
        //    {
        //        //check that new username not already in use
        //        long result = CheckIfUsernameAlreadyExist(newUserName);
        //        if (result == 0)
        //        {
        //            if (UpdateUser(newUserName, "", ""))
        //            {
        //                //Sign out current user
        //                LoginUtil.LogoutUser();
        //                //Sign in with new username
        //                FormsAuthentication.SetAuthCookie(newUserName, false);
        //                //Redirect, because the old UserName is cached
        //                Response.Redirect(CurrentPage.LinkURL);
        //            }
        //            else
        //            {
        //                //Error in UpdateUser
        //                MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
        //                new Logger("SaveUserName_Click - failed to update data in Cirix", "Cusno: " + Subscriber.Cusno);
        //            }
        //        }
        //        else if (result != -2)
        //        {
        //            //UserName occupied
        //            MySettingsPage.ShowMessage("/mysettings/errors/username/usernameexists", true, true);
        //        }
        //        else
        //        {
        //            //-2 = An error occured in CheckIfUsernameAlreadyExist()
        //            MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
        //        }
        //    }
        //}


        //protected void SavePassword_Click(object sender, EventArgs e)
        //{
        //    string newPasswordInput = NewPasswordInput.Text;
        //    if (Page.IsValid && newPasswordInput != Subscriber.Password)
        //    {
        //        string oldPasswordInput = OldPasswordInput.Text;
        //        string confirmedPasswordInput = NewPasswordConfirmationInput.Text;

        //        //check current password
        //        if (oldPasswordInput != Subscriber.Password)
        //        {
        //            MySettingsPage.ShowMessage("/mysettings/errors/password/oldpasswordmismatch", true, true);
        //            return;
        //        }

        //        //check that new password and confirm password are eqeual
        //        if (newPasswordInput != confirmedPasswordInput)
        //        {
        //            MySettingsPage.ShowMessage("/mysettings/errors/password/newpasswordmismatch", true, true);
        //            return;
        //        }

        //        //Username is not allowed as password
        //        if (newPasswordInput == Subscriber.UserName)
        //        {
        //            MySettingsPage.ShowMessage("/mysettings/errors/password/passwordmatchusername", true, true);
        //            return;
        //        }

        //        //Must be >=6 in length
        //        if (newPasswordInput.Length < 6)
        //        {
        //            MySettingsPage.ShowMessage("/mysettings/errors/password/passwordtooshort", true, true);
        //            return;
        //        }

        //        //Update user info
        //        if (UpdateUser("", newPasswordInput, ""))
        //        {
        //            MySettingsPage.InitUserInfo();
        //            SetDefaultValues();
        //        }
        //        else
        //        {
        //            MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
        //            new Logger("SavePassword_Click - failed to update data in Cirix", "Cusno: " + Subscriber.Cusno);
        //        }
        //    }
        //}


        /// <summary>
        /// Checks if Username is occupied
        /// </summary>
        /// <param name="strUserName"></param>
        /// <returns>0 if not already exist, -1 if more than one exist, return cusno if user already exist, -2 an error occured</returns>
        //private long CheckIfUsernameAlreadyExist(string strUserName)
        //{
        //    try
        //    {
        //        return Subscriber.IsUserNameInUse(strUserName.ToUpper());
        //    }
        //    catch (Exception ex)
        //    {
        //        new Logger("CheckIfUsernameAlreadyExist() - failed", ex.ToString());
        //        MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
        //        return -2;
        //    }
        //}


    }
}