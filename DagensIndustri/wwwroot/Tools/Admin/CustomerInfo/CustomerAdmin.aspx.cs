using System;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using DIClassLib.DbHandlers;
using EPiServer.PlugIn;
using System.Data;
using DIClassLib.DbHelpers;
using DIClassLib.SingleSignOn;
using System.Text;
using DIClassLib.Misc;

namespace DagensIndustri.Tools.Admin.CustomerInfo
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Kundinformation", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Kundinformation", UrlFromUi = "/Tools/Admin/CustomerInfo/CustomerAdmin.aspx", SortIndex = 1010)]
    public partial class CustomerAdmin : System.Web.UI.Page
    {

        protected override void OnLoad(EventArgs e)
        {
            LblError.Text = "";
            base.OnLoad(e);
        }

        protected void BtnSearchOnClick(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void DoSearch()
        {
            try
            {
                RepSearchResult.DataSource = SqlHelper.ExecuteDataset("DISEPren", "GetExpCustomerInfo", new SqlParameter(DdlCriteria.SelectedValue, TxtSearchString.Text.Replace("*", "%")));
                RepSearchResult.DataBind();
            }
            catch (Exception ex)
            {

                ShowError("Ett fel uppstod: " + ex.Message);
            }

        }

        protected void LbUnlockOnClick(object sender, EventArgs e)
        {
            try
            {
                string cusNo = ((LinkButton)sender).CommandArgument;
                //SqlHelper.ExecuteNonQuery("DISEPren", "UnlockExpCustomer", new SqlParameter[] { new SqlParameter("@cusno", cusNo) });

                DoSync(int.Parse(cusNo));
                DoSearch();
            }
            catch (Exception ex)
            {
                ShowError("Ett fel uppstod: " + ex.Message);
            }
        }

        private void ShowError(string strMessage)
        {
            LblError.Text = strMessage;
        }


        protected void ButtonSyncByCusno_Click(object sender, EventArgs e)
        {
            try
            {
                DoSync(int.Parse(TextBoxCusno.Text));
            }
            catch
            {
                LblError.Text = "Synkning misslyckades - angav du kundnummret som ett tal?";
                //new Logger("ButtonSyncByCusno_Click() failed", ex.ToString());
            }
        }



        protected void ButtonSsoGetCode_Click(object sender, EventArgs e)
        {
            LabelSsoCodeMess.Visible = true;
            PlaceHolderSsoCodeInfo.Visible = false;

            try
            {
                long cusno = 0;
                if (long.TryParse(TextBoxSsoCusno.Text, out cusno))
                {                     
                    if (ChkNewMyPage.Checked)
                    {
                        var ecusno = SubscriptionController.GetEcusnoByCustomer(cusno);
                        if (ecusno > 0)
                        {
                            TextBoxSsoCode.Text = ecusno.ToString();
                            TextBoxSsoEmail.Text = SubscriptionController.GetEmailAddress(cusno);
                            LabelSsoCodeMess.Visible = false;
                            PlaceHolderSsoCodeInfo.Visible = true;
                        }
                        else
                        {
                            LabelSsoCodeMess.Text = "Ingen kod hittades för angivet kundnr";   
                        }
                    }
                    else
                    {
                        DataSet ds = MsSqlHandler.SsoGetCustRow(cusno);
                        if (DbHelpMethods.DataSetHasRows(ds))
                        {
                            SsoConnectRow row = new SsoConnectRow(ds);
                            TextBoxSsoCode.Text = row.CustomerCode;
                            TextBoxSsoEmail.Text = SubscriptionController.GetEmailAddress(row.CirixCusno);
                            LabelSsoCodeMess.Visible = false;
                            PlaceHolderSsoCodeInfo.Visible = true;
                        }
                        else
                            LabelSsoCodeMess.Text = "Ingen kod hittades för angivet kundnr";
                    }
                }
                else
                    LabelSsoCodeMess.Text = "Kundnumret måste vara ett tal";
            }
            catch
            {
                LabelSsoCodeMess.Text = "Hämta kod misslyckades";
            }
        }

        protected void ButtonSsoSendCodeMail_Click(object sender, EventArgs e)
        {
            if (MiscFunctions.IsValidEmail(TextBoxSsoEmail.Text))
            {
                if (ChkNewMyPage.Checked)
                {
                    SendEmailNew(TextBoxSsoEmail.Text);
                }
                else
                {
                    SendEmail(TextBoxSsoEmail.Text);
                }                
                LabelSsoCodeMess.Text = "Mailet har skickats till kunden";
                LabelSsoCodeMess.Visible = true;
                PlaceHolderSsoCodeInfo.Visible = false;
            }
        }

        private void SendEmailNew(string email)
        {
            string mailSubject = "Kod till Dagens industri";
            var sb = new StringBuilder();

            sb.Append("Hej,<br><br>");
            sb.Append("För att kunna ta del av våra digitala tjänster och läsa Di digitalt måste du först skapa ett Di-konto och koppla samman detta med din prenumeration.<br>");
            sb.Append("<ul>");
            sb.Append("<li>Gå till <a href='http://di.se/pren/koppla/?code=" + TextBoxSsoCode.Text + "'>kopplasidan</a></li>");
            //sb.Append("<li>Gå till <a href='http://pren-di-stage.bonnierdigitalservices.se/pren/koppla/?code=" + TextBoxSsoCode.Text + "'>kopplasidan</a></li>");

            sb.Append("<li>Klicka på 'Skapa nytt Di-konto'</li>");
            sb.Append("<li>När du skapat ditt konto så klickar du på 'Koppla'</li>");
            sb.Append("</ul>");
            sb.Append("Du kan nu läsa Di digitalt via din dator, mobil eller läsplatta<br>");
            sb.Append("<ul>");
            sb.Append("<li>Di via dator – läs på <a href='" + MiscFunctions.GetAppsettingsValue("UrlCustAdmin_Tpn") + "'>di.se/di</a></li>");
            sb.Append("<li>Di via mobilen – ladda ner appen via <a href='" + MiscFunctions.GetAppsettingsValue("UrlCustAdmin_MobileItunes") + "'>App Store</a></li>");
            sb.Append("<li>Di via läsplattan – ladda ner appen via <a href='" + MiscFunctions.GetAppsettingsValue("UrlCustAdmin_PadItunes") + "'>App Store</a> eller <a href='" + MiscFunctions.GetAppsettingsValue("UrlCustAdmin_PadGooglePlay") + "'>Google Play</a></li>");
            sb.Append("</ul>");
            sb.Append("<br><br>");
            sb.Append("Vänliga hälsningar,<br>");
            sb.Append("Di Kundtjänst<br>");
            sb.Append("E-post: <a href='mailto:pren@di.se'>pren@di.se</a><br>");
            sb.Append("Tel: 08-573 651 00");

            MiscFunctions.SendMail("no-reply@di.se", email, mailSubject, sb.ToString(), true);
        }

        private void SendEmail(string email)
        {
            string mailSubject = "Kod till Dagens industri";
            var sb = new StringBuilder();
            
            sb.Append("Hej,<br><br>");
            sb.Append("För att kunna ta del av våra digitala tjänster och läsa Di digitalt måste du först skapa ett Di-konto och koppla samman detta med din prenumeration.<br>");
            sb.Append("<ul>");
            sb.Append("<li>Gå till <a href='http://dagensindustri.se'>http://dagensindustri.se</a></li>");
            sb.Append("<li>Håll muspekaren över 'Logga in' högst upp i högra hörnet</li>");
            sb.Append("<li>Klicka på 'Ny inloggning till Di på nätet - Läs mer här'</li>");
            sb.Append("<li>Klicka på 'Skapa nytt Di-konto'</li>");
            sb.Append("</ul>");
            sb.Append("När du skapat ditt Di-konto kommer du till en sida där du ska ange en kod.<br>");
            sb.Append("Din kod är: " + TextBoxSsoCode.Text + "<br><br><br>");
            sb.Append("Du kan nu läsa Di digitalt via din dator, mobil eller läsplatta<br>");
            sb.Append("<ul>");
            sb.Append("<li>Di via dator – läs på <a href='" + MiscFunctions.GetAppsettingsValue("UrlCustAdmin_Tpn") + "'>di.se/di</a></li>");
            sb.Append("<li>Di via mobilen – ladda ner appen via <a href='" + MiscFunctions.GetAppsettingsValue("UrlCustAdmin_MobileItunes") + "'>App Store</a></li>");
            sb.Append("<li>Di via läsplattan – ladda ner appen via <a href='" + MiscFunctions.GetAppsettingsValue("UrlCustAdmin_PadItunes") + "'>App Store</a> eller <a href='" + MiscFunctions.GetAppsettingsValue("UrlCustAdmin_PadGooglePlay") + "'>Google Play</a></li>");
            sb.Append("</ul>");
            sb.Append("<br><br>");
            sb.Append("Vänliga hälsningar,<br>");
            sb.Append("Di Kundtjänst<br>");
            sb.Append("E-post: <a href='mailto:pren@di.se'>pren@di.se</a><br>");
            sb.Append("Tel: 08-573 651 00");

            MiscFunctions.SendMail("no-reply@di.se", email, mailSubject, sb.ToString(), true);
        }


        private void DoSync(int cusno)
        {
            //int status = DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncCustToMssqlAndFlagInExpCust(cusno);
            int status = DIClassLib.EPiJobs.SyncSubs.SyncSubsHandler.SyncCustToMssqlLoginTables(cusno);

            string mess = "";
            if (status == 1)
                mess = "Kundens uppgifter verkar korrekta i prenumerationssystemet. Inlogg bör fungera om kunden har koll på sitt Di-konto.";
            if (status == -1)
                mess = "Kunden tycks inte ha någon aktiv prenumeration. Kundinlogg kommer att nekas.";
            if (status == -2)
                mess = "Nödvändiga kunduppgifter kunde ej hämtas från prenumerationssystemet.";

            LblError.Text = mess;
        }


        protected void ButtonEuroBonus_Click(object sender, EventArgs e)
        {
            LabelEuroBonusMess.Visible = true;
            string ebNumTxt = tbEuroBonusNum.Text.Trim();
            string cusnoTxt = tbEuroBonusCusno.Text.Trim();

            if (!MiscFunctions.IsNumeric(ebNumTxt) || ebNumTxt.Length != 9)
            {
                LabelEuroBonusMess.Text = "EuroBonusnumret måste vara ett 9-siffrigt tal";
                return;
            }

            if (!MiscFunctions.IsNumeric(cusnoTxt))
            {
                LabelEuroBonusMess.Text = "Kundnumret måste vara ett tal";
                return;
            }

            int cusno = int.Parse(cusnoTxt);
                
            if (!MsSqlHandler.MdbInsertExtraInfo(-1, cusno, "Obs! Kund måste ha betalat för kampanj samt ha giltig prenumeration för att poängen ska tilldelas.", ebNumTxt))
            {
                LabelEuroBonusMess.Text = "Ett tekniskt fel uppstod när EuroBonuspoäng skulle sparas till marknadsdatabasen (MDB)";
                return;
            }

            LabelEuroBonusMess.Text = "Kunden kommer att tilldelas EuroBonuspoäng";
        }


    }
}