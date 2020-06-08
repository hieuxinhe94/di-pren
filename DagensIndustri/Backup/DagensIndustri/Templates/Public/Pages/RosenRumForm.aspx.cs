using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.Misc;
using System.Text;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Templates.Public.Pages
{
    public partial class RosenRumForm : DiTemplatePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                SetMainText(true);
            else
            {
                SetMainText(false);
                PlaceHolderForm.Visible = false;
            }
        }

        private void SetMainText(bool isBeforeSend)
        {
            string key = isBeforeSend ? "TextBeforeSend" : "TextAfterSend";

            if (IsValue(key))
                Mainbody1.Text = CurrentPage.Property[key].ToString();
        }

        protected void Send_Click(object sender, EventArgs e)
        {
            string toEmail;

            if (rbSthlm.Checked)
                toEmail = IsValue("MailStockholm") ? CurrentPage.Property["MailStockholm"].ToString().Trim() : string.Empty;
            else
                toEmail = IsValue("MailGoteborg") ? CurrentPage.Property["MailGoteborg"].ToString().Trim() : string.Empty;

            if (!MiscFunctions.IsValidEmail(toEmail))
                toEmail = "pren@di.se";

            string custEmail = email.Text.ToString().Trim();
            if (!MiscFunctions.IsValidEmail(custEmail))
                custEmail = "no-reply@di.se";

            MiscFunctions.SendMail(custEmail, toEmail, "Bokningsförfrågan Rosenrummet", GetMailBody(), true);
            LogEvent();
        }

        private void LogEvent()
        {
            long? cusno = null;
            if (HttpContext.Current.User.Identity.IsAuthenticated)
                cusno = MembershipDbHandler.GetCusno(HttpContext.Current.User.Identity.Name);

            new Logger(Settings.LogEvent_RosenRummetBooking, cusno, true);
        }


        private string GetMailBody()
        {
            string city = (rbSthlm.Checked) ? "Stockholm" : "Göteborg";

            StringBuilder sb = new StringBuilder();
            sb.Append(firstName.Text.ToString() + " " + lastName.Text.ToString() + "<br>");
            sb.Append(email.Text.ToString() + "<br>");
            sb.Append(phone.Text.ToString() + "<br>");
            sb.Append("Önskad dag/tid: " + date.Text.ToString() + " " + time.Text.ToString() + "<br>");
            sb.Append("Antal personer: " + numPersons.Text.ToString() + "<br>");
            sb.Append("Stad: " + city + "<br>");

            return sb.ToString();
        }

    }
}