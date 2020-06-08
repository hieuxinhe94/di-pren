using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Subscriptions;
using DIClassLib.Misc;
using System.Text;
using DIClassLib.DbHelpers;
using System.Web.UI.HtmlControls;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class PersonInfo : DiTemplatePage, IUserSettingsPage
    {

        public SubscriptionUser2 Subscriber
        {
            get
            {
                if (ViewState["Subscriber"] == null)
                    ViewState["Subscriber"] = new SubscriptionUser2();

                return (SubscriptionUser2)ViewState["Subscriber"];
            }
            set
            {
                ViewState["Subscriber"] = value;
            }
        }


        protected override void OnLoad(System.EventArgs e)
        {
            base.OnLoad(e);
            base.UserMessageControl = UserMessageControl;
            UserMessageControl.ClearMessage();

            if (!User.Identity.IsAuthenticated)
            {
                HandleNotLoggedIn();
                return;
            }

            if (!IsPostBack)
                SetDefaultValues();
        }

        private void HandleNotLoggedIn()
        {
            MySettingsMenu1.Visible = false;
            PlaceHolderForm.Visible = false;
            ShowMessage("/mysettings2/notloggedin", true, true);
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
                ShowMessage("Ett tekniskt fel uppstod. Kontrollera att fälten är korrekt formaterade och försök igen.", false, true);
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
            if (string.IsNullOrEmpty(phone) || (MiscFunctions.IsValidSwePhoneNum(phone) && (phone != Subscriber.OPhone)))
            {
                if (!Subscriber.UpdatePhoneMobile(phone))
                    err.Append("Mobilnumret kunde inte sparas.");
            }

            if (err.ToString().Length > 0)
            {
                ShowMessage("Ett tekniskt fel uppstod. " + err.ToString(), true, true);
                return;
            }

            //System.Threading.Thread.Sleep(3000);

            ShowMessage("Dina personuppgifter har sparats", false, false);
        }

        public void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError)
        {
            ShowMessage(mess, isKey, isError);
        }
    }
}