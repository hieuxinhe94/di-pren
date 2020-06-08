using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EPiServer;
using DagensIndustri.Tools.Classes;
using DIClassLib.DbHandlers;
using DIClassLib.DbHelpers;
using DIClassLib.Misc;
using DIClassLib.CardPayment;
using DIClassLib.Subscriptions;
using System.Text;

namespace DagensIndustri.Templates.Public.Units.Placeable.MySettings2
{
    public partial class Invoice : UserControlBase
    {
        
        #region Properties
        
        /// <summary>
        /// Get the container page of this usercontrol
        /// </summary>
        //private Pages.MySettings2 MySettingsPage
        //{
        //    get
        //    {
        //        return (Pages.MySettings2)Page;
        //    }
        //}

        private DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2 MySettingsPage
        {
            get
            {
                return new DagensIndustri.Templates.Public.Pages.UserSettings.MySettings2();  //(DagensIndustri.Templates.Public.Pages.MySettings2)Page;
            }
        }

        /// <summary>
        /// Get the Subscriber object from the container page of this usercontrol
        /// </summary>
        public SubscriptionUser2 Subscriber
        {
            get
            {
                return (SubscriptionUser2)MySettingsPage.Subscriber;
            }
        }
        
        #endregion


        #region Events
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (!IsPostBack)
            {
                if (MySettingsPage.UserIsAuthenticated)
                {
                    //Init user, set properties
                    FillInvoices();
                    GetAutowhithdrawalTables();
                }
                //MAC will be returned from Posten after payment
                if (Request.QueryString["MAC"] != null)
                {
                    AurigaReturn ret = new AurigaReturn(EPiFunctions.GetCardPayFailPageUrl());
                    ret.HandleAurigaReturn(CurrentPage.LinkURL, Subscriber.Email);
                }
            }
        }

        /// <summary>
        /// Create a script for selection of the tab and div when PayInvoiceButton is clicked.
        /// If there is a invoice addressee, get that customer's details.
        /// Check if invoice is already payed, if so, disable pay button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //protected void InvoiceRepeater_ItemDataBound(object sender, RepeaterItemEventArgs e)
        //{
        //    if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
        //    {
        //        //Create a script for selection of the tab and div when PayInvoiceButton is clicked
        //        HtmlGenericControl invoiceDiv = e.Item.FindControl("InvoiceDiv") as HtmlGenericControl;
        //        Button payInvoiceButton = e.Item.FindControl("PayInvoiceButton") as Button;

        //        payInvoiceButton.OnClientClick = MySettingsPage.CreateSelectedTabScript(MySettingsPage.HyperLinkInvoice.NavigateUrl, invoiceDiv.ClientID);


        //        //Find out if invoice is payed. If payed disable button. Payed = 1, Not payed = 0
        //        try
        //        {
        //            string invoiceID = ((DataRowView)e.Item.DataItem).Row["INVNO"].ToString();
        //            string result = SqlHelper.ExecuteScalar("DisePren", "CardPayGetCompleteInvoicePayment2", new SqlParameter("@invoiceNumber", invoiceID)).ToString();
        //            payInvoiceButton.Visible = result != "1";

        //            Label invoiceStatusLabel = e.Item.FindControl("InvoiceStatusLabel") as Label;
        //            invoiceStatusLabel.Text = result != "1" ? Translate("/mysettings/invoice/unpaid") : Translate("/mysettings/invoice/paid");
        //        }
        //        catch (Exception ex)
        //        {
        //            new Logger("LbPayInvoiceOnDataBinding() - failed", ex.ToString());
        //            MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
        //        }


        //        //If there is a invoice addressee, get that customer's details
        //        //if (Subscriber.PayCustomerNumber > 0)
        //        //{
        //            //Literal addresseeNameLiteral = e.Item.FindControl("AddresseeNameLiteral") as Literal;
        //            //Literal addresseeAddressLiteral = e.Item.FindControl("AddresseeAddressLiteral") as Literal;
        //            //Literal addresseeCareOfLiteral = e.Item.FindControl("AddresseeCareOfLiteral") as Literal;
        //            //Literal addresseeZipCodeLiteral = e.Item.FindControl("AddresseeZipCodeLiteral") as Literal;
        //            //Literal addresseeCityLiteral = e.Item.FindControl("AddresseeCityLiteral") as Literal;
                    
        //            //SubscriptionUser payCustomer = new SubscriptionUser(Subscriber.PayCustomerNumber);

        //            //addresseeNameLiteral.Text = payCustomer.Name;
        //            //addresseeAddressLiteral.Text = payCustomer.Address;
        //            //addresseeCareOfLiteral.Text = payCustomer.Co;
        //            //addresseeZipCodeLiteral.Text = payCustomer.Zip;
        //            //addresseeCityLiteral.Text = payCustomer.PostName;
        //        //}                
        //    }
        //}

        /// <summary>
        /// On click on linkbutton "Pay"
        /// Calls saveDataBeforePostensPage and then redirects to posten
        /// </summary>
        protected void PayInvoice_Click(object sender, EventArgs e)        
        {
            //Get commandargument form Button containing OPENAMOUNT, INVAMOUNT, VATAMOUNT, INVNO
            string invoiceInfo = ((Button)sender).CommandArgument;
            string[] invoiceArr = invoiceInfo.Split('|');


            double price = double.Parse(invoiceArr[0]);
            double priceExcl = double.Parse(invoiceArr[1]);
            double invoiceVAT = double.Parse(invoiceArr[2]);
            double vatAmount = price - priceExcl;
            string responseURL = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string cancelURL = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string goodsDescription = invoiceArr[3]; //INVNO
            long invoiceNumber = Convert.ToInt64(invoiceArr[3]); //INVNO
            string comment = "Obetald faktura sidan";
            string consumerName = "Cusno " + Subscriber.Cusno;
            string emailAddress = Subscriber.Email;
            string merchID = EPiFunctions.SettingsPageSetting(CurrentPage, "MerchIDInvoice").ToString() ?? "";

            AurigaPrepare aurigaPrepare = new AurigaPrepare(price, vatAmount, null, true, false, responseURL, cancelURL, goodsDescription, merchID, comment, consumerName, emailAddress, invoiceNumber, null, null);
        }

        //protected void ButtonCancelSubs_Click(object sender, EventArgs e)
        //{ }

        #endregion


        #region Methods

        /// <summary>
        /// Initialize repeater with invoices
        /// </summary>
        private void FillInvoices()
        {
            //try
            //{
            //    MessagePlaceHolder.Visible = false;
            //    //other payer - do not show invoices
            //    if (Subscriber.PayCustomerNumber > 0)
            //    {
            //        MessagePlaceHolder.Visible = true;
            //    }
            //    else
            //    {
            //        //Get open invoices and bind to repeater
            //        //Check paycustomernumber
            //        DataSet customerInvoices = Subscriber.GetOpenInvoices();

            //        if (customerInvoices != null && customerInvoices.Tables[0].Rows.Count > 0)
            //        {
            //            InvoiceRepeater.DataSource = customerInvoices;
            //            InvoiceRepeater.DataBind();
            //        }
            //        else
            //        {
            //            MessagePlaceHolder.Visible = true;
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    new Logger("FillInvoices() - failed", ex.ToString());
            //    MySettingsPage.ShowMessage("/mysettings/errors/error", true, true);
            //}
        }

        private void GetAutowhithdrawalTables()
        {
            StringBuilder sb = new StringBuilder();
            DataSet ds = MsSqlHandler.GeAwdsForCust(Subscriber.Cusno);
            
            if (ds != null && ds.Tables != null && ds.Tables[0] != null && ds.Tables[0].Rows != null)
            {
                long subsIdTmp = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    long aurigaSubsId = long.Parse(dr["aurigaSubsId"].ToString());

                    if (subsIdTmp != aurigaSubsId)
                        sb.Append(GetAwdTableHead(dr));

                    sb.Append(GetAwdTablePayment(dr));

                    subsIdTmp = aurigaSubsId;
                }
            }

            LiteralAwdTables.Text = sb.ToString();
        }

        private string GetAwdTableHead(DataRow dr)
        {
            #region dr props
            //aurigasubsid
            //cusno
            //subsno
            //campno
            //amount
            //vat
            //status
            //status_code
            //purchase_date
            //datestopped
            #endregion

            long subsno = long.Parse(dr["subsno"].ToString());

            StringBuilder sb = new StringBuilder();
            sb.Append("<table border=0 cellpadding=0 cellspacing=0>");
            sb.Append("<tr>");
            sb.Append("<td>Prenumerationsnummer</td>");
            sb.Append("<td width=15 rowspan=10>&nbsp;</td>");
            sb.Append("<td>" + subsno.ToString() + "</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>Prenumerationstyp</td>");
            sb.Append("<td>Di+</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>Månadspris</td>");
            sb.Append("<td>" + (int.Parse(dr["amount"].ToString()) / 100).ToString() + ":- varav moms " + (int.Parse(dr["vat"].ToString()) / 100).ToString() + ":-</td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>Startdatum</td>");
            sb.Append("<td>" + Convert.ToDateTime(dr["purchase_date"]).ToString("yyyy-MM-dd") + "</td>");
            sb.Append("</tr>");
            //sb.Append("<tr>");
            //sb.Append("<td>Avslutad datum</td>");
            
            //string dateStopped = string.IsNullOrEmpty(dr["dateStopped"].ToString()) ? "-" : Convert.ToDateTime(dr["dateStopped"]).ToString("yyyy-MM-dd");
            //sb.Append("<td>" + dateStopped + "</td>");

            //sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>Kommande betalning</td>");

            string nextPayDate = "";
            List<DIClassLib.Subscriptions.Subscription> subs = CirixDbHandler.GetSubscriptions2(long.Parse(dr["cusno"].ToString()));
            foreach (DIClassLib.Subscriptions.Subscription sub in subs)
            {
                if (subsno == sub.SubsNo)
                {
                    nextPayDate = sub.SubsEndDate.Date.AddDays(-int.Parse(MiscFunctions.GetAppsettingsValue("awdNumDaysFwdPayIntervalEnd"))).ToString("yyyy-MM-dd");
                    break;
                }
            }
            sb.Append("<td>" + nextPayDate + "</td>");

            sb.Append("</tr>");
            sb.Append("</table>");
            sb.Append("<br>");
            return sb.ToString();
        }

        private string GetAwdTablePayment(DataRow dr)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("<table border=0 width=370 cellpadding=0 cellspacing=0>");
            sb.Append("<tr>");
            sb.Append("<td width=80><b>Bet.datum</b></td>");
            sb.Append("<td width=290><b>Notering</b></td>");
            sb.Append("</tr>");
            sb.Append("<tr>");
            sb.Append("<td>" + Convert.ToDateTime(dr["purchase_date"]).ToString("yyyy-MM-dd") + "</td>");
            sb.Append("<td>");

            if (dr["status"].ToString() == "A" && dr["status_code"].ToString() == "0")
                sb.Append("Betalning genomförd (" + (int.Parse(dr["amount"].ToString()) / 100).ToString() + ":- varav moms " + (int.Parse(dr["vat"].ToString()) / 100).ToString() + ":-)");
            else
                sb.Append("Tekniskt fel. Kontakta kundtjänst.");

            sb.Append("</td>");
            sb.Append("</tr>");
            sb.Append("</table>");
            return sb.ToString();
        }

        #endregion

    }
}