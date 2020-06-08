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

namespace DagensIndustri.Templates.Public.Pages.DiGasell
{
    public partial class GasellVip : DiTemplatePage
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
            string toEmail = IsValue("ToEmail") ? CurrentPage.Property["ToEmail"].ToString().Trim() : string.Empty;
            if (!MiscFunctions.IsValidEmail(toEmail))
                toEmail = "gasell@di.se";

            string custEmail = email.Text.ToString().Trim();
            if(!MiscFunctions.IsValidEmail(custEmail))
                custEmail = "no-reply@di.se";

            MiscFunctions.SendMail(custEmail, toEmail, "Anmälan till Gasell VIP", GetMailBody(), true);   
        }


        private string GetMailBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<b>VIP-person</b>" + "<br>");
            sb.Append(firstName.Text.ToString() + " " + lastName.Text.ToString() + "<br>");
            sb.Append(company.Text.ToString() + "<br>");
            sb.Append(email.Text.ToString() + "<br><br>");

            sb.Append("<b>Gäst</b>" + "<br>");
            sb.Append(firstName2.Text.ToString() + " " + lastName2.Text.ToString() + "<br>");
            sb.Append(company2.Text.ToString() + "<br>");

            return sb.ToString();
        }

    }
}