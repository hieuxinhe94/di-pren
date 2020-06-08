using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using DagensIndustri.Tools.Classes.BaseClasses;
using DIClassLib.CardPayment.Nets;
using DIClassLib.Subscriptions;
using DagensIndustri.Tools.Classes;
using System.Data;
using System.Data.SqlClient;
using DIClassLib.DbHelpers;
using DIClassLib.DbHandlers;


namespace DagensIndustri.Templates.Public.Pages.UserSettings
{
    public partial class Invoice : DiTemplatePage, IUserSettingsPage
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
                HandleNotLoggedIn();

            if (!IsPostBack)
            {
                //MAC will be returned from Posten after payment
                if (Request.QueryString["responseCode"] != null)
                {
                    NetsCardPayReturn ret = new NetsCardPayReturn(EPiFunctions.GetCardPayFailPageUrl());
                    ret.HandleNetsReturn(CurrentPage.LinkURL + "&pay=1", Subscriber.Email);
                }

                if (Request.QueryString["pay"] != null && Request.QueryString["pay"] == "1")
                    ShowMessage("Din faktura har betalats", false, false);

                GetOpenInvoices();
            }
        }

        private void HandleNotLoggedIn()
        {
            MySettingsMenu1.Visible = false;
            ShowMessage("/mysettings2/notloggedin", true, true);
        }


        protected void PayInvoiceLinkButton_Click(object sender, EventArgs e)
        {
            //Get commandargument form Button containing OPENAMOUNT, INVAMOUNT, VATAMOUNT, INVNO, CustomerNumber, Email
            string invoiceInfo = ((LinkButton)sender).CommandArgument;
            string[] invoiceArr = invoiceInfo.Split('|');

            double price = double.Parse(invoiceArr[0]); //OPENAMOUNT
            double priceExcl = double.Parse(invoiceArr[1]); //INVAMOUNT
            double invoiceVAT = double.Parse(invoiceArr[2]); //VATAMOUNT
            double vatAmount = price - priceExcl;
            string responseURL = EPiFunctions.GetFriendlyAbsoluteUrl(CurrentPage);
            string goodsDescription = invoiceArr[3]; //INVNO
            long invoiceNumber = Convert.ToInt64(invoiceArr[3]); //INVNO
            string comment = "Betala faktura via mina sidor";
            string consumerName = "Cusno " + invoiceArr[4]; //CustomerNumber
            string emailAddress = invoiceArr[5]; //Email            

            var prep = new NetsCardPayPrepare(price, vatAmount, null, true, false, responseURL, goodsDescription, comment, consumerName, emailAddress, invoiceNumber, PaymentMethod.TypeOfPaymentMethod.CreditCard, null);
        }

        
        public void GetOpenInvoices()
        {
            MessagePlaceHolder.Visible = false;
            SearchResultPlaceHolder.Visible = false;

            try
            {
                if (Subscriber.Cusno > 0)
                {
                    //Get open invoices and bind to repeater
                    //Check paycustomernumber
                    var inv = SubscriptionController.GetOpenInvoices(Subscriber.Cusno);

                    if (DbHelpMethods.DataSetHasRows(inv))
                    {
                        //Find out if invoice is payed. If payed but not processed yet by Di personnel, remove from list. Payed = 1, Not payed = 0
                        for (int i = inv.Tables[0].Rows.Count-1; i >= 0; i--)
                        {
                            DataRow dr = inv.Tables[0].Rows[i];
                            string invoiceID = dr["INVNO"].ToString();
                            string result = SqlHelper.ExecuteScalar("DisePren", "CardPayGetCompleteInvoicePayment2", new SqlParameter("@invoiceNumber", invoiceID)).ToString();

                            if (result == "1")
                                inv.Tables[0].Rows.RemoveAt(i);
                        }                           

                        int noOfHits = inv.Tables[0].Rows.Count;
                        if (noOfHits > 0)
                        {
                            InvoiceRepeater.DataSource = inv;
                            InvoiceRepeater.DataBind();

                            //SearchResultLiteral.Text = string.Format(Translate("/payinvoice/showresult"), 1, noOfHits, noOfHits);
                            SearchResultPlaceHolder.Visible = true;
                        }
                        else
                        {
                            //All invoices were paid (but not processed in Cirix). 
                            MessagePlaceHolder.Visible = true;
                        }
                    }
                    else
                    {
                        MessagePlaceHolder.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                new Logger("GetOpenInvoices() - failed", ex.ToString());
                ShowMessage("/common/errors/error", true, true);
            }
        }

        public void ShowMessageFromChildCtrl(string mess, bool isKey, bool isError)
        {
            ShowMessage(mess, isKey, isError);
        }
    }
}