using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EPiServer;
using EPiServer.Core;
using System.Text;
using DIClassLib.Misc;
using DIClassLib.SingleSignOn;
using System.Data;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;
using DagensIndustri.Tools.Classes.BaseClasses;


namespace DagensIndustri.Templates.Public.Units.Placeable.SingleSingOn
{
    public partial class SendCodeToEmail : UserControlBase
    {
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        protected void ButtonSendCode_Click(object sender, EventArgs e)
        {
            long cusno = 0;
            string email = "";
            string searchStr = MiscFunctions.REC(TextBoxRemind.Text);

            if (string.IsNullOrEmpty(searchStr))
            {
                ShowMess("Var god ange kundnummer eller e-postadress", true);
                return;
            }

            if (MiscFunctions.IsNumeric(searchStr))
                cusno = long.Parse(searchStr);

            if (MiscFunctions.IsValidEmail(searchStr))
            {
                email = searchStr;
                List<long> cusnos = SubscriptionController.GetCusnosByEmail(email);
                if (cusnos.Count == 1)
                    cusno = cusnos[0];
                else
                {
                    string s = (cusnos.Count == 0) ? "Angiven e-postadress återfanns inte" : "Angiven e-postadress var inte unik";
                    ShowMess(s + " i vårt system. Ange om möjligt kundnummer och försök igen. Var god kontakta kundtjänst om du inte kommer vidare.", true);
                    return;
                }   
            }

            if (cusno <= 0)
            {
                ShowMess("Ingen kund hittades. Var god kontakta kundtjänst.", true);
                return;
            }


            SsoConnectRow ssoRow = new SsoConnectRow(cusno);
            if (!string.IsNullOrEmpty(ssoRow.TryPopulareErr))
            {
                ShowMess(ssoRow.TryPopulareErr, true);
                return;
            }

            if (!MiscFunctions.IsValidEmail(email))
            {
                email = ssoRow.CirixEmail;
                if (!MiscFunctions.IsValidEmail(email))
                    email = TryGetEmailFromCirix(cusno);

                if (!MiscFunctions.IsValidEmail(email))
                {
                    ShowMess("Din kod kunde tyvärr inte skickas till dig. Ingen giltig e-postadress hittades för kundnummer " + cusno.ToString() + ". Var god kontakta kundtjänst.", true);
                    return;
                }
            }

            SendEmail(email, ssoRow.CustomerCode);
            StringBuilder sb = new StringBuilder();
            sb.Append("Koden har nu skickats till din e-postadress. ");
            sb.Append("Fyll i koden i rutan nedan för att aktivera dina digitala tjänster. ");
            sb.Append("Var god kontakta kundtjänst om du inte har fått ett mail inom kort.");
            ShowMess(sb.ToString(), false);

            this.Visible = false;
        }


        private string TryGetEmailFromCirix(long cusno)
        {
            DataSet ds = SubscriptionController.GetCustomer(cusno);
            if (DbHelpMethods.DataSetHasRows(ds))
            {
                return ds.Tables[0].Rows[0]["emailaddress"].ToString();

                #region old code
                //string userid = dr["wwwuserid"].ToString();
                //string passwd = CirixDbHandler.GetWWWPassword(long.Parse(cusnoStr));

                //StringBuilder sb = new StringBuilder();
                //if (!MiscFunctions.IsValidEmail(email))
                //    sb.Append("- Giltig mailadress saknas.<br>");
                //if (string.IsNullOrEmpty(userid))
                //    sb.Append("- Användarnamn saknas.<br>");
                //if (string.IsNullOrEmpty(passwd))
                //    sb.Append("- Lösenord saknas.<br>");
                #endregion
            }

            return string.Empty;
        }

        private void SendEmail(string email, string code)
        {
            string mailSubject = "Kod till Dagens industri";
            StringBuilder sb = new StringBuilder();
            sb.Append("Hej,<br><br>");
            sb.Append("Du har begärt att få en engångskod för att aktivera dina digitala tjänster hos Dagens industri.<br><br>");
            sb.Append("Din kod är: " + code + "<br><br>");
            sb.Append("Skriv in koden eller kopiera koden och klistra in den på registreringssidan. ");
            sb.Append("För att hitta tillbaka till sidan där koden ska registreras <a href='http://dagensindustri.se/mittDi'>klicka här</a> så kommer du till rätt sida.<br><br>");
            sb.Append("Om du inte begärt någon kod från Dagens industri så ber vi dig bortse från detta mail.<br><br>");
            sb.Append("MVH<br>");
            sb.Append("Dagens industri<br>");
            sb.Append("<a href='mailto:pren@di.se'>pren@di.se</a><br>");
            sb.Append("Tel: 08-573 651 00");
            MiscFunctions.SendMail("no-reply@di.se", email, mailSubject, sb.ToString(), true);
        }

        private void ShowMess(string mess, bool isError)
        {
            ((DiTemplatePage)Page).ShowMessage(mess, false, isError);
            ((DiTemplatePage)Page).UserMessageControl.Visible = true;
        }


    }
}