using System;
using System.Collections;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Data.SqlClient;
using System.IO;
using EPiServer;
using EPiServer.PlugIn;
using System.Data;
using System.Net;
using System.Collections.Generic;
using DIClassLib.DbHandlers;
using DIClassLib.Misc;
using DIClassLib.BonnierDigital;
using System.Text;
using EPiServer.Core;
//using EPiServer.Utility.DbHandlers;

namespace DagensIndustri.Tools.Admin.CustomerInfo
{
    [GuiPlugIn(Area = PlugInArea.AdminMenu, Description = "Kundsök", RequiredAccess = EPiServer.Security.AccessLevel.Administer, DisplayName = "Kundsök", UrlFromUi = "/Tools/Admin/CustomerInfo/FindCustomer.aspx", SortIndex = 2010)]
    public partial class FindCustomer : System.Web.UI.Page
    {
        public List<CustInfo> FoundCusts = new List<CustInfo>();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            //Register javascript for jquery
            this.Page.ClientScript.RegisterClientScriptInclude("jq", "/Tools/Admin/Styles/Scripts/jquery-1.4.2.min.js");
            this.Page.ClientScript.RegisterClientScriptInclude("jqcustom", "/Tools/Admin/Styles/Scripts/jquery-ui-1.8.2.custom.min.js");

            //StringBuilder sb = new StringBuilder();
            //sb.Append(LanguageManager.Instance.Translate("/findCustomer/mailHeader"));
        }

        protected void BtnSearchOnClick(object sender, EventArgs e)
        {
            try
            {
                ClearEmailForm();
                string sName1 = string.IsNullOrEmpty(TxtCompany.Text) ? TxtLastName.Text + " " + TxtFirstName.Text : TxtCompany.Text;
                string sName2 = string.IsNullOrEmpty(TxtCompany.Text) ? string.Empty : TxtLastName.Text + " " + TxtFirstName.Text;
                string sEmail = TxtEmail.Text;
                string sPhone = TxtPhone.Text;
                string sStreet = TxtStreet.Text;
                string sHouseNo = TxtStreetNo.Text;
                string sZipCode = TxtZip.Text;
                string sPostName = TxtCity.Text;

                if (string.IsNullOrEmpty((sName1 + sName2 + sEmail + sPhone + sStreet + sHouseNo + sZipCode + sPostName).Trim(' ')))
                {
                    LblError.Text = "Du måste ange minst ett värde";
                    return;
                }

                //LblError.Text = "sName1: '" + sName1.Trim(' ').ToUpper() + "'<br />sName2: '" + sName2.Trim(' ').ToUpper() + "'";

                DataSet ds = SubscriptionController.FindCustomers(0, 0, 0,
                                                           sName1.Trim(' ').ToUpper(),  //sname1
                                                           sName2.Trim(' ').ToUpper(),  //sname2
                                                           "",                          //sname3
                                                           sPhone.Trim(' '),            //phone
                                                           sEmail.Trim(' '),            //email
                                                           sStreet.Trim(' ').ToUpper(), //sStreet
                                                           sHouseNo.Trim(' '),          //sHouseNo
                                                           "",                          //sStairCase
                                                           "",                          //sApartment
                                                           "",                          //sCountry
                                                           sZipCode.Trim(' '),          //sZipCode
                                                           "", //sUserId
                                                           sPostName.Trim(' ').ToUpper() //sPostName
                                                           );
                GvCustomers.DataSource = ds;
                GvCustomers.DataBind();
            }
            catch (Exception ex)
            {

                ShowError("Ett fel uppstod: " + ex.Message);
            }
        }

        protected void LbShowSubscription(object sender, EventArgs e)
        {
            try
            {
                string cusNo = ((LinkButton)sender).CommandArgument;

                GvSubscription.DataSource = SubscriptionController.GetSubscriptions(long.Parse(cusNo), true);
                GvSubscription.DataBind();

                if (GvSubscription.Rows.Count < 1)
                    LbSubInfo.Text = "Prenumerationsuppgifter saknas <br/><br />";

                ShowModalWindow("subscription", "900", "auto");
            }
            catch (Exception ex)
            {
                ShowError("Ett fel uppstod: " + ex.Message);
            }
        }

        private void ShowModalWindow(string id, string width, string height)
        {
            Page.ClientScript.RegisterClientScriptBlock(
                GetType(),
                id,
                "<script type='text/javascript'>$(function() {showmodal('#" + id + "','" + width + "px', '" + height + "', true);});</script>");
        }

        private void ShowError(string strMessage)
        {
            LblError.Text = strMessage;
        }


        protected void GvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                long cusno = long.Parse(DataBinder.Eval(e.Row.DataItem, "CUSNO").ToString());
                string email = DataBinder.Eval(e.Row.DataItem, "EMAILADDRESS").ToString();
                CustInfo ci = TryAddFoundCust(cusno, email);

                if (MiscFunctions.IsValidEmail(ci.Email) && ci.EmailInServicePlus)
                    e.Row.Cells[11].Text = "Har konto";
                else
                    e.Row.Cells[11].Text = "<a href='" + EPiServer.Configuration.Settings.Instance.SiteUrl.ToString() + "mittdi?scode=" + ci.Code + "' target='_blank'>Skapa konto</a>";

                LinkButton lb = (LinkButton)e.Row.Cells[12].Controls[1];
                lb.CommandArgument = cusno.ToString() + "|" + ci.Code + "|" + email;

            }
        }

        private CustInfo TryAddFoundCust(long cusno, string email)
        {
            foreach (CustInfo ci in FoundCusts)
            {
                if (cusno == ci.Cusno)
                    return ci;
            }

            CustInfo ci2 = new CustInfo(cusno, email);
            FoundCusts.Add(ci2);
            return ci2;
        }

        protected void LinkButtonPopMailForm_Click(object sender, EventArgs e)
        {
            ClearEmailForm();

            string arg = ((LinkButton)sender).CommandArgument.ToString();
            string[] arr = arg.Split('|');

            TextBoxSendMailCusno.Text = arr[0];
            TextBoxSendMailCode.Text = arr[1];
            TextBoxSendMailEmail.Text = arr[2];
        }

        protected void ButtonSendMail_OnClick(object sender, EventArgs e)
        {
            string email = TextBoxSendMailEmail.Text;

            if (!MiscFunctions.IsValidEmail(email))
            {
                LabelSendMailMess.Text = "Ogiltig e-postadress";
                return;
            }

            if (GetNumSelectedCheckBoxes() == 0)
            {
                LabelSendMailMess.Text = "Välj minst en checkbox";
                return;
            }

            SendMail(email, TextBoxSendMailCusno.Text.Trim(), TextBoxSendMailCode.Text.Trim());

            ClearEmailForm();

            LabelSendMailMess.Text = "E-post har skickats till: " + email;
        }

        private int GetNumSelectedCheckBoxes()
        {
            int i = 0;
            foreach (ListItem li in CheckBoxListSendMail.Items)
                if (li.Selected) i++;

            return i;
        }

        private void SendMail(string email, string cusno, string code)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Hej,");
            sb.Append("<br><br>");
            sb.Append("Tack för en trevlig pratstund!");
            sb.Append("<br><br>");

            //link to camp page
            if (CheckBoxListSendMail.Items[0].Selected)
            {
                sb.Append("Här kommer länken till att prova Di en månad, enligt överenskommelse.<br>");
                sb.Append("<a href='http://dagensindustri.se/kampanj/konferens?tg=EVENT' target='_blank'>Klicka här för ditt erbjudande</a>");
                sb.Append("<br><br>");
            }

            //link to activate dig subs
            if (CheckBoxListSendMail.Items[1].Selected)
            {
                sb.Append("Här kommer länken där du enkelt aktiverar dina digitala tjänster som ingår i din prenumeration.<br>");
                sb.Append("<a href='" + EPiServer.Configuration.Settings.Instance.SiteUrl.ToString() + "mittdi");
                
                if (!string.IsNullOrEmpty(code))
                    sb.Append("?scode=" + code + "'");

                sb.Append(" target='_blank'>Klicka här</a>");
                sb.Append("<br><br>");
            }

            //link to forgot passwd
            if (CheckBoxListSendMail.Items[2].Selected)
            {
                sb.Append("Här kommer länken där du kan byta ditt lösenord så att du kan nyttja dina digitala tjänster som ingår i din prenumeration.<br>");
                sb.Append("<a href='https://login.di.se/password/forgot-password' target='_blank'>Klicka här</a>");
                sb.Append("<br><br>");
            }

            //link to Di Gold
            if (CheckBoxListSendMail.Items[3].Selected)
            {
                sb.Append("Här kommer länken för att bli medlem i Di Guld där du kan del av flera tjänster med ren affärsnytta. Du får även attraktiva erbjudanden som är exklusiva för våra prenumeranter.<br>");
                sb.Append("<a href='http://dagensindustri.se/diguld' target='_blank'>Klicka här</a>");
                sb.Append("<br><br>");
            }

            sb.Append("<br>");
            //sb.Append("Önskar dig en fortsatt trevlig sommar!");
            //sb.Append("<br><br>");
            sb.Append("Med vänliga hälsningar,<br>");
            sb.Append("Dagens industri<br>");
            sb.Append("08-573 651 00<br>");
            sb.Append("<a href='mailto:pren@di.se'>pren@di.se</a>");

            MiscFunctions.SendMail("pren@di.se", email, "Dagens industri", sb.ToString(), true);
        }

        protected void ButtonSendMailClearForm_OnClick(object sender, EventArgs e)
        {
            ClearEmailForm();
        }

        private void ClearEmailForm()
        {
            TextBoxSendMailEmail.Text = "";
            TextBoxSendMailCusno.Text = "";
            TextBoxSendMailCode.Text = "";
            LabelSendMailMess.Text = "";

            foreach (ListItem li in CheckBoxListSendMail.Items)
                li.Selected = false;
        }

    }

    public class CustInfo
    {
        public long Cusno;
        public string Email;

        string _code;
        public string Code
        {
            get 
            {
                if (!string.IsNullOrEmpty(_code))
                    return _code;

                _code = MsSqlHandler.GetSsoCustomerCode(Cusno);
                return _code;
            }
        }

        public bool EmailInServicePlus;
        public string ServicePlusUserId;


        public CustInfo(long cusno, string email)
        {
            Cusno = cusno;
            Email = email;

            if (!string.IsNullOrEmpty(Email))
            {
                SearchOutput searchRes = RequestHandler.SearchByEmail(Email);
                EmailInServicePlus = (int.Parse(searchRes.totalItems) > 0) ? true : false;

                if (EmailInServicePlus)
                    ServicePlusUserId = searchRes.users.id;
            } 
        }
    }
}