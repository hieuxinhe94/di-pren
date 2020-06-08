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
using System.Configuration;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class Complaint : DiTemplatePage
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

        public DateTime DateComplaint
        {
            get
            {
                DateTime dt = DateTime.MinValue;
                DateTime.TryParse(Date1.Text, out dt);
                return dt;
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
                Date1.MaxValue = DateTime.Now.ToShortDateString();
        }

        private void HandleNotLoggedIn()
        {
            MySettingsMenu1.Visible = false;
            PlaceHolderForm.Visible = false;
            ShowMessage("/mysettings2/notloggedin", true, true);
        }


        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            StringBuilder err = new StringBuilder();
            int numDays = int.Parse(DropDownListNumDays.SelectedValue.ToString());

            if (!Page.IsValid)
            {
                ShowMessage("Ett tekniskt fel uppstod. Kontrollera att fälten är korrekt formaterade och försök igen.", false, true);
                return;
            }

            if (DateComplaint == DateTime.MinValue)
                err.Append("Ange datum.<br>");

            if (err.ToString().Length > 0)
            {
                ShowMessage("Ett tekniskt fel uppstod. " + err.ToString(), true, true);
                return;
            }

            string mail = ConfigurationManager.AppSettings["mailPrenDiSe"];
            MiscFunctions.SendMail(mail, mail, "Reklamation", GetComplaintsMailText(numDays), true);

            PlaceHolderForm.Visible = false;
            ShowMessage("Din reklamation har skickats.", false, false);
        }

        private string GetComplaintsMailText(int numDays)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Reklamation via \"mina sidor\".<br><br>");
            sb.Append("Kundnummer: " + Subscriber.Cusno.ToString() + "<br>");
            sb.Append("E-post: " + Subscriber.Email + "<br>");
            sb.Append("Från datum: " + DateComplaint.ToShortDateString() + "<br>");
            sb.Append("Antal dagar: " + numDays.ToString() + "<br>");
            sb.Append("Orsak: " + DropDownListReason.SelectedValue);
            return sb.ToString();
        }

    }
}