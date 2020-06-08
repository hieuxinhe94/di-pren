using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Templates.Public;
using DagensIndustri.Tools.Classes;
using DIClassLib.Membership;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using System.Text;


namespace DagensIndustri.Templates.Public.Units.Placeable.MySettings2
{
    public partial class PersonInfo : UserControlBase
    {

        public SubscriptionUser2 Subscriber 
        {
            get
            {
                return (SubscriptionUser2)MySettingsPage.Subscriber;
            }
        }

        private DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2 MySettingsPage
        {
            get
            {
                return new DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2();  //(DagensIndustri.Templates.Public.Pages.MySettings2)Page;
            }
        }

        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
                SetDefaultValues();
        }

        
        public void SetDefaultValues()
        {
            InputEmail.Text = Subscriber.Email;
            InputPhone.Text = Subscriber.OPhone;
        }


        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            StringBuilder err = new StringBuilder();
            string email = InputEmail.Text.ToLower();
            string phone = InputPhone.Text;

            if (!Page.IsValid)
            {
                MySettingsPage.ShowMessage("Ett tekniskt fel uppstod. Kontrollera att fälten är korrekt formaterade och försök igen.", false, true);
                new Logger("ButtonSave_Click - page not valid", "Cusno: " + Subscriber.Cusno + ", Email:" + email + ", Mobnr:" + phone);
                return;
            }
            
            if (!string.IsNullOrEmpty(phone))
                phone = MiscFunctions.FormatPhoneNumber(phone, Settings.PhoneMaxNoOfDigits, true);

            //email required (cannot be empty)
            if (MiscFunctions.IsValidEmail(email) && email != Subscriber.Email.ToLower())
            {
                if (!Subscriber.UpdateEmail(email))
                    err.Append("E-postadressen kunde inte sparas. ");
            }

            //phone not required (can be empty)
            if (string.IsNullOrEmpty(phone) || (MiscFunctions.IsValidPhoneMobile(phone) && (phone != Subscriber.OPhone)))
            {
                if (!Subscriber.UpdatePhoneMobile(phone))
                    err.Append("Mobilnumret kunde inte sparas.");
            }

            if (err.ToString().Length > 0)
            {
                MySettingsPage.ShowMessage("Ett tekniskt fel uppstod. " + err.ToString(), true, true);
                return;
            }

            //MySettingsPage.ShowMessageSuccess("Dina personuppgifter har sparats", false, false);
        }



        //protected bool UserIsDIGoldMember
        //{
        //    get
        //    {
        //        bool isUserDIGoldMember = false;
        //        if (Subscriber != null && !string.IsNullOrEmpty(Subscriber.UserName))
        //        {
        //            isUserDIGoldMember = Roles.IsUserInRole(Subscriber.UserName, DiRoleHandler.RoleDiGold);
        //        }
        //        return isUserDIGoldMember;
        //    }
        //}

        /// <summary>
        /// Used to update username, email or password
        /// Updates Oracle and DISEpren
        /// </summary>
        /// <param name="strNewUserName"></param>
        /// <param name="strNewPassword"></param>
        /// <param name="strNewEmail"></param>
        /// <returns>Result string from WS</returns>
        //private bool UpdateUser(string strNewUserName, string strNewPassword, string strNewEmail)
        //{
        //    bool bo = Subscriber.UpdateUser(strNewUserName, strNewPassword, strNewEmail);

        //    if (!bo)
        //    {
        //        new Logger("UpdateUser() - failed", "Cusno: " + Subscriber.Cusno);
        //        MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
        //        return false;
        //    }

        //    return true;
        //}

        
    }
}