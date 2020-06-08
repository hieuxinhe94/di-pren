using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;

namespace DagensIndustri.Templates.Public.js.maintenanceHandler
{
    /// <summary>
    /// Send maintenance mail
    /// </summary>
    public class MailService : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var name = context.Request.QueryString["name"];
            var phone = context.Request.QueryString["phone"];
            var email = context.Request.QueryString["email"];
            var prefferedContactedBy = context.Request.QueryString["prefferedContactedBy"];

            SendMaintenanceMail(context, name, phone, email, prefferedContactedBy);
        }

        private void SendMaintenanceMail(HttpContext context, string name, string phone, string email, string prefferedContactedBy)
        {
            var body = new StringBuilder();
            body.Append("Inskickade kontaktuppgifter från underhållspopup");
            body.Append(Environment.NewLine);
            body.Append("Namn: " + name);
            body.Append(Environment.NewLine);
            body.Append("Telefonnummer: " + phone);
            body.Append(Environment.NewLine);
            body.Append("E-postadress: " + email);
            body.Append(Environment.NewLine);
            body.Append("Kontaktas helst via: " + prefferedContactedBy);

            var to = !string.IsNullOrEmpty(ConfigurationManager.AppSettings.Get("SendMaintenancePopupEmailsTo"))
                ? ConfigurationManager.AppSettings.Get("SendMaintenancePopupEmailsTo")
                : "kristoffer.jansson@di.se";

            DIClassLib.Misc.MiscFunctions.SendMail(email, to, "Skickat från underhållspopup", body.ToString(), false);

            context.Response.ContentType = "text/plain";
            context.Response.Write("ok");
        }

        public bool IsReusable { get; private set; }
    }
}