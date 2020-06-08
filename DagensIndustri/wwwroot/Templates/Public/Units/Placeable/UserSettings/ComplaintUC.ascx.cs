using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DIClassLib.Misc;
using DIClassLib.Subscriptions;
using EPiServer;


namespace DagensIndustri.Templates.Public.Units.Placeable.UserSettings
{
    public partial class ComplaintUC : UserControlBase
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
            UserMessageControl.ClearMessage();

            if (!IsPostBack)
                Date1.MaxValue = DateTime.Now.ToString("yyyy-MM-dd");
        }

        protected void ButtonSave_Click(object sender, EventArgs e)
        {
            var err = new StringBuilder();
            int numDays = int.Parse(DropDownListNumDays.SelectedValue.ToString());

            if (!Page.IsValid)
            {
                UserMessageControl.ShowMessage("Ett tekniskt fel uppstod. Kontrollera att fälten är korrekt formaterade och försök igen.", false, true);
                return;
            }

            if (DateComplaint == DateTime.MinValue)
                err.Append("Ange datum.<br>");

            if (err.ToString().Length > 0)
            {
                UserMessageControl.ShowMessage("Ett tekniskt fel uppstod. " + err.ToString(), true, true);
                return;
            }

            string mail = ConfigurationManager.AppSettings["mailPrenDiSe"];
            MiscFunctions.SendMail(mail, mail, "Reklamation", GetComplaintsMailText(numDays), true);

            PlaceHolderForm.Visible = false;
            UserMessageControl.ShowMessage("Din reklamation har skickats.", false, false);
        }

        private string GetComplaintsMailText(int numDays)
        {
            var sb = new StringBuilder();
            sb.Append("Reklamation via \"mina sidor\".<br><br>");
            sb.Append("Kundnummer: " + Subscriber.Cusno.ToString() + "<br>");
            sb.Append("E-post: " + Subscriber.Email + "<br>");
            sb.Append("Från datum: " + DateComplaint.ToString("yyyy-MM-dd") + "<br>");
            sb.Append("Antal dagar: " + numDays.ToString() + "<br>");
            sb.Append("Orsak: " + DropDownListReason.SelectedValue);
            return sb.ToString();
        }

    }
}